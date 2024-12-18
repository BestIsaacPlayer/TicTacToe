using UnityEngine;

namespace Board.Cell
{
    public class Controller : MonoBehaviour
    {
        public Content Content { get; private set; } = Content.Empty;
    }
}