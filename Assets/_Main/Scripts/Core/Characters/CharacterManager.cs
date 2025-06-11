using DIALOGUE;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

namespace CHARACTERS
{
    public class CharacterManager : MonoBehaviour
    {
        public static CharacterManager instance { get; private set; }
        private Dictionary<string, Character> characters = new Dictionary<string, Character>();

        private CharacterConfigSO config => DialogueSystem.instance.config.characterConfigurationAsset;

        private const string CHARACTER_CASTING_ID = "as";
        private const string CHARACTER_NAME_ID = "<charname>";
        private string characterRootPath => $"Characters/{CHARACTER_NAME_ID}";
        private string characterPrefabPath => $"{characterRootPath}/Character - [{CHARACTER_NAME_ID}]";

        [SerializeField] private RectTransform _characterpanel = null;
        public RectTransform characterPanel => _characterpanel;

        private void Awake()
        {
            instance = this;
        }

        public Character GetCharacter(string characterName, bool createIfDoesNotExist = false)
        {
            if(characters.ContainsKey(characterName.ToLower()))
                return characters[characterName.ToLower()];
            else if (createIfDoesNotExist)
                return CreateCharacter(characterName);

            return null;
        }

        public Character CreateCharacter(string characterName, bool revealAfterCreation = false)
        {
            if (characters.ContainsKey(characterName.ToLower()))
            {
                Debug.LogWarning($"A character called '{characterName}' already exists. Did not create the character.");
                return null;
            }

            Character_Info info = GetCharacterInfo(characterName);

            Character character = CreateCharacterFromInfo(info);

            characters.Add(characterName.ToLower(), character);

            if (revealAfterCreation)
                character.Show();

            return character;
        }

        private Character_Info GetCharacterInfo(string characterName)
        {
            Character_Info result = new Character_Info();

            string[] nameData = characterName.Split(CHARACTER_CASTING_ID, System.StringSplitOptions.RemoveEmptyEntries);
            result.name = nameData[0];
            result.castingName = nameData.Length > 1 ? nameData[1] : result.name;

            result.config = config.GetConfig(characterName);

            result.prefab = GetPrefabForCharacter(characterName);

            return result;
        }

        private GameObject GetPrefabForCharacter(string characterName)
        {
            string prefabPath = FormatCharacterPath(characterPrefabPath, characterName);
            return Resources.Load<GameObject>(prefabPath);
        }

        private string FormatCharacterPath(string path, string characterName) => path.Replace(CHARACTER_NAME_ID, characterName);

        private Character CreateCharacterFromInfo(Character_Info info)
        {
            switch (info.config.characterType)
            {
                case Character.CharacterType.Text:
                    return new Character_Text(info.name);

                case Character.CharacterType.Sprite:
                case Character.CharacterType.SpriteSheet:
                    return new Character_Sprite(info.name, info.prefab);

                default:
                    return null;
            }


        }

        private class Character_Info
        {
            public string name = "";
            public string castingName = "";

            public CharacterConfigData config = null;

            public GameObject prefab = null;
        }
    }
}