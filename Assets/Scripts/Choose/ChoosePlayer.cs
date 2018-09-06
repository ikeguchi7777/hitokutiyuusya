using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChoosePlayer : MonoBehaviour {
    [SerializeField] Dropdown[] dropdowns;
    [SerializeField] Button startButton;
	// Use this for initialization
	void Start () {
        PlayerID.Instance.Init();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Change()
    {
        startButton.gameObject.SetActive(false);
        for (int i = 0; i < dropdowns.Length; i++)
        {
            PlayerID.Instance.PlayerTypes[i] = (PlayerType)dropdowns[i].value;
            Debug.Log(PlayerID.Instance.PlayerTypes[i]);
            if (dropdowns[i].value != 0)
                startButton.gameObject.SetActive(true);
        }
    }
}
