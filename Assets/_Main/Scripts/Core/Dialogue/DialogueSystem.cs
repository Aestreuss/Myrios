using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DIALOGUE
{
    public class DialogueSystem : MonoBehaviour
    {
        [SerializeField]private DialogueSystemConfigSO _config;
        public DialogueSystemConfigSO config => _config;


        public DialogueContainer dialogueContainer = new DialogueContainer();
        private ConversationManager conversationManager;
        private TextArchitect architect;

        public static DialogueSystem instance { get; private set; }

        public delegate void DialogueSystemEvent();
        public event DialogueSystemEvent onUserPrompt_Next;

        public bool isRunningConversation => conversationManager.isRunning;

        private void Awake()
        {
            //creates a singleton for this script, and makes sure there is only ever one in the scene
            if (instance == null)
            {
                instance = this;
                Initialize();
            }
            else
                DestroyImmediate(gameObject);
        }

        bool _initialized = false; //only want it to initialize once 
        private void Initialize()
        {
            if (_initialized) 
                return;

            architect = new TextArchitect(dialogueContainer.dialogueText);
            conversationManager = new ConversationManager(architect);
        }

        public void OnUserPrompt_Next()
        {
            onUserPrompt_Next?.Invoke();
        }

        public void ShowSpeakerName(string speakerName = "")
        {
            if (speakerName.ToLower() != "narrator") //tolower makes it case sensitive
                dialogueContainer.nameContainer.Show(speakerName);
            else
                HideSpeakerName();
        }

        public void HideSpeakerName() => dialogueContainer.nameContainer.Hide();

        public Coroutine Say(string speaker, string dialogue)
        {
            List<string> conversation = new List<string>() { $"{speaker} \"{dialogue}\"" };
            return Say(conversation);
        }

        public Coroutine Say(List<string> conversation)
        {
            return conversationManager.StartConversation(conversation);
        }

    }
}

