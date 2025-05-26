using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

// this script handles saving, loading and encryption of the files in this project
// note = a relative path will always be contained within the defined "root" directory (in the case gameData set in FilePaths) 
// meanwhile an absolute path can point to anywhere inside or outside the game folders

public class FileManager 
{
    public static List<string> ReadTextFile(string filePath, bool includeBlankLines = true)
    {
        if (!filePath.StartsWith('/')) //if '/' then it is an absolute path, if its not specifying an absolute path then its forcing it to be where local files are
            filePath = FilePaths.root + filePath;

        //reads the lines within the files 
        List<string> lines = new List<string>();
        try
        {
            using (StreamReader sr = new StreamReader(filePath))
            {
                while (!sr.EndOfStream) //while we still have lines to read
                {
                    string line = sr.ReadLine(); 
                    if (includeBlankLines || !string.IsNullOrWhiteSpace(line))
                        lines.Add(line);
                }
            }
        }
        catch (FileNotFoundException ex)
        {
            Debug.LogError($"File not found:'{ex.FileName}'");
        }

        return lines;
    }

    //this does the same thing as above just referenced and loaded inside of resources while above is an absolute path
    public static List<string> ReadTextAsset(string filePath, bool includeBlankLines = true) 
    {
        TextAsset asset = Resources.Load<TextAsset>(filePath);
        if (asset == null)
        {
            Debug.LogError($"Asset not found: '{filePath}'");
            return null;
        }

        return ReadTextAsset(asset, includeBlankLines);
    }

    //this passes in a text asset directly
    public static List<string> ReadTextAsset(TextAsset asset, bool includeBlankLines = true)
    {
        List<string> lines = new List<string>();
        using (StringReader sr = new StringReader(asset.text))
        {
            while (sr.Peek() > -1) //this peeks to see if there is a line available
            {
                string line = sr.ReadLine();
                if (includeBlankLines || !string.IsNullOrWhiteSpace(line))
                    lines.Add(line);
            }
        }

        return lines;
    }
}
