using UnityEngine;

namespace Player
{
    public class Controller : MonoBehaviour
    {
        [SerializeField] private ManagerParent managerParent;

        private void OnValidate()
        {
            if (!managerParent) Debug.LogError($"The {Utility.Parser.FieldToName(nameof(managerParent))} field in the {gameObject.name} object is unset!");
        }
    }
}