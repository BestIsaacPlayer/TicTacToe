using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScreenOverlayManager : MonoBehaviour
{
    [Header("Tic Tac Toe Machine Setup")]
    [field: SerializeField] public GameObject ScreenOffOverlay { get; private set; }
    [field: SerializeField] public GameObject TurkThinkingOverlay { get; private set; }
    [field: SerializeField] public GameObject PlayerMoveOverlay { get; private set; }
    [field: SerializeField] public TextMeshProUGUI OffScreenText { get; set; }
    [field: SerializeField] public TextMeshProUGUI MainScreenText { get; set; }
    
    [Header("Tic Tac Toe Machine Setup")]
    [field: SerializeField] public GameObject RPCScreenOffOverlay { get; private set; }
    [field: SerializeField] public GameObject RPCTurkThinkingOverlay { get; private set; }
    [field: SerializeField] public GameObject RPCPlayerMoveOverlay { get; private set; }
    [field: SerializeField] public TextMeshProUGUI RPCOffScreenText { get; set; }
    [field: SerializeField] public TextMeshProUGUI RPCMainScreenText { get; set; }

    private GameObject _ticTacToeOverlay;
    private GameObject _rpcOverlay;

    private void Awake()
    {
        if (!ScreenOffOverlay) Debug.LogError($"The {Utility.Parser.FieldToName(nameof(ScreenOffOverlay))} field in the {gameObject.name} object is unset!");
        if (!TurkThinkingOverlay) Debug.LogError($"The {Utility.Parser.FieldToName(nameof(TurkThinkingOverlay))} field in the {gameObject.name} object is unset!");
        if (!PlayerMoveOverlay) Debug.LogError($"The {Utility.Parser.FieldToName(nameof(PlayerMoveOverlay))} field in the {gameObject.name} object is unset!");
        if (!OffScreenText) Debug.LogError($"The {Utility.Parser.FieldToName(nameof(OffScreenText))} field in the {gameObject.name} object is unset!");
    }

    public void SwitchOverlay(GameObject overlay, Type machine)
    {
        if (machine == typeof(Board.Controller))
        {
            if (_ticTacToeOverlay) _ticTacToeOverlay.SetActive(false);
            _ticTacToeOverlay = overlay;
            _ticTacToeOverlay.SetActive(true);
        }

        if (machine == typeof(RPC.Controller))
        {
            if (_rpcOverlay) _rpcOverlay.SetActive(false);
            _rpcOverlay = overlay;
            _rpcOverlay.SetActive(true);
        }
    }
}