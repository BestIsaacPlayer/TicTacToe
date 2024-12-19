using System;
using System.Collections;
using System.Linq;
using Board.Cell;
using UnityEngine;
using UnityEngine.InputSystem;
using Utility;
using Random = UnityEngine.Random;

namespace Board
{
    public class Controller : MonoBehaviour
    {
        [Header("Layout Setup")]
        [field: SerializeField] public Cell.Controller[] Cells { get; private set; } = new Cell.Controller[9];
        [SerializeField] private Transform indicatorTransform;

        private Content _playerSide;
        private Content _turkSide;
        private Content _currentSide = Content.X;
        private bool _isGameOver;
        private int _currentCellIndex = 4;
        private ManagerParent _managerParent;
        private Cell.Controller CurrentCell => Cells[_currentCellIndex];
        private Cell.Controller[] EmptyCells => Cells.Where(cell => cell.Content == Content.Empty).ToArray();
        private void OnValidate()
        {
            for (var i = 0; i < 1; ++i)
            {
                if (!Cells[i]) Debug.LogError($"The Element {i} in {Utility.Parser.FieldToName(nameof(Cells))} field in the {gameObject.name} object is unset!");
            }
            
            if (!indicatorTransform) Debug.LogError($"The {Utility.Parser.FieldToName(nameof(indicatorTransform))} field in the {gameObject.name} object is unset!");
        }

        private void Update()
        {
            indicatorTransform.gameObject.SetActive(!_isGameOver);
        }

        private void Awake()
        {
            _managerParent = FindFirstObjectByType<ManagerParent>();
            if (!_managerParent) Debug.LogError($"The {Utility.Parser.FieldToName(nameof(_managerParent))} field in the {gameObject.name} object is unset!");
            _managerParent.ScreenOverlayManager.OffScreenText.text = "You're out of credits!";
            
            (_playerSide, _turkSide) = Random.Range(0, 100) > 49 ? (Content.X, Content.O) : (Content.O, Content.X);
        }

        private void Start()
        {
            _managerParent.InputManager.InputActions.Main.Movement.performed += HandleMovementInput;
            _managerParent.InputManager.InputActions.Main.Mark.performed += HandleMarkInput;

            StartCoroutine(BoardLoadedDelay());
        }

        private IEnumerator BoardLoadedDelay()
        {
            _isGameOver = true;
            _managerParent.ScreenOverlayManager.OffScreenText.text = $"You are {(_playerSide == Content.X ? "X" : "O")}";
            yield return new WaitForSeconds(2f);
            _isGameOver = false;
            
            if (_currentSide == _turkSide)
            {
                _managerParent.ScreenOverlayManager.SwitchOverlay(_managerParent.ScreenOverlayManager.TurkThinkingOverlay, typeof(Controller));
                MarkTurkCell();
            }
            else
            {
                _managerParent.ScreenOverlayManager.SwitchOverlay(_managerParent.ScreenOverlayManager.PlayerMoveOverlay, typeof(Controller));
            }
        }

        private void OnDestroy()
        {
            _managerParent.InputManager.InputActions.Main.Movement.performed -= HandleMovementInput;
            _managerParent.InputManager.InputActions.Main.Mark.performed -= HandleMarkInput;
        }

