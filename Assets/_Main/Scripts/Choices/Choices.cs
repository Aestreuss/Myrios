using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DIALOGUE
{
    public class Choices : MonoBehaviour
    {
        public GameObject choice1;
        public GameObject choice2;
        public bool choice1Active = false;
        public bool choice2Active = false;
        public Button choose1;

        [SerializeField] private TextAsset file1 = null;
        public bool dialogueRead = false;

        public ConversationManager cm;


        void Start()
        {
            StartConversation();
        }

        void Update()
        {

        }

        public void ShowChoice1()
        {

            if (!dialogueRead == true)
            {
                choice1.SetActive(true);
                choice1Active = true;
            }

            dialogueRead = false;
        }

        public void ShowChoice2()
        {
            if (!choice1Active && dialogueRead == true)
            {
                choice1.SetActive(false);
                choice2.SetActive(true);
                choice2Active = true;
                choice1Active = false;
            }

        }
        void StartConversation()
        {
            List<string> lines = FileManager.ReadTextAsset(file1);

            DialogueSystem.instance.Say(lines);
        }

    }
}