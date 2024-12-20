﻿using System;
using UnityEngine;

namespace Board
{
    public class Manager : MonoBehaviour
    {
        [Header("Board Setup")]
        [SerializeField] private Transform boardTransform;
        [SerializeField] private Controller boardPrefab;
        public Controller TicTacToeBoardController { get; private set; }
        public Action ResetTicTacToe;

        private void OnValidate()
        {
            if (!boardTransform) Debug.LogError($"The {Utility.Parser.FieldToName(nameof(boardTransform))} field in the {gameObject.name} object is unset!");
            if (!boardPrefab) Debug.LogError($"The {Utility.Parser.FieldToName(nameof(boardPrefab))} field in the {gameObject.name} object is unset!");
        }

        private void Awake()
        {
            ResetTicTacToe += SpawnBoard;
        }

        private void SpawnBoard()
        {
            if (TicTacToeBoardController) Destroy(TicTacToeBoardController.gameObject);
            TicTacToeBoardController = Instantiate(boardPrefab, boardTransform.position, Quaternion.identity);
        }
    }
}