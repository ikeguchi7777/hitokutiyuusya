using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// 前奏とループが存在するBGMの実装
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class IntroBGM : MonoBehaviour
{
    [SerializeField]
    private List<SoundInfo> _infoList;
    private BgmDefine _currentBgmDefine;
    [SerializeField]
    private bool PlayAuto = true;

    private AudioSource _currentBgmSource;

    private void Awake()
    {
        _currentBgmSource = GetComponent<AudioSource>();
        if (PlayAuto && _infoList != null)
        {
            _currentBgmSource.clip = _infoList[0].clip;
            _currentBgmSource.Play();
            _currentBgmDefine = _infoList[0].define;
        }

    }

    /// <summary>
    /// BGMの再生
    /// </summary>
    /// <param name="key">Key.</param>
    public void PlayBgm(string key)
    {
        var info = _infoList.Find(x => x.clip.name == key);
        if (info == null)
        {
            return;
        }

        // 複数サウンド同時再生はこのサンプルでは考慮していません
        var source = GetComponent<AudioSource>();
        if (source == null)
        {
            source = gameObject.AddComponent<AudioSource>();
        }
        source.clip = info.clip;
        source.Play();

        // 再生するBGMのstartTimeのセット
        _currentBgmDefine = info.define;
        _currentBgmSource = source;
    }

    void Update()
    {
        // 再生中のBGMの再生時間を監視する
        if (_currentBgmSource.isPlaying)
        {
            if (_currentBgmSource.time >= _currentBgmDefine.endTime)
            {
                _currentBgmSource.time = _currentBgmDefine.loopTime;
            }
        }
    }

}

/// <summary>
/// BGM定義ファイルとAudioClipをセットにしたクラス
/// </summary>
[System.Serializable]
public class SoundInfo
{
    public BgmDefine define;
    public AudioClip clip;

    public SoundInfo(BgmDefine define, AudioClip clip)
    {
        this.define = define;
        this.clip = clip;
        // Clip名はdefine.keyに強制的に変更する。オペミスで異なった時のため
        this.clip.name = define.key;
    }
}

/// <summary>
/// BGM定義クラス
/// 最終的にはアセットバンドルと同梱するテキストファイルになる予定
/// </summary>
[System.Serializable]
public class BgmDefine
{
    /// <summary>
    /// ユニークキー
    /// </summary>
    public string key;
    /// <summary>
    /// ループポイント時間
    /// </summary>
    public float loopTime;
    /// <summary>
    /// BGMの終了時間
    /// </summary>
    public float endTime;
}