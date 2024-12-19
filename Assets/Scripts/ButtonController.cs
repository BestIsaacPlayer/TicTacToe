using UnityEngine;

public class ButtonController : MonoBehaviour
{
    private ManagerParent _managerParent;

    private void Awake()
    {
        _managerParent = FindFirstObjectByType<ManagerParent>();
        if (!_managerParent) Debug.LogError($"The {Utility.Parser.FieldToName(nameof(_managerParent))} field in the {gameObject.name} object is unset!");
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