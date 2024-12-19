using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{
    [Header("Instruction Note Setup")]
    [SerializeField] private Sprite noteTextKeyboardSprite;
    [SerializeField] private Sprite noteTextGamepadSprite;
    [SerializeField] private Image noteTextImage;

    [Header("Exit Button Setup")]
    [SerializeField] private Image exitButtonImage;
    [SerializeField] private Sprite gamepadExitImage;
    [SerializeField] private Sprite keyboardExitImage;
    
    [Header("Mark Button Setup")]
    [SerializeField] private Image markButtonImage;
    [SerializeField] private Sprite gamepadMarkButtonImage;
    [SerializeField] private Sprite keyboardMarkButtonImage;
    
    private ManagerParent _managerParent;

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
                exitButtonImage.sprite = keyboardExitImage;
                noteTextImage.sprite = noteTextKeyboardSprite;
                markButtonImage.sprite = keyboardMarkButtonImage;
                break;
            case "Gamepad":
                exitButtonImage.sprite = gamepadExitImage;
                noteTextImage.sprite = noteTextGamepadSprite;
                markButtonImage.sprite = gamepadMarkButtonImage;
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
        _managerParent.GameManager.HandleGameExit();
    }
}