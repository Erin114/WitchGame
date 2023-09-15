using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : MonoBehaviour
{
    float[] potionStats;
    float[] barRatios = { 0.0f, 0.0f, 0.0f, 0.0f };
    public float[] GetStats() { return potionStats; }
    public float[] GetRatios() { return barRatios; }
    int strength = 0;
    // Start is called before the first frame update
    public Potion(float[] stats)
    {
        potionStats = stats;
        int ratioindex = 0;
        
        for (int i = 0; i < 7; i += 2)
        {
            //develop potion ratios to read for recipes
            barRatios[ratioindex] = ((potionStats[i] + potionStats[i + 1]) / potionStats[i]) * 100;
            ratioindex++;
            //strength calculation
            strength += Mathf.RoundToInt(potionStats[i] + potionStats[i + 1]);
        }
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
