using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PotionTutorialText : MonoBehaviour
{
    public Dialogue[] convo;
    int convoIndex = 0;
    bool convoStarted = false;
    public Text t;
    public Text name;

    // Start is called before the first frame update
    void Start()
    {
        if(!(GameManager.Instance.currentDay == 0) || !(GameManager.Instance.currentCustomerIndex == 0))
        {
            this.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NextLine()
    {
        //if there is a convo to display, we arent at the end of it, and its the first interaction in the game
        if (convo != null && convoIndex < convo.Length - 1 && GameManager.Instance.currentDay == 0 && GameManager.Instance.currentCustomerIndex == 0)
        {
            //start the conversation with vivian's first response
            if (!convoStarted)
            {
                convoStarted = true;
                convoIndex = 0;
                name.text = convo[convoIndex].character.ToString().Replace('_', ' ');
                t.text = convo[convoIndex].text;
            }
            //continue the conversation till the end
            else
            {
                convoIndex++;
                t.text = convo[convoIndex].text;

                name.text = convo[convoIndex].character.ToString().Replace('_', ' ');

            }
        }
        //either there is no convo to display (showing intro text, or none exists, etc.) or we reached the end of it
        else
        {

            this.gameObject.SetActive(false);

            convo = null;

            
        }
    }

}
