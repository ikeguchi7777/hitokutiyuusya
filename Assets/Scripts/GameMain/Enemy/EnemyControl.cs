using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyControl<T, TEnum> : StatefulObjectBase<T, TEnum> , LockOnable
where T : EnemyControl<T, TEnum> where TEnum : System.IConvertible
{
    private static int enemyNum = 0;
    public int ID { get; set; }

    public int cameraFlag { get; set; }

    public Transform TargetTransform
    {
        get
        {
            return transform;
        }
    }

    protected override void Awake()
    {
        base.Awake();
        ID = enemyNum;
        enemyNum++;
        InstantiateObjectManager.Instance.EnemyList.Add(this);
    }

    private void LateUpdate()
    {
        cameraFlag = 0;
    }
    private void OnWillRenderObject()
    {
        switch (Camera.current.tag)
        {
            case "1P":
                cameraFlag |= 1;
                break;
            case "2P":
                cameraFlag |= 2;
                break;
            case "3P":
                cameraFlag |= 4;
                break;
            case "4P":
                cameraFlag |= 8;
                break;
            default:
                Debug.LogError(Camera.current.tag + "がレンダリングされている。");
                break;
        }
    }
    private void OnDestroy()
    {
        InstantiateObjectManager.Instance.RemoveEnemy(this);
    }
}

public interface LockOnable
{
    int ID { get; set; }
    int cameraFlag { get; set; }
    Transform TargetTransform { get; }
}