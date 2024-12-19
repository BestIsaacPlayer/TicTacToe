using UnityEngine;

public class ManagerParent : MonoBehaviour
{
    [Header("Child Managers Setup")]
    [field: SerializeField] public Input.Manager InputManager { get; private set; }
    [field: SerializeField] public Board.Manager TicTacToeBoardManager { get; private set; }
    [field: SerializeField] public RPC.Manager RPCBoardManager { get; private set; }
    [field: SerializeField] public GameManager GameManager { get; private set; }
    [field: SerializeField] public ScreenOverlayManager ScreenOverlayManager { get; private set; }
    [field: SerializeField] public AudioManager AudioManager { get; private set; }

    private void OnValidate()
    {
        if (!InputManager) Debug.LogError($"The {Utility.Parser.FieldToName(nameof(InputManager))} field in the {gameObject.name} object is unset!");
        if (!TicTacToeBoardManager) Debug.LogError($"The {Utility.Parser.FieldToName(nameof(TicTacToeBoardManager))} field in the {gameObject.name} object is unset!");
        if (!RPCBoardManager) Debug.LogError($"The {Utility.Parser.FieldToName(nameof(RPCBoardManager))} field in the {gameObject.name} object is unset!");
        if (!GameManager) Debug.LogError($"The {Utility.Parser.FieldToName(nameof(GameManager))} field in the {gameObject.name} object is unset!");
        if (!ScreenOverlayManager) Debug.LogError($"The {Utility.Parser.FieldToName(nameof(ScreenOverlayManager))} field in the {gameObject.name} object is unset!");
        if (!AudioManager) Debug.LogError($"The {Utility.Parser.FieldToName(nameof(AudioManager))} field in the {gameObject.name} object is unset!");
    }
}