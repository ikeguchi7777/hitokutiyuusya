using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SEType
{
    Select,
    Submit,
    Cancel,
    Slash,
    BlowTone,
    Heal
}

public class SEController : SingletonObject<SEController> {
    [SerializeField] List<SE> SEList;
    private AudioSource source;

    protected override void Awake()
    {
        base.Awake();
        source = GetComponent<AudioSource>();
    }

    public void PlaySE(SEType type)
    {
        var t = SEList.Find(se => se.type == type);
#if UNITY_EDITOR
        if (t == null)
            Debug.Log(type + "に割り振られた音が存在しません。");
#endif
        source.PlayOneShot(t.audio);
    }
    [System.Serializable]
    class SE
    {
        public AudioClip audio;
        public SEType type;
    }
}
