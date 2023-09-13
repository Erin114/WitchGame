using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UCharts;

public class RecipeBuilder : MonoBehaviour
{
    //singleton logic
    public static RecipeBuilder Instance { get; private set; }
 

    public enum Emotion { terror,rage,grief,joy,vigilance,amazment,loathing,admiration};
    Dictionary<Emotion, int> emotionValues;
    public EmotionBar[] bars = new EmotionBar[4];
    public PieChart brewBalance;
    
    // Start is called before the first frame update
    void Start()
    {
        Set_Up_Dictionary();
    }

    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
    public void Set_Up_Dictionary()
    {
        emotionValues = new Dictionary<Emotion, int>();
        foreach (Emotion emotion in Emotion.GetValues(typeof(Emotion)))
        {
            emotionValues.Add(emotion, 0);
        }

    }
    public void SetPieChart()
    {
        List<PieChartDataNode> bar_values = new List<PieChartDataNode>();
        foreach (EmotionBar item in bars)
        {
            PieChartDataNode value = new PieChartDataNode();
            value.Text = item.barName;
            value.Value = item.TotalValue;
            bar_values.Add(value);
        }
        brewBalance.SetData(bar_values);
    }    

   

    // Update is called once per frame
    void Update()
    {
        
    }

     
}
