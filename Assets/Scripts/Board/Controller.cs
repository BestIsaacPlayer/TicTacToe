using UnityEngine;

namespace Board
{
    public class Controller : MonoBehaviour
    {
        [field: SerializeField] public Cell.Controller[] Cells { get; private set; } = new Cell.Controller[9];

        private void OnValidate()
        {
            for (var i = 0; i < 1; ++i)
            {
                if (!Cells[i]) Debug.LogError($"The Element {i} in {Utility.Parser.FieldToName(nameof(Cells))} field in the {gameObject.name} object is unset!");
            }
        }
    }
}