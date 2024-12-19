using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class ButtonController : MonoBehaviour
{
    private ManagerParent _managerParent;

    [SerializeField] private TextMeshProUGUI exitText;
    [SerializeField] private TextMeshProUGUI noteText;
    [SerializeField] private TextMeshProUGUI markText;

    private void Awake()
    {
        _managerParent = FindFirstObjectByType<ManagerParent>();
        if (!_managerParent) Debug.LogError($"The {Utility.Parser.FieldToName(nameof(_managerParent))} field in the {gameObject.name} object is unset!");
    }

    private void Update()
    {
        var device = _managerParent.InputManager.PlayerInput.currentControlScheme;
        
        switch (device)
        {
            case "Keyboard":
                exitText.text = "[ESCAPE]";
                noteText.text = "[ENTER] coins to play!";
                markText.text = "Mark selected [SPACE]";
                break;
            case "Gamepad":
                exitText.text = "E[X]it";
                noteText.text = "Enter coins to pl[A]y";
                markText.text = "Mark selected [B]ox";
                break;
        }
    }

    public void HandleMovementButtonPress(int direction)
    {
        if (_managerParent.BoardManager.BoardController == null) return;
        _managerParent.BoardManager.BoardController.HandleMovementInput(direction);
    }

    public void HandleMarkButtonPress()
    {
        if (_managerParent.BoardManager.BoardController == null)
        {
            _managerParent.GameManager.StartGame();
        }
        else
        {
            _managerParent.BoardManager.BoardController.HandleMarkInput();
        }
    }

    public void HandleCoinInserted() => _managerParent.GameManager.InsertCoin();

    public void HandleExitButtonPress()
    {
        GameManager.HandleGameExit();
    }
}