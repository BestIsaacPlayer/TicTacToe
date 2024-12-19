using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScreenOverlayManager : MonoBehaviour
{
    [field: SerializeField] public GameObject ScreenOffOverlay { get; private set; }
    [field: SerializeField] public GameObject TurkThinkingOverlay { get; private set; }
    [field: SerializeField] public GameObject PlayerMoveOverlay { get; private set; }
    [field: SerializeField] public Image TurkThinkingImage { get; private set; }
    [field: SerializeField] public TextMeshProUGUI OffScreenText { get; set; }
    [field: SerializeField] public TextMeshProUGUI MainScreenText { get; set; }

    private GameObject _currentOverlay;

    private void Awake()
    {
        if (!ScreenOffOverlay) Debug.LogError($"The {Utility.Parser.FieldToName(nameof(ScreenOffOverlay))} field in the {gameObject.name} object is unset!");
        if (!TurkThinkingOverlay) Debug.LogError($"The {Utility.Parser.FieldToName(nameof(TurkThinkingOverlay))} field in the {gameObject.name} object is unset!");
        if (!PlayerMoveOverlay) Debug.LogError($"The {Utility.Parser.FieldToName(nameof(PlayerMoveOverlay))} field in the {gameObject.name} object is unset!");
        if (!TurkThinkingImage) Debug.LogError($"The {Utility.Parser.FieldToName(nameof(TurkThinkingImage))} field in the {gameObject.name} object is unset!");
        if (!OffScreenText) Debug.LogError($"The {Utility.Parser.FieldToName(nameof(OffScreenText))} field in the {gameObject.name} object is unset!");
    }

    public void SwitchOverlay(GameObject overlay)
    {
        if (_currentOverlay) _currentOverlay.SetActive(false);
        _currentOverlay = overlay;
        _currentOverlay.SetActive(true);
    }

    public void ToggleTurkThinking(bool isOn)
    {
        TurkThinkingImage.color = isOn ? Color.red : Color.white;
    }
}