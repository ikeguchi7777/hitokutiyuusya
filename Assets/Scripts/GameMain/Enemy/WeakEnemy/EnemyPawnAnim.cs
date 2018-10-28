using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPawnAnim : MonoBehaviour {

    [SerializeField]Transform obj;
    [SerializeField] ParticleSystem particle;
    Vector3 des;
	void Awake () {
        des = obj.position;
        obj.position += Vector3.down * 1.5f;
        StartCoroutine(Pawn());
	}

    IEnumerator Pawn()
    {
        var main = particle.main;
        float time = 0.0f;
        yield return null;
        while (time <= 0.5f)
        {
            main.startSize = Mathf.Lerp(0.0f, 3.0f, time * 2.0f);
            time += Time.deltaTime;
            yield return null;
        }
        time = 0.0f;
        var pos = obj.position;
        yield return null;
        while(time<=1.0f)
        {
            obj.position = Vector3.Lerp(pos, des, time);
            time += Time.deltaTime;
            yield return null;
        }
        time = 0.0f;
        obj.position = des;
        gameObject.SendMessage("Pawn");
        yield return null;
        while (time <= 0.5f)
        {
            main.startSize = Mathf.Lerp(3.0f, 0.0f, time * 2.0f);
            time += Time.deltaTime;
            yield return null;
        }
        Destroy(particle.gameObject);
        Destroy(this);
    }
}
