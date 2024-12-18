using System;
using UnityEngine;

namespace Board
{
    public class Controller : MonoBehaviour
    {
        [field: SerializeField] public Cell.Controller[] Cells { get; private set; } = new Cell.Controller[9];
        public int CurrentCellIndex { get; private set; } = 5;
        public void MoveCurrentCell(Direction direction)
        {
            var x = CurrentCellIndex % 3;
            Math.DivRem(CurrentCellIndex, 3, out var y);
            switch (direction)
            {
                case Direction.Up:
                    x = (x + 1) % 3;
                    break;
                case Direction.Down:
                    x = (x - 1) % 3;
                    break;
                case Direction.Left:
                    y = (y - 1) % 3;
                    break;
                case Direction.Right:
                    y = (y + 1) % 3;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
            }

            CurrentCellIndex = x + y;
        }

        private void OnValidate()
        {
            for (var i = 0; i < 1; ++i)
            {
                if (!Cells[i]) Debug.LogError($"The Element {i} in {Utility.Parser.FieldToName(nameof(Cells))} field in the {gameObject.name} object is unset!");
            }
        }
    }
}