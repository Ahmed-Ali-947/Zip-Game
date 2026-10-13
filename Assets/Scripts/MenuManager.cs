using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI currLevelText;

    void Start()
    {
        currLevelText.text = "Level " + PlayerPrefs.GetInt(GameConstants.CurrentPlayerLevel_PlayerPrefsString, 1);
    }

    public void StartGame()
    {
        SceneManager.LoadScene(GameConstants.GameScene_SceneName);
    }
}
