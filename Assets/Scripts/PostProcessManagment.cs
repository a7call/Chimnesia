using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PostProcessManagment : MonoBehaviour
{
    [SerializeField] Volume volume;
    ChromaticAberration chromaticAberation;
    float timeElapsed;
    public float lerpDuration = 2f;
    float endValue = 1;
    float startValue = 0;
    private void Awake()
    {
        volume.profile.TryGet<ChromaticAberration>(out chromaticAberation);
    }

    private void FixedUpdate()
    {
        if (GameManagement.GetInstance().isInPresent)
        {
            chromaticAberation.intensity.value = 0;
            return;
        }

        if(timeElapsed < lerpDuration)
        {
            chromaticAberation.intensity.value = Mathf.Lerp(startValue, endValue, timeElapsed / lerpDuration);
            timeElapsed += Time.deltaTime;
        }
        else
        {
            timeElapsed = 0;

            if (endValue == 1)
            {
                startValue = 1;
                endValue = 0;
            }
            else
            {
                startValue = 0;
                endValue = 1;
            }
                         
        }
       
    }
}
