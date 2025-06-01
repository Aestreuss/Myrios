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

        OnComplete();
    }

    private void OnComplete()
    {
        buildProcess = null;
        hurryUp = false;
    }

    //double clicking speeds up, triple click completes it
    public void ForceComplete()
    {
        switch (buildMethod)
        {
            case BuildMethod.typewriter:
                tmpro.maxVisibleCharacters = tmpro.textInfo.characterCount;
                break;
            case BuildMethod.fade:
                tmpro.ForceMeshUpdate();
                break;
        }

        Stop();
        OnComplete();
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
        //resetting itself to make sure its ready for starting
        tmpro.color = tmpro.color;
        tmpro.maxVisibleCharacters = 0;
        tmpro.text = preText;

        //this checks for if there is pretext and updates before checking if it is visible
        if (preText != "")
        {
            tmpro.ForceMeshUpdate();
            tmpro.maxVisibleCharacters = tmpro.textInfo.characterCount;
        }

        tmpro.text += targetText; //adds the target
        tmpro.ForceMeshUpdate();
    }
    private void Prepare_Fade()
    {
        tmpro.text = preText;
        if (preText != "")
        {
            tmpro.ForceMeshUpdate();
            preTextLength = tmpro.textInfo.characterCount;
        }
        else
            preTextLength = 0;

        tmpro.text += targetText;
        tmpro. maxVisibleCharacters = int.MaxValue;
        tmpro.ForceMeshUpdate();

        TMP_TextInfo textInfo = tmpro.textInfo; 

        Color colorVisable = new Color(textColour.r, textColour.g, textColour.b, 1);
        Color colorHidden = new Color(textColour.r, textColour.g, textColour.b, 0);

        Color32[] vertexColors = textInfo.meshInfo[textInfo.characterInfo[0].materialReferenceIndex].colors32;

        for (int i = 0; i < textInfo.characterCount; i++)
        {
            TMP_CharacterInfo charInfo = textInfo.characterInfo[i];

            if(!charInfo.isVisible) 
                continue;

            if (i < preTextLength)
            {
                for(int v = 0; v < 4; v++)
                    vertexColors[charInfo.vertexIndex + v] = colorVisable;
            }
            else
            {
                for (int v = 0; v < 4; v++)
                    vertexColors[charInfo.vertexIndex + v] = colorHidden;
            }
        }

        tmpro.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
    }

    //typrewriter text
    private IEnumerator Build_Typewriter()
    {
        //makes sure we are getting the max visible characters to the max character count
        while (tmpro.maxVisibleCharacters < tmpro.textInfo.characterCount)
        {
            tmpro.maxVisibleCharacters += hurryUp ? characterPerCycle * 5: characterPerCycle; 

            yield return new WaitForSeconds(0.015f / speed); //makes sure it doesn't appear instantly 
        }

    }

    //fade text
    private IEnumerator Build_Fade()
    {
        int minRange = preTextLength;
        int maxRange = minRange + 1;

        byte alphaThreshold = 15;

        TMP_TextInfo textInfo = tmpro.textInfo;

        Color32[] vertexColors = textInfo.meshInfo[textInfo.characterInfo[0].materialReferenceIndex].colors32;
        float[] alphas = new float[textInfo.characterCount]; //transitioning or lerping the colours from color32 is choppy because its in bytes so creating a new alpha that holds the info will be smoother

        while (true)
        {
            float fadeSpeed = ((hurryUp ? characterPerCycle * 5 : characterPerCycle) * speed) *4f;

            for (int i = minRange; i < maxRange; i++)
            {
                TMP_CharacterInfo charInfo = textInfo.characterInfo[i];

                if (!charInfo.isVisible)
                    continue;

                int vertexIndex = textInfo.characterInfo[i].vertexIndex;
                alphas[i] = Mathf.MoveTowards(alphas[i], 255, fadeSpeed);

                for (int v = 0; v < 4; v++)
                    vertexColors[charInfo.vertexIndex + v].a = (byte)alphas[i];

                if (alphas[i] >= 255)
                    minRange++;
            }

            tmpro.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);

            bool lastCharacterIsInvisible = !textInfo.characterInfo[maxRange - 1].isVisible;
            if (alphas[maxRange - 1] > alphaThreshold || lastCharacterIsInvisible) 
            {
                if (maxRange < textInfo.characterCount) 
                    maxRange++;
                else if (alphas[maxRange - 1] >= 255 || lastCharacterIsInvisible) //if last character has reached max opaqueness it will break out of this and end build
                    break;
            }

            yield return new WaitForEndOfFrame();
        }


    }
}
