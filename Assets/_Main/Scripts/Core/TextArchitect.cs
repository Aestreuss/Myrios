using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using static TextArchitect;

public class TextArchitect 
{
    private TextMeshProUGUI tmpro_ui;
    private TextMeshPro tmpro_world;
    public TMP_Text tmpro => tmpro_ui != null ? tmpro_ui : tmpro_world; //if tmpro ui has space then it uses the tmpro ui if not it will use tmpro world text

    public string currentText => tmpro.text;
    public string targetText { get; private set; } = ""; //what we are trying to build, can be publicly retrieved but only privately assigned
    public string preText { get; private set; } = ""; //whatever is already on the architect will be stored here before the new text is pasted on top of it 
    private int preTextLength = 0;

    public string fullTargetText => preText + targetText; //will give full string that the architect will be building

    public enum BuildMethod { instant, typewriter, fade} //different types of way text will appear
    public BuildMethod buildMethod = BuildMethod.typewriter; //default set to typewriter

    public Color textColour { get {  return tmpro.color; } set { tmpro.color = value; } } //changes text colour

    public float speed { get { return baseSpeed * speedMultiplier; } set { speedMultiplier = value; } }  //how fast the text speed will be 
    private const float baseSpeed = 1; //text speed can be universally changed
    private float speedMultiplier = 1; //can be changed/configured by the user in config

    public int characterPerCycle { get { return speed <= 2f ? characterMultiplier : speed <= 2.5f ? characterMultiplier * 2 : characterMultiplier * 3; } } 
    // ^ if speed is less than 2 it will be the default speed, if less than equal it will become 2 characters per cycle
    private int characterMultiplier = 1;

    public bool hurryUp = false; //whether the text will speed up or not

    public TextArchitect(TextMeshProUGUI tmpro_ui)
    {
        this.tmpro_ui = tmpro_ui;
    }

    public TextArchitect(TextMeshPro tmpro_world)
    {
        this.tmpro_world = tmpro_world;
    }

    public Coroutine Build(string text)
    {
        preText = "";
        targetText = text;
        //for building text

        Stop();

        buildProcess = tmpro.StartCoroutine(Building());
        return buildProcess;
    }

    //appends text to what is already in the text architect
    public Coroutine Append(string text)
    {
        preText = tmpro.text;
        targetText = text;

        Stop();

        buildProcess = tmpro.StartCoroutine(Building());
        return buildProcess;
    }

    private Coroutine buildProcess = null;
    public bool isBuilding => buildProcess != null;

    public void Stop()
    {
        //if not building it does nothing
        if (!isBuilding)
            return; 

        //stops the text build process
        tmpro.StopCoroutine(buildProcess);
        buildProcess = null;
    }

    IEnumerator Building()
    {
        Prepare();

        switch(buildMethod)
        {
            case BuildMethod.typewriter:
                yield return Build_Typewriter(); //yield for however long this is
                break;
            case BuildMethod.fade:
                yield return Build_Fade();
                break;
        }
    }

    private void OnComplete()
    {
        buildProcess = null;
    }

    //instant text
    private void Prepare()
    {
        switch (buildMethod)
        {
            case BuildMethod.instant:
                Prepare_Instant();
                break;
            case BuildMethod.typewriter:
                Prepare_Typewriter();
                break;
            case BuildMethod.fade:
                Prepare_Fade();
                break;
        }
    }

    private void Prepare_Instant()
    {
        tmpro.color = tmpro.color; //reapplies the initial colour back onto the texts so thats its fully visible (could be from aftermath of fade text)
        tmpro.text = fullTargetText; //sets text directly to what we want to build
        tmpro.ForceMeshUpdate(); //any changes made will be applied when this is called
        tmpro.maxVisibleCharacters = tmpro.textInfo.characterCount; //makes sure every character is visible on screen
    }
    private void Prepare_Typewriter()
    {

    }
    private void Prepare_Fade()
    {

    }

    //typrewriter text
    private IEnumerator Build_Typewriter()
    {
        yield return null;
    }

    //fade text
    private IEnumerator Build_Fade()
    {
        yield return null;
    }
}
