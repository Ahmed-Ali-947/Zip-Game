using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLevelManager : MonoBehaviour
{
    [SerializeField] private GridManager gridManager;
    [SerializeField] private LevelSO[] levels;

    private int currLevel = 1;
    private int currentLevelIndex = 0;
    private LevelSO currLevelSO;
    void Awake()
    {
        currLevel = PlayerPrefs.GetInt(GameConstants.CurrentPlayerLevel_PlayerPrefsString, 1);
        currentLevelIndex = currLevel - 1;
        currLevelSO = levels[currentLevelIndex];
    }
    private void Start()
    {
        LoadCurrentLevel();
    }

    public void LoadCurrentLevel()
    {
        gridManager.GenerateGrid(currLevelSO);
    }

    public void LoadNextLevel()
    {
        currentLevelIndex++;
        currLevel++;

        if (currentLevelIndex >= levels.Length)
        {
            Debug.Log("All levels complete!");
            currentLevelIndex = levels.Length - 1;
            currLevel = currentLevelIndex + 1;
            return;
        }

        PlayerPrefs.SetInt(GameConstants.CurrentPlayerLevel_PlayerPrefsString, currLevel);
        PlayerPrefs.Save();

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public int GetCurrentLevel()
    {
        return currLevel;
    }

    public LevelSO GetCurrentLevelSO()
    {
        return currLevelSO;
    }
}
