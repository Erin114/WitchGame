using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Cauldron : MonoBehaviour
{
    public List<PotionUI> potions;
    public GameObject potionInfoUI;
    PotionUI potion;
    Level_SO level;
    public Material outline;
    public Material normal;

    public GameObject arrow;
    public GameObject wrongPotionPopUp;

    NPC c;

    // Start is called before the first frame update
    void Start()
    {
        c = GameObject.Find("TempManager").GetComponent<QuestionManager>().currentCharacter;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0) && GameObject.Find("TempManager").GetComponent<QuestionManager>().currentCharacter != null && !GameManager.Instance.servedPotion)
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

                        ToggleArrow(false);

                        //reveal emotional info from the book UI
                        GameObject.Find("Book").GetComponent<BookUI>().RevealEmotionalInfo(level);

                        //update the 'confirm potion' UI
                        
                        GameObject.Find("ConfirmPotion").GetComponent<EmotionInfoUI>().UpdateText(GameObject.Find("TempManager").GetComponent<QuestionManager>().amountOfDiscovered,
                                                                                                  GameObject.Find("TempManager").GetComponent<QuestionManager>().currentCharacter.hasBeenDiscovered.Length,
                                                                                                  potion.gameObject.GetComponent<Potion>().potionName);

                    }
                    else
                    {
                        wrongPotionPopUp.GetComponent<Animator>().SetTrigger("Reset");
                        wrongPotionPopUp.GetComponent<TMP_Text>().text = "Incorrect potion, try another...";
                    }

                }

            }

        }
    }

    public void ToggleArrow(bool val)
    {
        arrow.SetActive(val);
    }

    public bool ValidPotionChoice(PotionUI potionObj)
    {
        //check that the potion being brewed is a valid choice
        potion = potionObj;
        bool validChoice = false;

        /*for (int i = 0; i < potions.Count; i++)
        {
            if (potions[i].active)
            {
                potion = potions[i];
                Debug.Log(potion.gameObject.GetComponent<Potion>().potionName);
                //break;
            }
        }*/

        if (potion != null)
        {
            Debug.Log(potion.name);

            //get references to the current characters level objects
            List<Level_SO> levels = GameObject.Find("TempManager").GetComponent<QuestionManager>().currentCharacter.potionPossibilities;

            validChoice = false;

            for (int i = 0; i < levels.Count; i++)
            {
                if (potion.gameObject.GetComponent<Potion>().potionName == levels[i].potionName)
                {
                    validChoice = true;
                    level = levels[i];
                    //break;
                }
            }
        }

        if(GameManager.Instance.currentCustomerIndex == 0 && GameManager.Instance.currentDay == 0)
        {
            ToggleArrow(validChoice);
        }
       
        return validChoice;

    }

    public void StartBrewingProcess()
    {
        GameManager.Instance.SendInfoToPotionScene(level, GameObject.Find("TempManager").GetComponent<QuestionManager>().currentCharacter.hasBeenDiscovered);
    }

    private void OnMouseEnter()
    {
        if (GameObject.Find("TempManager").GetComponent<QuestionManager>().currentCharacter != null)
        {
            GetComponent<SpriteRenderer>().material = outline;
        }
            
    }
    private void OnMouseExit()
    {
        GetComponent<SpriteRenderer>().material = normal;

    }

}
