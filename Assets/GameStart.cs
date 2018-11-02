using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameStart : MonoBehaviour {

    public GameObject ReadyText;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Select1PAnime.ready == true)
        {
            ReadyText.SetActive(true);
        }
        else
        {
            ReadyText.SetActive(false);
        }

        ReadyText.transform.localScale = new Vector2(1 + Mathf.Pow(Mathf.Cos(2 * Mathf.PI * Time.time), 2) * 0.3f, 1 + Mathf.Pow(Mathf.Cos(2 * Mathf.PI * Time.time), 2) * 0.3f);
	}
}
