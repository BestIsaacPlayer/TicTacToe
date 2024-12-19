using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Sound Setup")]
    [SerializeField] private AudioClip backgroundMusic;
    [SerializeField] private AudioClip coinInsertSound;
    
    [Space]
    [SerializeField] private ManagerParent managerParent;
    
    [Space]
    [SerializeField] private TextMeshProUGUI coinsText;
    
    private int _insertedCoinsAmount;
    private int _coinsAmount;
    private AudioSource _audioSource;

    public Action OnCoinUpdate;

    private void Awake()
    {
        if (!managerParent) Debug.LogError($"The {Utility.Parser.FieldToName(nameof(managerParent))} field in the {gameObject.name} object is unset!");
        OnCoinUpdate += UpdateUI;
        
        managerParent.ScreenOverlayManager.SwitchOverlay(managerParent.ScreenOverlayManager.ScreenOffOverlay, typeof(Board.Controller));
        managerParent.ScreenOverlayManager.OffScreenText.text = "You're out of credits!";
        managerParent.ScreenOverlayManager.SwitchOverlay(managerParent.ScreenOverlayManager.RPCScreenOffOverlay, typeof(RPC.Controller));
        managerParent.ScreenOverlayManager.RPCOffScreenText.text = "ROCK TO START\nSTART TO ROCK";
        _audioSource = managerParent.AudioManager.PlayClip(backgroundMusic, true);
    }
    
    private void UpdateUI() => coinsText.text = _coinsAmount.ToString();

    private void Start()
    {
        managerParent.InputManager.InputActions.Main.Quit.performed += _ => HandleGameExit();
        managerParent.InputManager.InputActions.Main.Enter.performed += _ => InsertCoin();
        managerParent.InputManager.InputActions.Main.Mark.performed += _ => StartGameOnOffScreen();
        managerParent.InputManager.InputActions.Main.Rock.performed += _ => StartRPCOnOffScreen();
    }

    public void StartTicTacToe()
    {
        if (_insertedCoinsAmount < 1) return;
        
        _insertedCoinsAmount--;
        managerParent.TicTacToeBoardManager.ResetTicTacToe?.Invoke();
    }

    public void StartRPC()
    {
        managerParent.RPCBoardManager.ResetRPC?.Invoke();
    }

    private void StartGameOnOffScreen()
    {
        if (managerParent.TicTacToeBoardManager.TicTacToeBoardController != null) return;
        managerParent.GameManager.StartTicTacToe();
    }

    private void StartRPCOnOffScreen()
    {
        if (managerParent.RPCBoardManager.RPCBoardController != null) return;
        managerParent.GameManager.StartRPC();
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

    public void AddCoins(int amount)
    {
        _coinsAmount += amount;
        OnCoinUpdate?.Invoke();
    }

    public void CashOut()
    {
        _coinsAmount += _insertedCoinsAmount;
        _insertedCoinsAmount = 0;
        OnCoinUpdate?.Invoke();
    }

    public void InsertCoin()
    {
        if (_coinsAmount < 1) return;
        managerParent.AudioManager.PlayClip(coinInsertSound);
        _insertedCoinsAmount++;
        _coinsAmount--;
        managerParent.ScreenOverlayManager.OffScreenText.text = "Press the MARK button to start the game!";
    }

    public void HandleGameExit()
    {
        Destroy(_audioSource.gameObject);
        Application.Quit();
    }
}