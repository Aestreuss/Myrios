using System.Collections;
using System.Collections.Generic;
using DIALOGUE;
using UnityEngine;
using COMMANDS;

public class TestDialogueFiles : MonoBehaviour
{
    [SerializeField] private TextAsset file = null;
    public bool dialogueRead = false;


    void Start()
    {
        StartConversation();

    }

    void StartConversation()
    {

        List<string> lines = FileManager.ReadTextAsset(file);


        DialogueSystem.instance.Say(lines);

        if (lines != null)
        {
            Debug.Log("finish");
            dialogueRead = true;
        }


    }
}
