using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using COMMANDS;

namespace TESTING
{

    //this script will serve as an example for how to add future commands to the command system
    //note = all commands must be STATIC, this is so they can be added without object references to the database. Inside these commands, you can access other non-static items likes items in the scene and other class instances.

    public class CMD_DatabaseExtensionExamples : CMD_DatabaseExtension
    {
        new public static void Extend(CommandDatabase database)
        {
            //add actions with no parameters
            database.AddCommand("print", new Action(PrintDefaultMessage));
            database.AddCommand("print_1p", new Action<string>(PrintUserMessage));
            database.AddCommand("print_mp", new Action<string[]>(PrintLines));

            //add lambda with no parameters
            database.AddCommand("lambda", new Action(() => { Debug.Log("Printing a default message to console from lambda command."); }));
            database.AddCommand("lambda_1p", new Action<string>((arg) => { Debug.Log($"Log User Lambda Message: '{arg};"); }));
            database.AddCommand("lambda_mp", new Action<string[]>((args) => { Debug.Log(string.Join(", ", args)); }));

            //add coroutine with no parameters
            database.AddCommand("process", new Func<IEnumerator>(SimpleProcess));
            database.AddCommand("process_1p", new Func<string, IEnumerator>(LineProcess));
            database.AddCommand("process_mp", new Func<string[], IEnumerator>(MultiLineProcess));

        }

        private static void PrintDefaultMessage()
        {
            Debug.Log("Printing a default message to console.");
        }

        private static void PrintUserMessage(string message)
        {
            Debug.Log($"User Message: '{message}'");
        }

        private static void PrintLines(string[] lines)
        {
            int i = 1;
            foreach (string line in lines)
            {
                Debug.Log($"{i++}. '{line}'");

            }
        }

        private static IEnumerator SimpleProcess()
        {
            for (int i = 1; i <= 5; i++)
            {
                Debug.Log($"Process Running... [{i}]");
                yield return new WaitForSeconds(1);
            }
        }

        private static IEnumerator LineProcess(string data)
        {
            if (int.TryParse(data, out int num))
            {
                for (int i = 1; i <= num; i++)
                {
                    Debug.Log($"Process Running... [{i}]");
                    yield return new WaitForSeconds(1);
                }
            }

        }

        private static IEnumerator MultiLineProcess(string[] data)
        {
            foreach (string line in data)
            {
                Debug.Log($"Process Message: '{line}'");
                yield return new WaitForSeconds(0.5f);
            }

        }
    }
}