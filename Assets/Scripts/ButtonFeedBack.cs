using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using System.Linq;

public class ButtonFeedBack : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] List<GameObject> toUnActive;
    public void OnHooverEnter()
    {
        transform.DOScale(1.25f, 0.4f);
    }

    public void OnHooverExit()
    {
        transform.DOScale(1, 0.4f);
    }
    public void OnEnable()
    {
        transform.localScale = new Vector3(1, 1, 1);
    }

    public void PlayInteractionAnim()
    {
        toUnActive.ForEach(g => g.SetActive(false));
        animator.SetTrigger("Interact");
    }

}
