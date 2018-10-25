using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PauseControl : MonoBehaviour {
    [SerializeField] GameObject pausePanel;
    [SerializeField] GameObject firstSelected;
    EventSystem eventSystem;
    StandaloneInputModule inputModule;

    private void Awake()
    {
        eventSystem = GetComponent<EventSystem>();
        inputModule = GetComponent<StandaloneInputModule>();
    }

    public void Pause(int i)
    {
        Time.timeScale = 0.0f;
        InstantiateObjectManager.Instance.Pause();
        SetButton(i);
        pausePanel.SetActive(true);
        eventSystem.SetSelectedGameObject(firstSelected);
    }

    public void Resume()
    {
        pausePanel.SetActive(false);
        InstantiateObjectManager.Instance.Resume();
        Time.timeScale = 1.0f;
        gameObject.SendMessage("ChangeState", GameState.Playing);
    }

    void SetButton(int i)
    {
        var input = PlayerInput.PlayerInputs[i];
        inputModule.horizontalAxis = input.GetAxisName(EAxis.X);
        inputModule.verticalAxis = input.GetAxisName(EAxis.Y);
        inputModule.submitButton = input.GetButtonName(EButton.WeakAttackAndSubmit);
        inputModule.cancelButton = input.GetButtonName(EButton.Evade);
    }
}
