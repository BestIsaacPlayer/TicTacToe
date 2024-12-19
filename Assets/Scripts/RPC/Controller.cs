using System;
using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;

namespace RPC
{
    public class Controller : MonoBehaviour
    {
        private Content _turkChoice;
        private Content _playerChoice;
        private ManagerParent _managerParent;
        private bool _canMove;

        private void Awake()
        {
            _managerParent = FindFirstObjectByType<ManagerParent>();
            if (_managerParent == null) Debug.LogError($"The {Utility.Parser.FieldToName(nameof(_managerParent))} field in the {gameObject.name} object is unset!");
            
            _turkChoice = Enum.GetValues(typeof(Content)).OfType<Enum>().OrderBy(e => Guid.NewGuid()).Cast<Content>().FirstOrDefault();
            Debug.Log(_turkChoice);
        }

        private void OnDestroy()
        {
            _managerParent.ScreenOverlayManager.SwitchOverlay(_managerParent.ScreenOverlayManager.RPCScreenOffOverlay, typeof(Controller));
        }

        private void Start()
        {
            StartCoroutine(BoardLoadedDelay());
            _managerParent.InputManager.InputActions.Main.Rock.performed += _ => HandleButtonInput(Content.Rock);
            _managerParent.InputManager.InputActions.Main.Paper.performed += _ => HandleButtonInput(Content.Paper);
            _managerParent.InputManager.InputActions.Main.Scissors.performed += _ => HandleButtonInput(Content.Scissors);
        }

        private IEnumerator BoardLoadedDelay()
        {
            _managerParent.ScreenOverlayManager.SwitchOverlay(_managerParent.ScreenOverlayManager.RPCTurkThinkingOverlay, typeof(Controller));
            yield return new WaitForSeconds(2f);
            _managerParent.ScreenOverlayManager.SwitchOverlay(_managerParent.ScreenOverlayManager.PlayerMoveOverlay, typeof(Controller));
            _managerParent.ScreenOverlayManager.RPCMainScreenText.text = "Enter your choice!";
            _canMove = true;
        }

        public void HandleButtonInput(Content content)
        {
            if (!_canMove) return;
            _canMove = false;
            _playerChoice = content;
            HandleGameOver();
        }

        private void HandleGameOver()
        {
            switch (_turkChoice)
            {
                case Content.Rock:
                    switch (_playerChoice)
                    {
                        case Content.Rock:
                            Draw();
                            break;
                        case Content.Paper:
                            PlayerWon();
                            break;
                        case Content.Scissors:
                            PlayerLost();
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                    break;
                case Content.Paper:
                    switch (_playerChoice)
                    {
                        case Content.Rock:
                            PlayerLost();
                            break;
                        case Content.Paper:
                            Draw();
                            break;
                        case Content.Scissors:
                            PlayerWon();
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                    break;
                case Content.Scissors:
                    switch (_playerChoice)
                    {
                        case Content.Rock:
                            PlayerWon();
                            break;
                        case Content.Paper:
                            PlayerLost();
                            break;
                        case Content.Scissors:
                            Draw();
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            StartCoroutine(ResetGame());
        }

        private IEnumerator ResetGame()
        {
            yield return new WaitForSeconds(2f);
            Destroy(gameObject);
        }

        private void Draw()
        {
            _managerParent.ScreenOverlayManager.RPCMainScreenText.text = "It's a draw!";
        }

        private void PlayerWon()
        {
            _managerParent.ScreenOverlayManager.RPCMainScreenText.text = "You win!";
            _managerParent.GameManager.AddCoins(1);
        }

        private void PlayerLost()
        {
            _managerParent.ScreenOverlayManager.RPCMainScreenText.text = "You lose!";
        }
    }
}