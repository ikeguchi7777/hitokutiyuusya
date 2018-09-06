using UnityEngine;
using System.Collections;

public class IKLookAt : MonoBehaviour
{
    private Animator avator;
    public Transform lookAtObj { set; private get; }

    [SerializeField, Range(0, 1)]
    private float lookAtWeight = 1.0f;
    [SerializeField, Range(0, 1)]
    private float bodyWeight = 0.4f;
    [SerializeField, Range(0, 1)]
    private float headWeight = 0.7f;
    [SerializeField, Range(0, 1)]
    private float eyesWeight = 0.5f;
    [SerializeField, Range(0, 1)]
    private float clampWeight = 0.5f;

    // Use this for initialization
    void Awake()
    {
        avator = GetComponent<Animator>();
    }

    void OnAnimatorIK(int layorIndex)
    {
        avator.SetLookAtWeight(lookAtWeight, bodyWeight, headWeight, eyesWeight, clampWeight);
        avator.SetLookAtPosition(lookAtObj.position);
    }
}