using UnityEngine;

public class ManagerParent : MonoBehaviour
{
    [field: SerializeField] public Input.Manager InputManager { get; private set; }

    private void OnValidate()
    {
        if (!InputManager) Debug.LogError($"The {Utility.Parser.FieldToName(nameof(InputManager))} field in the {gameObject.name} object is unset!");
    }
}