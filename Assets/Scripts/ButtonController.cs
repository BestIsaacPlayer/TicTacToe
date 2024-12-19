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
    
    [Header("Quit Note Setup")]
    [SerializeField] private Image quitNoteImage;
    [SerializeField] private Sprite pcQuitNote;
    [SerializeField] private Sprite gamepadQuitNote;
    
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
                if (exitButtonImage) exitButtonImage.sprite = keyboardExitImage;
                if (noteTextImage) noteTextImage.sprite = noteTextKeyboardSprite;
                if (markButtonImage) markButtonImage.sprite = keyboardMarkButtonImage;
                break;
            case "Gamepad":
                if (exitButtonImage) exitButtonImage.sprite = gamepadExitImage;
                if (noteTextImage) noteTextImage.sprite = noteTextGamepadSprite;
                if (markButtonImage) markButtonImage.sprite = gamepadMarkButtonImage;
                break;
        }
    }

    public void HandleMovementButtonPress(int direction)
    {
        if (_managerParent.TicTacToeBoardManager.TicTacToeBoardController == null) return;
        _managerParent.TicTacToeBoardManager.TicTacToeBoardController.HandleMovementInput(direction);
    }

    public void HandleMarkButtonPress()
    {
        if (_managerParent.TicTacToeBoardManager.TicTacToeBoardController == null)
        {
            _managerParent.GameManager.StartTicTacToe();
        }
        else
        {
            _managerParent.TicTacToeBoardManager.TicTacToeBoardController.HandleMarkInput();
        }
    }

    public void HandleCoinInserted() => _managerParent.GameManager.InsertCoin();

    public void HandleExitButtonPress()
    {
        _managerParent.GameManager.HandleGameExit();
    }

    public void HandleRockButton()
    {
        if (_managerParent.RPCBoardManager.RPCBoardController == null)
        {
            _managerParent.GameManager.StartRPC();
        }
        else
        {
            _managerParent.RPCBoardManager.RPCBoardController.HandleButtonInput(RPC.Content.Rock);
        }
    }
    
    public void HandleScissorsButton() => _managerParent.RPCBoardManager.RPCBoardController.HandleButtonInput(RPC.Content.Scissors);
    public void HandlePaperButton() => _managerParent.RPCBoardManager.RPCBoardController.HandleButtonInput(RPC.Content.Paper);
}