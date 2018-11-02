using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealEffectControl : MonoBehaviour {

	// Use this for initialization
	void Start () {
        StartCoroutine(DestroyEffect());
	}
	
    IEnumerator DestroyEffect()
    {
        yield return new WaitForSeconds(3.0f);
        Destroy(gameObject);
    }
}
