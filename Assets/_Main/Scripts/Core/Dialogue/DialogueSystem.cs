using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DIALOGUE
{
    public class DialogueSystem : MonoBehaviour
    {
        public DialogueContainer dialogueContainer = new DialogueContainer();
        private ConversationManager conversationManager = new ConversationManager();

        public static DialogueSystem instance;

        public bool isRunningConversation => conversationManager.isRunning;

        private void Awake()
        {
            //creates a singleton for this script, and makes sure there is only ever one in the scene
            if (instance == null)
                instance = this;
            else
                DestroyImmediate(gameObject);
        }

        public void Say(string speaker, string dialogue)
        {
            List<string> conversation = new List<string>() { $"{speaker} \"{dialogue}\"" };
            Say(conversation);
        }

        public void Say(List<string> conversation)
        {
            conversationManager.StartConversation(conversation);
        }

    }
}

