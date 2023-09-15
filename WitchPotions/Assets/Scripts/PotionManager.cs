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
    float[] bars = new float[8] { 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f }; 
    //rage (0) terror(1), grief(2), joy (3),vigilance(4), amazement(5),loathing(6), admiration (7)
    int money = 0;
    int moneySpent;
    List<Potion> pots;

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
    public void AddQuality(int qualityValue, int qualityIndex)
    {
        
        bars[qualityIndex] += qualityValue;
    }

    /// <summary>
    /// Finishes The potion, sending it to the game manager
    /// then resets the potion stats
    /// </summary>
    void FinishPotion()
    {
        Potion pot = new Potion(bars);
        pots.Add(pot);
        Debug.Log(pot.GetStats());
        ResetPotion();
        moneySpent = 0;
    }

    /// <summary>
    /// Resets the currently brewed potion
    /// 
    /// </summary>
    void ResetPotion()
    {
        bars = new float[8] { 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f };
    }

    void ResetMoney()
    {
        money += moneySpent;
    }
    

}
