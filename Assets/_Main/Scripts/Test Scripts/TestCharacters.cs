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
            //Character Asterios = CharacterManager.instance.CreateCharacter("Asterios");
            //Character StoryTeller = CharacterManager.instance.CreateCharacter("StoryTeller");
            //Character Myrth = CharacterManager.instance.CreateCharacter("Myrth");

            StartCoroutine(Test());

        }

        IEnumerator Test()
        {
            Character Myrth = CharacterManager.instance.CreateCharacter("Myrth");

            yield return Myrth.Show();
            yield return new WaitForSeconds(0.5f);
            yield return Myrth.Hide();
            yield return new WaitForSeconds(0.5f);
            yield return Myrth.Show();
            yield return Myrth.Say("uh.. hello there");
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}