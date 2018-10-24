using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PauseControl : MonoBehaviour {
    EventSystem eventSystem;

    private void Awake()
    {
        eventSystem = GetComponent<EventSystem>();
    }

    public void Pause(int i)
    {
        Time.timeScale = 0.0f;
    }

    public void Resume()
    {
        StartCoroutine(Wait());
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(0.5f);
        Time.timeScale = 1.0f;
        gameObject.SendMessage("ChangeState", GameState.Playing);
    }

    void SetButton(int i)
    {

    }
}
