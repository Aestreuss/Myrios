using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

//logic controller for DialogueContainer's speakers name field and controls the visibility and other logic independently 
//is part of the dialogue container and holds the name text 

namespace DIALOGUE
{
    [System.Serializable]
    public class NameContainer
    {
        [SerializeField] private GameObject root;
        [SerializeField] private TextMeshProUGUI nameText;

        public void Show(string nameToShow = "")
        {
            root.SetActive(true);

            if (nameToShow != string.Empty)
                nameText.text = nameToShow;
        }

        public void Hide()
        {
            root.SetActive(false);
        }
    }
}

