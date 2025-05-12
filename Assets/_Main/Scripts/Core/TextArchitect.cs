using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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

    private const float baseSpeed = 1; //text speed can be universally changed
    private float speedMultiplier = 1; //can be changed by the user in config
}
