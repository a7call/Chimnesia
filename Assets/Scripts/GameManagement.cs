using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Yarn.Unity;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;


public class GameManagement : Singleton<GameManagement>
{
    public static bool isInDialogue = false;

    [Header("Dialogue UI")]
    [SerializeField] GameObject dialogueUI;
    [SerializeField] Image speakerPortrait;
    //[SerializeField] TextMeshProUGUI txt_speakerName;

    [Header("NPC")]
    [SerializeField] List<GameObject> UnActiveDuringDialogue;

    [Header("Player")]
    [SerializeField] GameObject player;
    [SerializeField] List<Transform> playerPositions;
    private int position = 0;


    public DialogueRunner runner;

    Dictionary<string, SpeakerData> speakerDataBase = new Dictionary<string, SpeakerData>();

    public NPC currentNPC { get; set; }

    private float panelAlpha;
    private float buttonAlpha;
    private DialogueUI dialUi;

    protected override void Awake()
    {
        base.Awake();
        dialUi = runner.GetComponent<DialogueUI>();
        panelAlpha = dialUi.GetComponent<Image>().color.a;
        dialUi.optionButtons.ForEach(x =>
        {
            buttonAlpha = x.transform.GetComponent<Image>().color.a;
        });

    }
    private void Start()
    {
        dialUi.dialogueContainer = dialogueUI;
       // dialogueUI.SetActive(false);
        runner.AddCommandHandler("SetSpeaker", SetSpeakerInfo);
        runner.AddCommandHandler("PlaySound", PlaySoudFX);
    }

    public void PlaySoudFX(string[] info)
    {
        string soundName = info[0];
        EffectManager.GetInstance().Play(soundName, this.gameObject);
    }
    public void MoveForward()
    {
        if (playerPositions.Count <= position + 1)
            return;

        position++;
        player.transform.parent = playerPositions[position];
        player.transform.position = playerPositions[position].position;
    }
    public void MoveBackward()
    {
        if (position - 1 < 0)
            return;

        position--;
        player.transform.parent = playerPositions[position];
        player.transform.position = playerPositions[position].position;
    }

    public void AddSpeaker(SpeakerData data)
    {
        if (speakerDataBase.ContainsKey(data.speakerName))
        {
            Debug.LogWarningFormat("Attemptiong to add {0} int speaker data, but it already exist", data.speakerName);
            return;
        }

        speakerDataBase.Add(data.speakerName, data);
    }

    void SetSpeakerInfo(string[] info)
    {
        string speaker = info[0];
        string emotion = info.Length > 1 ? info[1].ToLower() : SpeakerData.EMOTION_NEUTRAL;
        print(emotion);

        if (speakerDataBase.TryGetValue(speaker, out SpeakerData data))
        {
            speakerPortrait.sprite = data.GetEmotionPortrait(emotion);
            //txt_speakerName.text = data.speakerName;
        }
        else
        {
            Debug.LogErrorFormat("Could not set speaker info for unknown speaker {0}", speaker);
        }


    }
    public void StartDialogue()
    {
        isInDialogue = true;
        currentNPC = EventSystem.current.currentSelectedGameObject.GetComponent<NPC>();
        dialogueUI.SetActive(true);
        //SetNPCState(false);
        runner.StartDialogue(currentNPC.YarnStartNode);
    }

    public void CompleteDialogue()
    {
        isInDialogue = false;
        runner.GetComponent<DialogueUI>().optionButtons.ForEach(o => o.gameObject.SetActive(false));
        //SetNPCState(true);
    }
    //public void SetNPCState(bool active)
    //{
    //    UnActiveDuringDialogue.ForEach(n => n.SetActive(active));
    //}
    public Vector3 punch;
    public float duration;
    public void EndOptions()
    {
        var selectedOption = EventSystem.current.currentSelectedGameObject;
        DisableOptions(selectedOption);
    }

    private void DisableOptions(GameObject selectedOption)
    {
        //runner.GetComponent<DialogueUI>().optionButtons.ForEach(x =>
        //{
        //    if (x.gameObject != selectedOption)
        //    {
        //        if (!x.gameObject.activeSelf)
        //            return;

        //        var image = x.transform.GetComponent<Image>();
        //        image.DOFade(0, 0.2f).OnComplete(() =>
        //        {
        //            image.gameObject.SetActive(false);
        //            image.DOFade(buttonAlpha, 0.01f);
        //        });

        //        var text = x.gameObject.GetComponentInChildren<TextMeshProUGUI>();
        //        text.DOFade(0, 0.2f).OnComplete(() =>
        //        {
        //            text.DOFade(1, 0.1f);
        //        });
        //    }
        //    else
        //    {
        //        var image = x.transform.GetComponent<Image>();
        //        image.DOFade(0, 1f).OnComplete(() =>
        //        {
        //            image.gameObject.SetActive(false);
        //            image.DOFade(buttonAlpha, 0.01f);
        //        });

        //        var text = x.gameObject.GetComponentInChildren<TextMeshProUGUI>();
        //        text.DOFade(0, 0.4f).OnComplete(() =>
        //        {
        //            text.DOFade(1, 0.1f);
        //        });
        //    }
        //});
    }

    public void EndDialogue()
    {
        dialUi.dialogueContainer.GetComponent<Image>().DOFade(0, 0.2f);
        dialUi.dialogueContainer.transform.DOScale(0, 0.5f).SetEase(Ease.InExpo).OnComplete(() => {
            dialUi.dialogueContainer.GetComponent<Image>().DOFade(panelAlpha, 0.1f);
            dialUi.dialogueContainer.transform.DOScale(1, 0.1f);
            runner.GetComponent<DialogueUI>().dialogueContainer.SetActive(false);
            });
    }
}


