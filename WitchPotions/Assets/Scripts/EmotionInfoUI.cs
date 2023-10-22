using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EmotionInfoUI : MonoBehaviour
{

    public Text amountDiscovered;
    public Text potionName;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateText(int discovered, int total, string potName)
    {
        amountDiscovered.text = discovered.ToString() + " charges/voids/bipolars discovered out of " + total;
        potionName.text = potName;
    }
}
