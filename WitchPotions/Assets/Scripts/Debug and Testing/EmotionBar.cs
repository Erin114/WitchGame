using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EmotionBar : MonoBehaviour
{
    public enum Emotion { terror, rage, grief, joy, vigilance, amazment, loathing, admiration };
    public string positiveName;
    public string negativeName;
    public string barName;

    //-----------Emotion date-----------------//

    //Current value of positive/negative emotions
    public int emotionNegative;
    public int emotionPositive;
    //False = negative is dominent 
    //True = positive is dominent
    bool dominent = false;
    //Current ratio of bar;
    private float ratio; // field
    public float Ratio   // property
    {
        get { return ratio; }   // get method
    }
    //Return total value of all emotions
    public int TotalValue { get { return emotionNegative + emotionPositive; } }

    //-----------------UI elements---------------------//
    public Slider sliderPostive;
    public Slider sliderNegative;
    public TMP_Text ratioText;
    public TMP_Text emotionPositiveText;
    public TMP_Text emotionNegativeText;
    public TMP_InputField input;

    public void findRaio ()
    {
        float dominentEmotion = 0;
        float weakEmotion = 0;

        //If both emotion are the same the bar's value is 0
        if (emotionNegative == emotionPositive)
        {
            ratio = 0;
            return; 
        }



        if (emotionNegative > emotionPositive)
        { 
            dominentEmotion = emotionNegative;
            weakEmotion = emotionPositive;
            dominent = false;
        }
        else
        {
            dominentEmotion = emotionPositive;
            weakEmotion = emotionNegative;
            dominent = true;
        }

        ratio = ((dominentEmotion - weakEmotion) / dominentEmotion) * 100;
        ShowStats();
    }
    
    public void addNegative()
    {
        emotionNegative += int.Parse(input.text);
        findRaio();
    }
    public void addPositive()
    {
        emotionPositive += int.Parse(input.text); 
        findRaio();
    }
    public void ResetBar()
    {
        emotionPositive = 0;
        emotionNegative = 0;
        findRaio();
    }

    public void ShowStats()
    {
        //We only need to work on the dominent slider, we should also set the other one to 0;
        Slider dominentSlider;
        if (dominent)
        {
            dominentSlider = sliderPostive;
            sliderNegative.value = 0;
        }
        else
        {
            dominentSlider = sliderNegative;
            sliderPostive.value = 0;
        }
        dominentSlider.value = ratio;
        ratioText.text = "Ratio: %" + (int)ratio;
        emotionPositiveText.text = positiveName+": " + emotionPositive.ToString();
        emotionNegativeText.text = negativeName+ ": " + emotionNegative.ToString();
        RecipeBuilder.Instance.SetPieChart();
    }
}
