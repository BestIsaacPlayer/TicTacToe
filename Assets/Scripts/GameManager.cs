using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Input.Manager inputManager;

    private void Awake()
    {
        if (!inputManager) Debug.LogError($"The {Utility.Parser.FieldToName(nameof(inputManager))} field in the {gameObject.name} object is unset!");
    }

    private void Start()
    {
        inputManager.InputActions.Main.Quit.performed += _ => HandleGameExit();
    }

    public void HandleGameExit()
    {
        Application.Quit();
    }
}