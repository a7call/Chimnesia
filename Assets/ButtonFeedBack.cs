using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ButtonFeedBack : MonoBehaviour
{
    public void OnHooverEnter()
    {
        transform.DOScale(1.25f, 0.4f);
    }

    public void OnHooverExit()
    {
        transform.DOScale(1, 0.4f);
    }
    public void OnDisable()
    {
        transform.localScale = new Vector3(1, 1, 1);
    }
}
