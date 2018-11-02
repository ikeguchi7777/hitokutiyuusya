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
    void Update() {

       ReadyText.transform.localScale = new Vector2(1 + Mathf.Pow(Mathf.Cos(Mathf.PI * Time.time), 2) * 0.3f, 1 + Mathf.Pow(Mathf.Cos(Mathf.PI * Time.time), 2) * 0.3f);
        
    }

    public void Change()
    {
        ReadyText.SetActive(false);

        for (int i = 0; i < 4; i++)
        {
            
            if (SelectAnime.ready[i] == true)
            {
                ReadyText.SetActive(true);
            }
        
        }
    }

}
