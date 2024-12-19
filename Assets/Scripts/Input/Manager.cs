using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Input
{
    public class Manager : MonoBehaviour
    {
        public InputActions InputActions { get; private set; }
        [SerializeField] private PlayerInput playerInput;

        private void Awake()
        {
            InputActions = new InputActions();
        }

        private void Update()
        {
            Debug.Log(playerInput.currentControlScheme);
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