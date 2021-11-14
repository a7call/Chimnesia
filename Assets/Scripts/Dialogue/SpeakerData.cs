using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Dialog/Speaker")]
public class SpeakerData : ScriptableObject
{
    public const string EMOTION_NEUTRAL = "neutral";
    public const string EMOTION_FOCUSED = "focused";
    public const string EMOTION_SHOKED = "shocked";

    public string speakerName;

    public Sprite portraitNeutral, portraitFocused, portraitShocked;

    public Sprite GetEmotionPortrait(string emotion)
    {
        switch (emotion)
        {
            default:
            case EMOTION_NEUTRAL:
                return portraitNeutral;

            case EMOTION_SHOKED:
                return portraitShocked;

            case EMOTION_FOCUSED:
                return portraitFocused;
        }
    }
}
