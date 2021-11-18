using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class ButtonFeedBack : MonoBehaviour
{
    [SerializeField] List<GameObject> toUnActive;

    public Sprite HoverSprite;
    public Sprite HoverExitSprite;
    public void OnHooverEnter()
    {
        transform.DOScale(1.1f, 0.4f);
        GetComponent<Image>().sprite = HoverSprite;
    }

    public void OnHooverExit()
    {
        transform.DOScale(1, 0.4f);
        GetComponent<Image>().sprite = HoverExitSprite;
    }
    public void OnEnable()
    {
        transform.localScale = new Vector3(1, 1, 1);
        GetComponent<Image>().sprite = HoverExitSprite;
    }
    public void OnDisable()
    {
        GetComponent<Image>().sprite = HoverExitSprite;
    }


}
