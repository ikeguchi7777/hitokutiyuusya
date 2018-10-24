using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkModel : MonoBehaviour
{
    private SkinnedMeshRenderer skinnedMesh;

    [SerializeField] float speed = 1.0f;
    [SerializeField] float interval = 3.0f;

    private void Awake()
    {
        skinnedMesh = GetComponentInChildren<SkinnedMeshRenderer>();
    }

    IEnumerator Blink()
    {
        while (true)
        {
            float time = 0;
            yield return null;
            while (time <= speed)
            {
                time += Time.deltaTime * 10;
                skinnedMesh.SetBlendShapeWeight(0, Mathf.Lerp(0, 100, time / speed));
                yield return null;
            }
            time = 0;
            yield return null;
            while (time <= speed)
            {
                time += Time.deltaTime * 10;
                skinnedMesh.SetBlendShapeWeight(0, Mathf.Lerp(100, 0, time / speed));
                yield return null;
            }
            yield return new WaitForSeconds(interval);
        }
    }

    private void Start()
    {
        StartCoroutine(Blink());
    }
}
