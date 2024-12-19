using UnityEngine;
using UnityEngine.InputSystem;

namespace Input
{
    public class Manager : MonoBehaviour
    {
        public InputActions InputActions { get; private set; }
        [field: SerializeField] public PlayerInput PlayerInput { get; private set; }

        private void Awake()
        {
            InputActions = new InputActions();
        }

        private void OnEnable()
        {
            InputActions.Enable();
        }

        private void OnDisable()
        {
            InputActions.Disable();
        }
    }
}