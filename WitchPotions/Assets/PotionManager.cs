using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionManager : MonoBehaviour
{

    // GameManager gamemanager;
    // int array PotionQualities {rage/terror, grief/joy,
    // vigilance/amazement, loathing/admiration};
    // goes from -25 to 25 on each;
    float poison;
    float[] topBar = { 0f, 0f, 0f, 0f }; //rage,grief,vigilance,loathing
    float[] bottomBar = { 0f, 0f, 0f, 0f }; //terror,joy,amazement,admiration
    int money = 0;
    int moneySpent;
    // int money;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    /// <summary>
    /// Adds the quality of a given value (positive or negative) to the potion array
    /// </summary>
    /// <param name="qualityValue"></param>
    /// <param name="qualityIndex"></param>
    void AddQuality(int qualityValue, int qualityIndex)
    {
        
    }

    /// <summary>
    /// Finishes The potion, sending it to the game manager
    /// then resets the potion stats
    /// </summary>
    void FinishPotion()
    {
        moneySpent = 0;
    }

    /// <summary>
    /// Resets the currently brewed potion
    /// 
    /// </summary>
    void ResetPotion()
    {
        moneySpent = 0;
    }

    void ResetMoney()
    {
        money += moneySpent;
    }
    

}
