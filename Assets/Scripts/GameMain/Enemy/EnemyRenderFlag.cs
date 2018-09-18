using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRenderFlag : MonoBehaviour
{
    private LockOnable parent;
    private void Awake()
    {
        parent = GetComponentInParent<LockOnable>();
    }
    private void OnWillRenderObject()
    {
        switch (Camera.current.tag)
        {
            case "1P":
                parent.cameraFlag |= 1;
                break;
            case "2P":
                parent.cameraFlag |= 2;
                break;
            case "3P":
                parent.cameraFlag |= 4;
                break;
            case "4P":
                parent.cameraFlag |= 8;
                break;
            default:
                //Debug.LogError(Camera.current.tag + "がレンダリングされている。");
                break;
        }
    }
}
