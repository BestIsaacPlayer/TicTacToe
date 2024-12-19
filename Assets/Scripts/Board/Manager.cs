using System;
using UnityEngine;

namespace Board
{
    public class Manager : MonoBehaviour
    {
        [Header("Board Setup")]
        [SerializeField] private Transform boardTransform;
        [SerializeField] private Controller boardPrefab;
        public Controller BoardController { get; private set; }
        public Action ResetBoard;

        private void OnValidate()
        {
            if (!boardTransform) Debug.LogError($"The {Utility.Parser.FieldToName(nameof(boardTransform))} field in the {gameObject.name} object is unset!");
            if (!boardPrefab) Debug.LogError($"The {Utility.Parser.FieldToName(nameof(boardPrefab))} field in the {gameObject.name} object is unset!");
        }

        private void Awake()
        {
            ResetBoard += SpawnBoard;
        }

        private void SpawnBoard()
        {
            if (BoardController) Destroy(BoardController.gameObject);
            BoardController = Instantiate(boardPrefab, boardTransform.position, Quaternion.identity);
        }
    }
}