using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cauldron : MonoBehaviour
{
    public List<PotionUI> potions;
    public GameObject potionInfoUI;
    PotionUI potion;
    Level_SO level;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0) && GameObject.Find("TempManager").GetComponent<QuestionManager>().currentCharacter != null)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if(GetComponent<BoxCollider2D>().OverlapPoint(mousePos))
            {
                //display brewing UI where charges,voids,bipolars and the final potion are revealed

                //check that the potion being brewed is a valid choice
                potion = new PotionUI();

                for(int i = 0; i < potions.Count; i++)
                {
                    if(potions[i].active)
                    {
                        potion = potions[i];
                        Debug.Log(potion.gameObject.GetComponent<Potion>().potionName);
                        //break;
                    }
                }

                if(potion != null)
                {
                    Debug.Log(potion.name);

                    //get references to the current characters level objects
                    List<Level_SO> levels = GameObject.Find("TempManager").GetComponent<QuestionManager>().currentCharacter.potionPossibilities;

                    bool validChoice = false;

                    for (int i = 0; i < levels.Count; i++)
                    {
                        if (potion.gameObject.GetComponent<Potion>().potionName == levels[i].potionName)
                        {
                            validChoice = true;
                            level = levels[i];
                            //break;
                        }
                    }

                    Debug.Log(validChoice + "| " + levels.Count);

                    if (validChoice)
                    {
                        //display UI showing charges discovered, etc.
                        potionInfoUI.SetActive(true);

                        //reveal emotional info from the book UI
                        GameObject.Find("Book").GetComponent<BookUI>().RevealEmotionalInfo(level);

                        //update the 'confirm potion' UI
                        GameObject.Find("ConfirmPotion").GetComponent<EmotionInfoUI>().UpdateText(GameObject.Find("TempManager").GetComponent<QuestionManager>().amountOfDiscovered,
                                                                                                  GameObject.Find("TempManager").GetComponent<QuestionManager>().currentCharacter.hasBeenDiscovered.Length,
                                                                                                  potion.gameObject.GetComponent<Potion>().potionName);

                    }

                }

            }

        }
    }

    public void StartBrewingProcess()
    {
        GameManager.Instance.SendInfoToPotionScene(level, GameObject.Find("TempManager").GetComponent<QuestionManager>().currentCharacter.hasBeenDiscovered);
    }

}
