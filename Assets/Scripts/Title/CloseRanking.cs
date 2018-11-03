using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseRanking : MonoBehaviour {
    [SerializeField] GameObject title;
	void Update () {
        if (Input.GetButtonDown("Cancel"))
        {
            gameObject.SetActive(false);
            title.SetActive(true);
        }
	}
}
