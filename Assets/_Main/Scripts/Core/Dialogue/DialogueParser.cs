using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

//this is a system that handles parsing functions to convert strings into DIALOGUE_LINES
//note = in general, adding \ (known as escaping) in from of anything that is usually used in code (for example \" or \') makes it so that is functions as normal character and ignored. 

namespace DIALOGUE
{
    public class DialogueParser 
    {
        // \w is a word character * is any character amount any amount of times \s is white space
        private const string commandRegexPattern = @"[\w\[\]]*[^\s]\("; //so this formula is, a word of any length as long as it is not proceeded by a white space and looking for a (
        public static DIALOGUE_LINE Parse(string rawLine)
        {
            Debug.Log($"Parsing line - '{rawLine}'");

            (string speaker, string dialogue, string commands) = RipContent(rawLine);

            Debug.Log($"Speaker = '{speaker}'\nDialogue = '{dialogue}'\nCommands = '{commands}'");

            return new DIALOGUE_LINE(speaker, dialogue, commands);
        }

        private static (string, string, string) RipContent(string rawLine)
        {
            string speaker = "", dialogue = "", commands = "";

            int dialogueStart = -1;
            int dialogueEnd = -1;
            bool isEscaped = false;

            //this finds where the start and the end of the dialogue is and also ignores escaped characters that may indicate something else
            for (int i = 0; i < rawLine.Length; i++)
            {
                char current = rawLine[i];
                if (current == '\\')
                    isEscaped = !isEscaped;
                else if (current == '"' && !isEscaped)
                {
                    if (dialogueStart == -1)
                        dialogueStart = i;
                    else if (dialogueEnd == -1)
                        dialogueEnd = i;

                }
                else
                    isEscaped = false;
            }

            //Debug.Log(rawLine.Substring(dialogueStart + 1, dialogueEnd - dialogueStart - 1));

            //identify command pattern
            //formula for commands = CommandName(arguments go here) example. PlaySong("Hi there" -v 1 -p 0.3). white space splits each arguemnt quotations encapsulate two word names and stuff
            Regex commandRegex = new Regex(commandRegexPattern);
            Match match = commandRegex.Match(rawLine);
            int commandStart = -1;
            if (match.Success)
            {
                //this is if command is detected with no dialogue/speaker
                commandStart = match.Index;

                if (dialogueStart == -1 && dialogueEnd == -1)
                    return ("", "", rawLine.Trim());

            }

            //this is for in the case that dialogue or multiword arguments are in the command, it checks to see if it is dialogue 
            if (dialogueStart != -1 && dialogueEnd != -1 && (commandStart == -1 || commandStart > dialogueEnd))
            {
                //at this point we know that there is dialogue
                speaker = rawLine.Substring(0, dialogueStart).Trim();
                dialogue = rawLine.Substring(dialogueStart + 1, dialogueEnd - dialogueStart - 1).Replace("\\\"","\"");
                if (commandStart != -1)
                    commands = rawLine.Substring(commandStart).Trim();
            }
            else if (commandStart != -1 && dialogueStart > commandStart)
                commands = rawLine;
            else 
                speaker = rawLine;

                return (speaker, dialogue, commands);
        }
    }
}

