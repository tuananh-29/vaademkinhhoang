using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("Settings Panel")]
    public GameObject panelSettings;

    [Header("Ten Scene Choi Game")]
    public string tenSceneChoiGame = "TenSceneChoiGame"; // doi thanh ten scene that cua ban

    // Nut BAT DAU / PLAY - luon bat dau choi tu dau
    public void PlayGame()
    {
        SceneManager.LoadScene(tenSceneChoiGame);
    }

    // Nut CONTINUE - choi tiep tu scene da luu gan nhat
    public void ContinueGame()
    {
        string lastScene = PlayerPrefs.GetString("LastScene", tenSceneChoiGame);
        SceneManager.LoadScene(lastScene);
    }

    // Nut SETTINGS - mo bang cai dat
    public void OpenSettings()
    {
        panelSettings.SetActive(true);
    }

    // Nut Quay Lai trong bang Settings - dong bang cai dat
    public void CloseSettings()
    {
        panelSettings.SetActive(false);
    }

    // Nut THOAT - thoat game
    public void QuitGame()
    {
        Debug.Log("Da thoat game!");
        Application.Quit();
    }

    // Ham nay goi tu Scene choi game de quay ve Main Menu
    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu"); // phai trung dung ten Scene Main Menu
    }
}