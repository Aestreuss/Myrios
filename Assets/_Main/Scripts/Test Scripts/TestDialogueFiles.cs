using System.Collections;
using System.Collections.Generic;
using DIALOGUE;
using UnityEngine;

public class TestDialogueFiles : MonoBehaviour
{
    [SerializeField] private TextAsset file;
    // Start is called before the first frame update
    void Start()
    {
        StartConversation();

        //string line = "Speaker \"Dialogue \\\"goes in\\\" here.\" Command(argument here)";

        //DialogueParser.Parse(line);

    }

    void StartConversation()
    {
        List<string> lines = FileManager.ReadTextAsset(file);

        foreach (string line in lines)
        {
            if (line == string.Empty)
                continue;
            DIALOGUE_LINE dl = DialogueParser.Parse(line);
        }
    }
}