        private void HandleMovementInput(InputAction.CallbackContext context)
        {
            var direction = context.ReadValue<Vector2>().Vector2ToDirection();
            
            var x = Math.DivRem(_currentCellIndex, 3, out var y);

            switch (direction)
            {
                case Direction.Right:
                    y += 1;
                    if (y > 2) y = 0;
                    break;
                case Direction.Left:
                    y -= 1;
                    if (y < 0) y = 2;
                    break;
                case Direction.Up:
                    x -= 1;
                    if (x < 0) x = 2;
                    break;
                case Direction.Down:
                    x += 1;
                    if (x > 2) x = 0;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            _currentCellIndex = 3 * x + y;
            indicatorTransform.position = CurrentCell.transform.position;
        }

        public void HandleMovementInput(int direction)
        {
            if (_isGameOver) return;
            var x = Math.DivRem(_currentCellIndex, 3, out var y);

            switch (direction)
            {
                case 3:
                    y += 1;
                    if (y > 2) y = 0;
                    break;
                case 2:
                    y -= 1;
                    if (y < 0) y = 2;
                    break;
                case 0:
                    x -= 1;
                    if (x < 0) x = 2;
                    break;
                case 1:
                    x += 1;
                    if (x > 2) x = 0;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            _currentCellIndex = 3 * x + y;
            indicatorTransform.position = CurrentCell.transform.position;
        }

        private void HandleMarkInput(InputAction.CallbackContext context)
        {
            if (_currentSide != _playerSide || CurrentCell.Content != Content.Empty || _isGameOver) return;
            
            StartCoroutine(MarkCell(CurrentCell, _playerSide));
        }

        public void HandleMarkInput() => HandleMarkInput(new InputAction.CallbackContext());

        private void MarkTurkCell()
        {
            if (_currentSide != _turkSide || _isGameOver) return;
            indicatorTransform.position = Cells[4].transform.position;
            _currentCellIndex = 4;
            StartCoroutine(TurkMoveCoroutine());
        }

        private IEnumerator TurkMoveCoroutine()
        {
            var bestCell = GetBestCell();
            yield return new WaitForSeconds(Random.Range(1.5f, 2.75f));
            _managerParent.ScreenOverlayManager.SwitchOverlay(_managerParent.ScreenOverlayManager.PlayerMoveOverlay, typeof(Controller));
            StartCoroutine(MarkCell(bestCell, _turkSide));
        }

        private IEnumerator MarkCell(Cell.Controller cell, Content content)
        {
            cell.MarkCell(content);

            _isGameOver = true;
            var timer = 0.0f;
            var color = cell.spriteRenderer.color;
            while (timer < 1.5f)
            {
                yield return null;
                cell.spriteRenderer.color = new Color(color.r, color.g, color.b, timer / 1.5f);
                timer += Time.deltaTime;
            }
            if (content == _playerSide) _managerParent.ScreenOverlayManager.SwitchOverlay(_managerParent.ScreenOverlayManager.TurkThinkingOverlay, typeof(Controller));
            cell.spriteRenderer.color = new Color(color.r, color.g, color.b, 1.0f);
            _isGameOver = false;
            
            if (GetBoardGameState() != Result.MatchNotOver)
            {
                if (Parser.ParseWinner(_playerSide) == GetBoardGameState())
                {
                    _managerParent.GameManager.AddCoins(3);
                }
                _managerParent.GameManager.HandleGameOver(GetBoardGameState());
                _isGameOver = true;
            }
            _currentSide = Utility.Parser.GetOppositeSide(_currentSide);
            
            if (content == _playerSide) MarkTurkCell();
        }

        private Result GetBoardGameState()
        {
            for (var i = 0; i < 3; ++i)
            {
                var isRowSame = Cells[3 * i].Content == Cells[3 * i + 1].Content &&
                                Cells[3 * i + 1].Content == Cells[3 * i + 2].Content;
                if (isRowSame && Cells[3 * i].Content != Cell.Content.Empty)
                {
                    return Parser.ParseWinner(Cells[3 * i].Content);
                }
            }

            for (var i = 0; i < 3; ++i)
            {
                var isColumnSame = Cells[i].Content == Cells[3 + i].Content &&
                                   Cells[3 + i].Content == Cells[6 + i].Content;

                if (isColumnSame && Cells[i].Content != Content.Empty)
                {
                    return Parser.ParseWinner(Cells[i].Content);
                }
            }

            var isDiagonalSame = new[]
            {
                Cells[0].Content == Cells[4].Content && Cells[4].Content == Cells[8].Content,
                Cells[2].Content == Cells[4].Content && Cells[4].Content == Cells[6].Content,
            };

            if (Cells[4].Content != Content.Empty && (isDiagonalSame[0] || isDiagonalSame[1]))
            {
                return Parser.ParseWinner(Cells[4].Content);
            }

            for (var i = 0; i < 3; ++i)
            {
                for (var j = 0; j < 3; ++j)
                {
                    if (Cells[3 * i + j].Content == Content.Empty) return Result.MatchNotOver;
                }
            }

            return Result.Draw;
        }
        
        private Board.Cell.Controller GetBestCell()
        {
            foreach (var cell in EmptyCells)
            {
                cell.MarkCell(_turkSide, true);
                if (GetBoardGameState() == Parser.ParseWinner(_turkSide))
                {
                    cell.MarkCell(Content.Empty, true);
                    return cell;
                }
                cell.MarkCell(Content.Empty, true);
            }
            
            foreach (var cell in EmptyCells)
            {
                cell.MarkCell(_playerSide, true);
                if (GetBoardGameState() == Parser.ParseWinner(_playerSide))
                {
                    cell.MarkCell(Content.Empty, true);
                    return cell;
                }
                cell.MarkCell(Content.Empty, true);
            }
            
            return EmptyCells[Random.Range(0, EmptyCells.Length)];
        }
    }
}