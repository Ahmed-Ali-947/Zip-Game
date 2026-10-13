using UnityEngine;
using TMPro;
using System;
using System.IO;
using UnityEngine.Rendering;
public class UIManager : MonoBehaviour
{
    [SerializeField] private GameLevelManager gameLevelManager;
    [SerializeField] private WinChecker winChecker;
    [SerializeField] private PathManager pathManager;

    [SerializeField] private TextMeshProUGUI gameStateText;
    [SerializeField] private GameObject winPanel;
    [SerializeField] private TextMeshProUGUI currLevelText;

    void Start()
    {
        UpdateCurrentLevel();
    }
    void OnEnable()
    {
        winChecker.OnGameWon += OnGameWon_WinChecker;
    }
    void OnDisable()
    {
        winChecker.OnGameWon -= OnGameWon_WinChecker;
    }

    private void OnGameWon_WinChecker(object s, EventArgs e)
    {
        winPanel.SetActive(true);
    }

    private void UpdateCurrentLevel()
    {
        currLevelText.text = "Level " + gameLevelManager.GetCurrentLevel().ToString();
    }
    public void UpdateGameStateText(string text)
    {
        gameStateText.text = text;
    }

    public void LoadNextLevel()
    {
        gameLevelManager.LoadNextLevel();
    }

    public void UndoLastMoveBtn()
    {
        pathManager.UndoLastMoveBtn();
    }

    public void GiveHintBtn()
    {
        pathManager.GiveHint();
    }

    public void ResetBtn()
    {
        pathManager.ResetPath();
    }
}
