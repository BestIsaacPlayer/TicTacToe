﻿using System;
using System.Linq;
using Board.Cell;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

namespace Board
{
    public class Controller : MonoBehaviour
    {
        [field: SerializeField] public Cell.Controller[] Cells { get; private set; } = new Cell.Controller[9];
        [SerializeField] private Transform indicatorTransform;
        private ManagerParent _managerParent;
        private int _currentCellIndex = 4;
        private Cell.Controller CurrentCell => Cells[_currentCellIndex];
        public Cell.Controller[] EmptyCells => Cells.Where(cell => cell.Content == Content.Empty).ToArray();

        private Content _playerSide;
        private Content _turkSide;
        private Content _currentSide = Content.X;
        private void OnValidate()
        {
            for (var i = 0; i < 1; ++i)
            {
                if (!Cells[i]) Debug.LogError($"The Element {i} in {Utility.Parser.FieldToName(nameof(Cells))} field in the {gameObject.name} object is unset!");
            }
            
            if (!indicatorTransform) Debug.LogError($"The {Utility.Parser.FieldToName(nameof(indicatorTransform))} field in the {gameObject.name} object is unset!");
        }

        private void Start()
        {
            _managerParent = FindFirstObjectByType<ManagerParent>();
            if (!_managerParent) Debug.LogError($"The {Utility.Parser.FieldToName(nameof(_managerParent))} field in the {gameObject.name} object is unset!");

            _managerParent.InputManager.InputActions.Main.Movement.performed += HandleMovementInput;
            _managerParent.InputManager.InputActions.Main.Mark.performed += HandleMarkInput;
            

            // TODO
            // Change 0 to 50
            (_playerSide, _turkSide) = Random.Range(0, 100) > -1 ? (Content.X, Content.O) : (Content.O, Content.X);
        }

        private void HandleMovementInput(InputAction.CallbackContext context)
        {
            var direction = Utility.Parser.Vector2ToDirection(context.ReadValue<Vector2>());
            
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

        private void HandleMarkInput(InputAction.CallbackContext context)
        {
            if (_currentSide != _playerSide) return;
            MarkCell(CurrentCell, _playerSide);
        }

        public void MarkTurkCell(Cell.Controller cell)
        {
            if (_currentSide != _turkSide) return;
            
            // TODO
            // Implement AI based cell choosing
            var bestCell = Cells[0];
            MarkCell(bestCell, _turkSide);
        }

        private void MarkCell(Cell.Controller cell, Content content)
        {
            cell.MarkCell(content);
            _currentSide = Utility.Parser.GetOppositeSide(_currentSide);
        }
        
        public Result GetBoardGameState()
        {
            for (var i = 0; i < 3; ++i)
            {
                var isRowSame = Cells[3 * i].Content == Cells[3 * i + 1].Content &&
                                Cells[3 * i + 1].Content == Cells[3 * i + 2].Content;
                if (isRowSame && Cells[3 * i].Content != Cell.Content.Empty)
                {
                    return Utility.Parser.ParseWinner(Cells[3 * i].Content);
                }
            }

            for (var i = 0; i < 3; ++i)
            {
                var isColumnSame = Cells[i].Content == Cells[3 + i].Content &&
                                   Cells[3 + i].Content == Cells[6 + i].Content;

                if (isColumnSame && Cells[i].Content != Content.Empty)
                {
                    return Utility.Parser.ParseWinner(Cells[i].Content);
                }
            }

            var isDiagonalSame = new[]
            {
                Cells[0].Content == Cells[4].Content && Cells[4].Content == Cells[8].Content,
                Cells[2].Content == Cells[4].Content && Cells[4].Content == Cells[6].Content,
            };

            if (Cells[4].Content != Content.Empty && (isDiagonalSame[0] || isDiagonalSame[1]))
            {
                return Utility.Parser.ParseWinner(Cells[4].Content);
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
    }
}