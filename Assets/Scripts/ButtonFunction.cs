using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class ButtonFunction : MonoBehaviour
{

    public void LoadChooseScene()
    {
        SceneManager.LoadScene("Choose");
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
}
