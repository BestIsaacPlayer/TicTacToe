using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Board
{
    public class Controller : MonoBehaviour
    {
        [field: SerializeField] public Cell.Controller[] Cells { get; private set; } = new Cell.Controller[9];
        private ManagerParent _managerParent;
        private int _currentCellIndex = 4;
        private void OnValidate()
        {
            for (var i = 0; i < 1; ++i)
            {
                if (!Cells[i]) Debug.LogError($"The Element {i} in {Utility.Parser.FieldToName(nameof(Cells))} field in the {gameObject.name} object is unset!");
            }
        }

        private void Start()
        {
            _managerParent = FindFirstObjectByType<ManagerParent>();
            if (!_managerParent) Debug.LogError($"The {Utility.Parser.FieldToName(nameof(_managerParent))} field in the {gameObject.name} object is unset!");

            _managerParent.InputManager.InputActions.Main.Movement.performed += MoveCurrentCell;
        }

        private void MoveCurrentCell(InputAction.CallbackContext context)
        {
            var direction = Utility.Parser.Vector2ToDirection(context.ReadValue<Vector2>());
            
            var x = Math.DivRem(_currentCellIndex, 3, out var y);

            switch (direction)
            {
                case Direction.Right:
                    y += 1;
                    if (y > 2) y = 0;
                    break;
                case Direction.Left:
                    y -= 1;
                    if (y < 0) y = 2;
                    break;
                case Direction.Up:
                    x -= 1;
                    if (x < 0) x = 2;
                    break;
                case Direction.Down:
                    x += 1;
                    if (x > 2) x = 0;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            _currentCellIndex = 3 * x + y;
            Debug.Log(_currentCellIndex);
        }
    }
}