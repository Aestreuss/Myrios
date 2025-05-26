using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DIALOGUE
{
    public class DialogueSystem : MonoBehaviour
    {
        public DialogueContainer dialogueContainer = new DialogueContainer();


        public static DialogueSystem instance;

        private void Awake()
        {
            //creates a singleton for this script, and makes sure there is only ever one in the scene
            if (instance == null)
                instance = this;
            else
                DestroyImmediate(gameObject);
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}

