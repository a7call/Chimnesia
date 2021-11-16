using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Yarn.Unity;

public class NPC : MonoBehaviour
{
    public string YarnStartNode 
    {
        get
        {
            return yarnStartNode;
        }
    }

    [SerializeField] YarnProgram yarnDialogue;
    [SerializeField] string yarnStartNode = "Start";
    [SerializeField] SpeakerData speakerData;
    GameManagement Manager;

    private void Awake()
    {
        Manager = GameManagement.GetInstance();
        Manager.AddSpeaker(speakerData);
        if (yarnDialogue != null)
            Manager.runner.Add(yarnDialogue);
    }

    private void OnDisable()
    {
        
    }

}
