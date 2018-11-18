using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class page : MonoBehaviour {



    [SerializeField] Material[] Lpages = new Material[4];
    [SerializeField] Material[] Rpages = new Material[4];

    
    // Use this for initialization
    void Start () {

        StartCoroutine("Wait");

    }
	
	// Update is called once per frame
	void Update () {
		
	}



    private IEnumerator Wait()
    {

        Debug.Log("1");

        yield return new WaitForSeconds(1.0f);
    
        Debug.Log("2");
    
        yield return new WaitForSeconds(2.0f);

        Debug.Log("3");

    }



}

