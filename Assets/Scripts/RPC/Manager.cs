using System;
using UnityEngine;

namespace RPC
{
    public class Manager : MonoBehaviour
    {
        [Header("Board Setup")]
        [SerializeField] private Transform boardTransform;
        [SerializeField] private Controller boardPrefab;
        public Controller RPCBoardController { get; private set; }
        public Action ResetRPC;

        private void OnValidate()
        {
            if (!boardTransform) Debug.LogError($"The {Utility.Parser.FieldToName(nameof(boardTransform))} field in the {gameObject.name} object is unset!");
            if (!boardPrefab) Debug.LogError($"The {Utility.Parser.FieldToName(nameof(boardPrefab))} field in the {gameObject.name} object is unset!");
        }

        private void Awake()
        {
            ResetRPC += SpawnBoard;
        }

        private void SpawnBoard()
        {
            if (RPCBoardController) Destroy(RPCBoardController.gameObject);
            RPCBoardController = Instantiate(boardPrefab, boardTransform.position, Quaternion.identity);
        }
    }
}