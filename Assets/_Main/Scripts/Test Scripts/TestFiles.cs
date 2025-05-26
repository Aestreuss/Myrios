using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TESTING
{
    public class TestFiles : MonoBehaviour
    {
        // private string fileName = "testFile"; //note = when loading something from resources, you do not specify the file extention, only the name, otherwise it can't find it 

        [SerializeField] private TextAsset fileName;
        // Start is called before the first frame update
        void Start()
        {
            StartCoroutine(Run());
        }

        IEnumerator Run()
        {
            List<string> lines = FileManager.ReadTextAsset(fileName, false); //setting to true keeps blank lines, setting to false removes blank lines / reading from asset accesses resources, from file is from the direct gameData

            foreach (string line in lines)
                Debug.Log(line);
            yield return null;
        }

    }
}

