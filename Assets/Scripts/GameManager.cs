using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private ManagerParent managerParent;

    public Action CoinAmountChanged;
    private int _insertedCoinsAmount;

    private void Awake()
    {
        if (!managerParent) Debug.LogError($"The {Utility.Parser.FieldToName(nameof(managerParent))} field in the {gameObject.name} object is unset!");
    }

    private void Start()
    {
        managerParent.InputManager.InputActions.Main.Quit.performed += _ => HandleGameExit();
        managerParent.ScreenOverlayManager.SwitchOverlay(managerParent.ScreenOverlayManager.ScreenOffOverlay);
    }

    public void StartGame()
    {
        if (_insertedCoinsAmount < 1)
        {
        }
        else
        {
            _insertedCoinsAmount--;
            managerParent.BoardManager.ResetBoard();
        }
    }

    public void HandleGameOver(Board.Result result)
    {
        Destroy(managerParent.BoardManager.BoardController.gameObject);
        managerParent.ScreenOverlayManager.SwitchOverlay(managerParent.ScreenOverlayManager.ScreenOffOverlay);
    }
    
    public void InsertCoin() => _insertedCoinsAmount++;

    public static void HandleGameExit()
    {
        Application.Quit();
    }
}