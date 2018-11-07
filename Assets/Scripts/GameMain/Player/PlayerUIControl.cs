using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class PlayerUIControl : MonoBehaviour
{
    int id;
    bool SPComp = false, STComp = false;
    [SerializeField] Sprite[] PlayerUI;
    [SerializeField] Image p_ui;
    [SerializeField] Image hpbar, specialGage, strongGage, time;
    [SerializeField] Color inCompleteColor, CompleteColor;

    public void SetUI(int id, Transform player)
    {
        this.id = id;
        p_ui.sprite = PlayerUI[id];
        var _event = player.GetComponent<IPlayerEvent>();
        _event.RemainHP.Subscribe(hp => hpbar.fillAmount = hp / 100.0f).AddTo(hpbar);
        _event.SpecialGage.Subscribe(gage => specialGage.fillAmount = gage).AddTo(specialGage);
        _event.StrongGage.Subscribe(gage => strongGage.fillAmount = gage).AddTo(strongGage);
        GameControl.RemainTime.Subscribe(remain => time.fillAmount = remain).AddTo(time);
        specialGage.color = inCompleteColor;
        strongGage.color = inCompleteColor;
    }

    private void Update()
    {
        if (SPComp && specialGage.fillAmount < 0.5f)
        {
            SPComp = false;
            specialGage.color = inCompleteColor;
        }
        else if (!SPComp && specialGage.fillAmount >= 1.0f)
        {
            SPComp = true;
            specialGage.color = CompleteColor;
        }
        if (STComp && strongGage.fillAmount < 0.5f)
        {
            STComp = false;
            strongGage.color = inCompleteColor;
        }
        else if (!STComp && strongGage.fillAmount >= 1.0f)
        {
            STComp = true;
            strongGage.color = CompleteColor;
        }
    }
}
