using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class NodeVisitedTracker : MonoBehaviour
{
    public DialogueRunner dialogueRunner; // assign in inspector
    HashSet<string> _visitedNodes = new HashSet<string>(); // this could be a HashSet if we want more efficient search


    void Start()
    {
        dialogueRunner.AddFunction("visited", 1, delegate (Yarn.Value[] parameters)
        {
            var nodeName = parameters[0];
            return _visitedNodes.Contains(nodeName.AsString);
        }
        );

    }

    public void NodeComplete(string nodeName)
    {
        _visitedNodes.Add(nodeName);
    }
    public bool WasVisited(string nodeName)
    {
        return _visitedNodes.Contains(nodeName);
    }
}
