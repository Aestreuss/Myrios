using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this script is an access hub for file and directory locations 
public class FilePaths
{
    //takes the default path for whatever application is (for example assets or anything like that) and set it to a defined directory 
    public static readonly string root = $"{Application.dataPath}/gameData/"; 
}
