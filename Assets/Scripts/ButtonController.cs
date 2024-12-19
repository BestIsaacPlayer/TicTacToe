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
        _managerParent.BoardManager.BoardController.HandleMovementInput(direction);
    }

    public void HandleMarkButtonPress()
    {
        _managerParent.BoardManager.BoardController.HandleMarkInput();
    }

    public void HandleExitButtonPress()
    {
        _managerParent.GameManager.HandleGameExit();
    }
}