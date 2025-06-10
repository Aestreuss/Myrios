using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CHARACTERS;

namespace TESTING
{
    public class TestCharacters : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            Character Asterios = CharacterManager.instance.CreateCharacter("Asterios");
            Character StoryTeller = CharacterManager.instance.CreateCharacter("StoryTeller");
            Character Myrth = CharacterManager.instance.CreateCharacter("Myrth");

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}