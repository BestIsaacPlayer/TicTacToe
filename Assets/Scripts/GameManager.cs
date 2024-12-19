﻿using System;
using System.Collections;
using Board;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Sound Setup")]
    [SerializeField] private AudioClip backgroundMusic;
    [SerializeField] private AudioClip coinInsertSound;
    
    [Space]
    [SerializeField] private ManagerParent managerParent;
    
    private int _insertedCoinsAmount;
    private AudioSource _audioSource;

    private void Awake()
    {
        if (!managerParent) Debug.LogError($"The {Utility.Parser.FieldToName(nameof(managerParent))} field in the {gameObject.name} object is unset!");
        
        managerParent.ScreenOverlayManager.SwitchOverlay(managerParent.ScreenOverlayManager.ScreenOffOverlay);
        managerParent.ScreenOverlayManager.OffScreenText.text = "You're out of credits!";
        _audioSource = managerParent.AudioManager.PlayClip(backgroundMusic, true);
    }

    private void Start()
    {
        managerParent.InputManager.InputActions.Main.Quit.performed += _ => HandleGameExit();
        managerParent.InputManager.InputActions.Main.Enter.performed += _ => InsertCoin();
        managerParent.InputManager.InputActions.Main.Mark.performed += _ => StartGameOnOffScreen();
    }

    public void StartGame()
    {
        if (_insertedCoinsAmount < 1)
        {
        }
        else
        {
            _insertedCoinsAmount--;
            managerParent.BoardManager.ResetBoard();
        }
    }

    private void StartGameOnOffScreen()
    {
        if (managerParent.BoardManager.BoardController == null)
        {
            managerParent.GameManager.StartGame();
        }
    }

    public void HandleGameOver(Result result)
    {
        Destroy(managerParent.BoardManager.BoardController.gameObject);
        StartCoroutine(HandleGameEndDisplay(result));
    }

    private IEnumerator HandleGameEndDisplay(Result result)
    {
        managerParent.ScreenOverlayManager.SwitchOverlay(managerParent.ScreenOverlayManager.PlayerMoveOverlay);
        managerParent.ScreenOverlayManager.MainScreenText.text = result switch {
            Result.XWon => "X won!",
            Result.OWon => "O won!",
            Result.Draw => "It's a draw!",
            Result.MatchNotOver => "Something went terribly wrong!",
            _ => throw new ArgumentOutOfRangeException(nameof(result), result, null)
        };
        yield return new WaitForSeconds(2f);
        managerParent.ScreenOverlayManager.OffScreenText.text = _insertedCoinsAmount < 1 ? "You're out of credits!" : "Press the MARK button to start the game!";
        managerParent.ScreenOverlayManager.MainScreenText.text = string.Empty;
        managerParent.ScreenOverlayManager.SwitchOverlay(managerParent.ScreenOverlayManager.ScreenOffOverlay);
    }

    public void InsertCoin()
    {
        managerParent.AudioManager.PlayClip(coinInsertSound);
        _insertedCoinsAmount++;
        managerParent.ScreenOverlayManager.OffScreenText.text = "Press the MARK button to start the game!";
    }

    public void HandleGameExit()
    {
        Destroy(_audioSource.gameObject);
        Application.Quit();
    }
}