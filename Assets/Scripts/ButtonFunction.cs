using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class ButtonFunction : MonoBehaviour
{
    [SerializeField] GameObject titlePanel;
    [SerializeField] GameObject rankingPanel;


     void Start()
    {
        Cursor.visible = false;
    }

    public void LoadChooseScene()
    {
        SceneManager.LoadScene("Choose");
    }

    public void LoadDescriptionScene()
    {
        SceneManager.LoadScene("Description");
    }

    public void GameEnd()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
		Application.Quit();
#endif
    }

    public void LoadGameMainScene()
    {
        SceneManager.LoadScene("GameMain");
    }

    public void OpenRanking()
    {
       
        titlePanel.SetActive(false);
        rankingPanel.SetActive(true);
    }

    public void CloseRanking()
    {

        titlePanel.SetActive(true);
        rankingPanel.SetActive(false);
    }
}
