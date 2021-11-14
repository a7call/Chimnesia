using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Yarn.Unity;
using UnityEngine.UI;
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

    protected override void Awake()
    {
        base.Awake();
       
    }
    private void Start()
    {
        dialogueUI.SetActive(false);
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
        if (playerPositions.Count <= position +1)
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

        if(speakerDataBase.TryGetValue(speaker, out SpeakerData data))
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
        SetNPCState(false);
        runner.StartDialogue(currentNPC.YarnStartNode);
    }

    public void CompleteDialogue()
    {
        isInDialogue = false;
        runner.GetComponent<DialogueUI>().optionButtons.ForEach(o => o.gameObject.SetActive(false));
        SetNPCState(true);
    }
    public void SetNPCState(bool active)
    {
        UnActiveDuringDialogue.ForEach(n => n.SetActive(active));
    }
}