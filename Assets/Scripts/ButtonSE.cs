using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonSE : MonoBehaviour,ISubmitHandler, ISelectHandler
{
    public void OnSelect(BaseEventData eventData)
    {
        SEController.Instance.PlaySE(SEType.Select);
    }

    public void OnSubmit(BaseEventData eventData)
    {
        SEController.Instance.PlaySE(SEType.Submit);
    }
}
