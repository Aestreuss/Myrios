using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using COMMANDS;

namespace TESTING
{
    public class CommandTesting : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            StartCoroutine(Running());
        }

        IEnumerator Running()
        {
            yield return CommandManager.instance.Execute("print");
            yield return CommandManager.instance.Execute("print_1p", "hello world");
            yield return CommandManager.instance.Execute("print_mp", "line 1", "line 2", "line 3");

            yield return CommandManager.instance.Execute("lambda");
            yield return CommandManager.instance.Execute("lambda_1p", "hello world");
            yield return CommandManager.instance.Execute("lambda_mp", "line 1", "line 2", "line 3");

            yield return CommandManager.instance.Execute("process");
            yield return CommandManager.instance.Execute("process_1p", "3");
            yield return CommandManager.instance.Execute("process_mp", "process line 1", "process line 2", "process line 3");
        }
    }
}