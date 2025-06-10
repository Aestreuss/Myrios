using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CHARACTERS;
using DIALOGUE;

namespace TESTING
{
    public class TestCharacters : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            
            //Character StoryTeller = CharacterManager.instance.CreateCharacter("StoryTeller");
            //Character Myrth = CharacterManager.instance.CreateCharacter("Myrth");

            StartCoroutine(Test());

        }

        IEnumerator Test()
        {
            Character Asterios = CharacterManager.instance.CreateCharacter("Asterios");

            List<string> lines = new List<string>()
            {
                "HI",
                "this is just a line",
                "and another line",
                "and the final line"
            };
            yield return Asterios.Say(lines);
          
            Debug.Log("finished");
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}