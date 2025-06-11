using DIALOGUE;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

namespace CHARACTERS
{
    public abstract class Character
    {
        public string name = "";
        public string displayName = "";
        public RectTransform root = null;
        public Animator animator;

        protected CharacterManager manager => CharacterManager.instance;
        public DialogueSystem dialogueSystem => DialogueSystem.instance;

        //coroutines
        protected Coroutine co_revealing, co_hiding;

        public bool isRevealing => co_revealing != null;
        public bool isHiding => co_hiding != null;
        public virtual bool isVisible { get; set; }

        public Character(string name, GameObject prefab)
        {
            this.name = name;
            displayName = name;

            if (prefab != null)
            {
                GameObject ob = Object.Instantiate(prefab, manager.characterPanel);
                ob.SetActive(true);
                root = ob.GetComponent<RectTransform>();
                animator = root.GetComponentInChildren<Animator>();
            }
        }

        public Coroutine Say(string dialogue) => Say(new List<string> { dialogue });

        public Coroutine Say(List<string> dialogue)
        {
            dialogueSystem.ShowSpeakerName(displayName);
            return dialogueSystem.Say(dialogue);
        }

        public virtual Coroutine Show()
        {
            if (isRevealing)
                return co_revealing;

            if (isHiding)
                manager.StopCoroutine(co_hiding);



            co_revealing = manager.StartCoroutine(ShowingOrHiding(true));

            return co_revealing;
        }
        
        public virtual Coroutine Hide()
        {
            if (isHiding)
                return co_hiding;

            if (isRevealing)
                manager.StopCoroutine(co_revealing);

            co_hiding = manager.StartCoroutine(ShowingOrHiding(false));

            return co_hiding;
        }

        public virtual IEnumerator ShowingOrHiding(bool show)
        {
            Debug.Log("Show/Hide cannot be called from a base character type.");
            yield return null;
        }

        public enum CharacterType
        {
            Text,
            Sprite,
            SpriteSheet
        }
    }
}