using System;
using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Sound Setup")]
    [SerializeField] private AudioClip backgroundMusic;
    [SerializeField] private AudioClip coinInsertSound;
    
    [Space]
    [SerializeField] private ManagerParent managerParent;
    
    private int _insertedCoinsAmount;
    private AudioSource _audioSource;

    private void Awake()
    {
        if (!managerParent) Debug.LogError($"The {Utility.Parser.FieldToName(nameof(managerParent))} field in the {gameObject.name} object is unset!");
        
        managerParent.ScreenOverlayManager.SwitchOverlay(managerParent.ScreenOverlayManager.ScreenOffOverlay, typeof(Board.Controller));
        managerParent.ScreenOverlayManager.OffScreenText.text = "You're out of credits!";
        _audioSource = managerParent.AudioManager.PlayClip(backgroundMusic, true);
    }

    private void Start()
    {
        managerParent.InputManager.InputActions.Main.Quit.performed += _ => HandleGameExit();
        managerParent.InputManager.InputActions.Main.Enter.performed += _ => InsertCoin();
        managerParent.InputManager.InputActions.Main.Mark.performed += _ => StartGameOnOffScreen();
    }

    public void StartGame()
    {
        if (_insertedCoinsAmount < 1) return;
        
        _insertedCoinsAmount--;
        managerParent.TicTacToeBoardManager.ResetTicTacToe();
    }

    private void StartGameOnOffScreen()
    {
        if (managerParent.TicTacToeBoardManager.TicTacToeBoardController != null) return;
        managerParent.GameManager.StartGame();
    }

    public void HandleGameOver(Board.Result result)
    {
        Destroy(managerParent.TicTacToeBoardManager.TicTacToeBoardController.gameObject);
        StartCoroutine(HandleGameEndDisplay(result));
    }

    private IEnumerator HandleGameEndDisplay(Board.Result result)
    {
        managerParent.ScreenOverlayManager.SwitchOverlay(managerParent.ScreenOverlayManager.PlayerMoveOverlay, typeof(Board.Controller));
        managerParent.ScreenOverlayManager.MainScreenText.text = result switch {
            Board.Result.XWon => "X won!",
            Board.Result.OWon => "O won!",
            Board.Result.Draw => "It's a draw!",
            Board.Result.MatchNotOver => "Something went terribly wrong!",
            _ => throw new ArgumentOutOfRangeException(nameof(result), result, null)
        };
        yield return new WaitForSeconds(2f);
        managerParent.ScreenOverlayManager.OffScreenText.text = _insertedCoinsAmount < 1 ? "You're out of credits!" : "Press the MARK button to start the game!";
        managerParent.ScreenOverlayManager.MainScreenText.text = string.Empty;
        managerParent.ScreenOverlayManager.SwitchOverlay(managerParent.ScreenOverlayManager.ScreenOffOverlay, typeof(Board.Controller));
    }

    public void InsertCoin()
    {
        managerParent.AudioManager.PlayClip(coinInsertSound);
        _insertedCoinsAmount++;
        managerParent.ScreenOverlayManager.OffScreenText.text = "Press the MARK button to start the game!";
    }

    public void HandleGameExit()
    {
        Destroy(_audioSource.gameObject);
        Application.Quit();
    }
}