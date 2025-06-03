using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.UI;

public class SuspicionMeter : MonoBehaviour
{
    public Slider slider;
    public float sliderSpeed;

    [Range(0f, 1f)]
    public float currentSuspicionLevel = 0.1f;

    [Space]
    #region Test
    public float testAmt;
    [ContextMenu("Alter Suspicion")]
    public void AlterSuspicion()
    {
        //This is what you will call in your dialogue choices.
        EditSuspicionLevel(testAmt);
    }
    #endregion

    private void Update()
    {
        //OR LERP
        slider.value = Mathf.MoveTowards(slider.value, currentSuspicionLevel, sliderSpeed * Time.deltaTime);
    }

    //In your dialogue choices, some choices can call this function.
    public void EditSuspicionLevel(float amt)
    {
        currentSuspicionLevel += amt;
        if (currentSuspicionLevel < 0)
            currentSuspicionLevel = 0;
        else if (currentSuspicionLevel >= 1)
        {
            //You lose, game ends. 
            Debug.Log("YOU LOSE!!!!");
        }

    }
}

////DEMONSTRATION OF HOW TO IMPLMENET SUSPICION FLOAT
//[System.Serializable]
//public class Choice
//{
//    public string title;
//    public string[] dialogue;
//    public float susAmt;
//}