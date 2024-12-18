using UnityEngine;

namespace Input
{
    public class Manager : MonoBehaviour
    {
        public InputActions InputActions { get; private set; }

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