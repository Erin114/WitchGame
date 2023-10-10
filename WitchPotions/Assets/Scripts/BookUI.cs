using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class BookUI : MonoBehaviour
{

    public List<Potion> potions;
    public List<CustomToggle> emotions;

    [SerializeField]
    private List<Emotions> activeEmotions;
    private List<Emotions> inactiveEmotions;

    // Start is called before the first frame update
    void Start()
    {
        activeEmotions = new List<Emotions>();
        inactiveEmotions = new List<Emotions>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateUI()
    {
        activeEmotions.Clear();
        inactiveEmotions.Clear();

        //get all the active / inactive emotions
        for(int i = 0; i < emotions.Count; i++)
        {
            if (emotions[i].toggleState == ToggleStates.Yes)
            {
                activeEmotions.Add(emotions[i].emotion);
            }
            else if(emotions[i].toggleState == ToggleStates.No)
            {
                inactiveEmotions.Add(emotions[i].emotion);
            }
        }

        //display certain values based on this
        for(int i = 0; i < potions.Count; i++)
        {
            //check against each emotion in the potion's tag list
            for(int j = 0; j < potions[i].tags.Count; j++)
            {


                bool containsActiveEmotions = true;

                //for each potion loop through all the active emotions
                for (int a = 0; a < activeEmotions.Count; a++)
                {
                    if (!potions[i].tags.Contains(activeEmotions[a]))
                    {
                        containsActiveEmotions = false;
                    }
                }


                if(containsActiveEmotions)
                {
                    potions[i].gameObject.SetActive(true);
                }
                else
                {
                    potions[i].gameObject.SetActive(false);
                }

                //for each potion loop through all inactive emotions
                for (int n = 0; n < inactiveEmotions.Count; n++)
                {
                    if (potions[i].tags.Contains(inactiveEmotions[n]))
                    {
                        potions[i].gameObject.SetActive(false);
                        
                    }
                }

            }

        }

    }
}
