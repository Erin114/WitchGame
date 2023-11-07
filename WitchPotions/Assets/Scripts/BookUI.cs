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

    public List<int> discoveredIndexes;
    public List<Level_SO.NodeTypes> discoveredNodes;

    // Start is called before the first frame update
    void Start()
    {
        activeEmotions = new List<Emotions>();
        inactiveEmotions = new List<Emotions>();

        discoveredIndexes = new List<int>();
        discoveredNodes = new List<Level_SO.NodeTypes>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdatePotions()
    {
        //clear potions
        for(int i = 0; i < potions.Count; i++)
        {
            potions[i].gameObject.GetComponent<PotionUI>().active = false;
            potions[i].gameObject.GetComponent<PotionUI>().UpdateGraphic(false);
        }
    }

    public void ShowRevealedInfo(List<int> discoveredIndexes, List<Level_SO.NodeTypes> discoveredNodes)
    {
        //reveal discovered info
        for (int i = 0; i < discoveredIndexes.Count; i++)
        {
            RevealInfoInBook(discoveredIndexes[i], discoveredNodes[i]);
        }
    }

    public void RevealInfoInBook(int emotionIndex, Level_SO.NodeTypes nodeType)
    {
        //loop through emotions
        for(int i = 0; i < emotions.Count; i++)
        {
            //loop through indices for each emotion until they match
            for(int a = 0; a < GameManager.Instance.emotionalIndexes[i].Length; a++)
            {
                //if they match, change the state of the toggle inside the book
                if(GameManager.Instance.emotionalIndexes[i][a] == emotionIndex)
                {
                    switch(nodeType)
                    {
                        case Level_SO.NodeTypes.bipolar:

                            break;

                        case Level_SO.NodeTypes.charger:
                            emotions[i].toggleState = ToggleStates.Yes;
                            emotions[i].UpdateGraphics();
                            Debug.Log("rara");
                            break;

                        case Level_SO.NodeTypes.voidNode:
                            emotions[i].toggleState = ToggleStates.No;
                            emotions[i].UpdateGraphics();
                            break;
                    }
                }
            }
        }
    }

    public void RevealEmotionalInfo(Level_SO level)
    {

        QuestionManager m = GameObject.Find("TempManager").GetComponent<QuestionManager>();

        for(int i = 0; i < emotions.Count; i++)
        {
            //charges
            if(emotions[i].toggleState == ToggleStates.Yes)
            {
                //loop through each indice/node
                for(int a = 0; a < level.emotionIndices.Length; a++)
                {
                    //check it against the indices for that emotion
                    for(int k = 0; k < GameManager.Instance.emotionalIndexes[i].Length; k++)
                    {
                        //if the indicee is the same, and its a charger then reveal the charger
                        if(GameManager.Instance.emotionalIndexes[i][k] == level.emotionIndices[a] && level.nodeType[a] == Level_SO.NodeTypes.charger)
                        {
                            //if we havent already counted this charger
                            if(!m.currentCharacter.hasBeenDiscovered[a])
                            {
                                m.currentCharacter.hasBeenDiscovered[a] = true;
                                m.amountOfDiscovered++;
                            }
                        }
                    }
                }
            }
            //voids
            else if(emotions[i].toggleState == ToggleStates.No)
            {
                //loop through each indice/node
                for (int a = 0; a < level.emotionIndices.Length; a++)
                {
                    //check it against the indices for that emotion
                    for (int k = 0; k < GameManager.Instance.emotionalIndexes[i].Length; k++)
                    {
                        //if the indicee is the same, and its a charger then reveal the charger
                        if (GameManager.Instance.emotionalIndexes[i][k] == level.emotionIndices[a] && level.nodeType[a] == Level_SO.NodeTypes.voidNode)
                        {
                            //if we havent already counted this void
                            if (!m.currentCharacter.hasBeenDiscovered[a])
                            {
                                m.currentCharacter.hasBeenDiscovered[a] = true;
                                m.amountOfDiscovered++;
                            }
                        }
                    }
                }
            }
        }

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
        /*for(int i = 0; i < potions.Count; i++)
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

        }*/

    }
}
