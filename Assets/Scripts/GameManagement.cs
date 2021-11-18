using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Yarn.Unity;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using System;
using UnityEngine.SceneManagement;

public class GameManagement : Singleton<GameManagement>
{
    public static bool isInDialogue = false;

    [Header("Dialogue UI")]
    [SerializeField] GameObject dialogueUI;
    [SerializeField] Image speakerPortrait;
    [SerializeField] GameObject txt_speakerName;
    public List<SpeakerData> speakers;

    [Header("BackGround")]
    [SerializeField] Image PresentBackgroundImage;
    [SerializeField] Image PastBackgroundImage;
    [SerializeField] Animator bgAnimator;
    public Animator endCanvas;
    public TextMeshProUGUI endText;
    public Animator evanouis;

    //[SerializeField] TextMeshProUGUI txt_speakerName;

    [Header("Player")]
    [SerializeField] GameObject player;
    [SerializeField] List<Transform> playerPositions;
    private int position = 0;


    public DialogueRunner runner;

    string Fin1 = "Merci d'avoir joué. Auriez-vous pu montrer à la fillette ce qu'est la musique, si vous auviez fait les choses différemment ?";
    string Fin2 = "Merci d'avoir joué. Vous avez réussi à montrer ce que peut être la musique.";
    string FinNulle = "Merci d'avoir joué. Auriez-vous pu montrer à la fillette ce qu'est la musique, si vous auviez fait les choses différemment ?";
    string FinUni = "Merci d'avoir joué. Que ce serait-il passé si vous aviez pris ces Larmes ?";




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

        speakers.ForEach(s => AddSpeaker(s));

    }
    private void Start()
    {
        dialUi.dialogueContainer = dialogueUI;
       // dialogueUI.SetActive(false);
        runner.AddCommandHandler("SetSpeaker", SetSpeakerInfo);
        runner.AddCommandHandler("PlaySoundFx", PlaySoudFX);
        runner.AddCommandHandler("StopFx", StopFx);
        runner.AddCommandHandler("StopMusic", StopMusic);
        runner.AddCommandHandler("ChangeAmbiance", PlayAmbiance);
        runner.AddCommandHandler("GoPast", GoToPast);
        runner.AddCommandHandler("CharleInterract", PastCharleInteraction);
        runner.AddCommandHandler("ContrebasseInterract", PastContrebasseInteraction);
        runner.AddCommandHandler("End", End);
        runner.AddCommandHandler("Evanouis", Evanouis);
    }

    private void Evanouis(string[] parameters)
    {
        evanouis.SetTrigger("Evanouis");     
    }

    IEnumerator GoToPlayScene()
    {
        yield return new WaitForSeconds(6f);
        SceneManager.LoadScene(0);
    }

    private void End(string[] parameters)
    {
        if(parameters[0] == "Fin1")
        {
            endText.text = Fin1;
        }
        else if(parameters[0] == "Fin2")
        {
            endText.text = Fin2;
        }
        else if(parameters[0] == "FinNulle")
        {
            endText.text = FinNulle;
        }
        else if(parameters[0] == "FinUni")
        {
            endText.text = FinUni;
        }
            
        
        endCanvas.SetTrigger("End");
        StartCoroutine(GoToPlayScene());
    }

    private void StopMusic(string[] parameters)
    {
        var musicName = parameters[0];
        MusicManager.GetInstance().Stop(musicName);
    }

    private void StopFx(string[] parameters)
    {
        var musicName = parameters[0];
        EffectManager.GetInstance().Stop(musicName);
    }

    private void PlayAmbiance(string[] parameters)
    {
        var musicName = parameters[0];
        MusicManager.GetInstance().Play(musicName);
    }

    public bool isInPresent = true;
    private void GoToPast(string[] parameters)
    {
        if (isInPresent)
        {
            isInPresent = false;
            PresentBackgroundImage.DOFade(0, 1f);
            PastBackgroundImage.DOFade(1, 1f);
        }
        else
        {
            isInPresent = true;
            PresentBackgroundImage.DOFade(1, 1f);
            PastBackgroundImage.DOFade(0, 1f);
        }
    }
    public bool isContrebasse = false;
    public void PastCharleInteraction(string[] parameters)
    {
        if (isContrebasse)
            bgAnimator.SetTrigger("RevertContrebasse");

        bgAnimator.SetTrigger("Interact");
    }
    public void PastContrebasseInteraction(string[] parameters)
    {

        isContrebasse = true;
        bgAnimator.SetTrigger("InteractContrebasse");
    }
    public void PlaySoudFX(string[] info)
    {
        string soundName = info[0];
        EffectManager.GetInstance().Play(soundName, EffectManager.GetInstance().gameObject);
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
        if (info[0] == "none")
        {
            speakerPortrait.gameObject.SetActive(false);
            txt_speakerName.transform.parent.gameObject.SetActive(false);
            return;
        }

        txt_speakerName.transform.parent.gameObject.SetActive(true);
        var speaker = info[0];
        string emotion = info.Length > 1 ? info[1].ToLower() : SpeakerData.EMOTION_NEUTRAL;
        print(emotion);

        if (speakerDataBase.TryGetValue(speaker, out SpeakerData data))
        {
            
            speakerPortrait.sprite = data.GetEmotionPortrait(emotion);
            if (data.GetEmotionPortrait(emotion) == null)
                speakerPortrait.gameObject.SetActive(false);
            else
                speakerPortrait.gameObject.SetActive(true);

            txt_speakerName.GetComponent<TextMeshProUGUI>().text = data.speakerName;
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
        runner.StartDialogue(currentNPC.YarnStartNode);
    }

    public void CompleteDialogue()
    {
        isInDialogue = false;
        runner.GetComponent<DialogueUI>().optionButtons.ForEach(o => o.gameObject.SetActive(false));
    }

    public Vector3 punch;
    public float duration;


    public void EndOptions()
    {
        var selectedOption = EventSystem.current.currentSelectedGameObject;      
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


