using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class Ingredient_Menu : MonoBehaviour
{
    //Reference which is later sent to btton to connect them to the potion manager 
    public PotionBehavior PotionBehaviorManager;

    //Panal to place al ingredients in
    public GameObject ingredientMenuPanel;
    //List of all Ingredients_SO in game
    public Ingredients_SO [] ingredientsObjectArr;
    //Prefeb of button
    public GameObject buttonPrefeb;
    GameObject[] buttons;

    //Popup info
    //Info popup
    public GameObject infoPanel;
    public TextMeshProUGUI popUpPrice;
    public TextMeshProUGUI popUpPoison;
    public TextMeshProUGUI popUpData;

    //Slection Options:
    Dictionary<string, List<GameObject>> ingredientsDic = 
    new Dictionary<string, List<GameObject>>();
    Dictionary<string, bool> ingredientsAtiveCheck =
    new Dictionary<string, bool>();
    bool allActive;

    // Start is called before the first frame update
    void Start()
    {
        SetUpIngredientDic();
        buttons = new GameObject[ingredientsObjectArr.Length];
        for (int i = 0; i < ingredientsObjectArr.Length; i++)
        {
            buttons[i] = Instantiate(buttonPrefeb, ingredientMenuPanel.transform);
            buttons[i].GetComponent<Ingredient_Button_Class>().SetUp(PotionBehaviorManager, ingredientsObjectArr[i],this);
            
            //Set up ingredient dictonery 

            //Set up all ingredients into an arrey depending on what emotions the effect
            for (int j = 0; j < ingredientsObjectArr[i].ingredients_Emotion.Length; j++)
            {
                switch (ingredientsObjectArr[i].ingredients_Emotion[j])
                {
                    case "Terror":
                        ingredientsDic["Terror"].Add(buttons[i]);
                        break;
                    case "Admiration":
                        ingredientsDic["Admiration"].Add(buttons[i]);
                        break;
                    case "Joy":
                        ingredientsDic["Joy"].Add(buttons[i]);
                        break;
                    case "Vigilance":
                        ingredientsDic["Vigilance"].Add(buttons[i]);
                        break;
                    case "Rage":
                        ingredientsDic["Rage"].Add(buttons[i]);
                        break;
                    case "Loathing":
                        ingredientsDic["Loathing"].Add(buttons[i]);
                        break;
                    case "Grief":
                        ingredientsDic["Grief"].Add(buttons[i]);
                        break;
                    case "Amazement":
                        ingredientsDic["Amazement"].Add(buttons[i]);
                        break;
                    default:
                        Debug.Log("error, ingredient doesn't exists");
                        break;

                }
            }
            
        }
        //all of the options are active at the starts
        allActive = true;
       

    }

    public void SetUpIngredientDic()
    {
        ingredientsDic.Add("Terror", new List<GameObject>());
        ingredientsDic.Add("Admiration", new List<GameObject>());
        ingredientsDic.Add("Joy", new List<GameObject>());
        ingredientsDic.Add("Vigilance", new List<GameObject>());
        ingredientsDic.Add("Rage", new List<GameObject>());
        ingredientsDic.Add("Loathing", new List<GameObject>());
        ingredientsDic.Add("Grief", new List<GameObject>());
        ingredientsDic.Add("Amazement", new List<GameObject>());

        ingredientsAtiveCheck.Add("Terror", true);
        ingredientsAtiveCheck.Add("Admiration", true);
        ingredientsAtiveCheck.Add("Joy", true);
        ingredientsAtiveCheck.Add("Vigilance", true);
        ingredientsAtiveCheck.Add("Rage", true);
        ingredientsAtiveCheck.Add("Loathing", true);
        ingredientsAtiveCheck.Add("Grief", true);
        ingredientsAtiveCheck.Add("Amazement", true);
    }

    public void SetUpPopUp(Ingredients_SO ingredient)
    {
        infoPanel.SetActive(true);
        popUpPrice.text = "Price - " + ingredient.ingredients_Price.ToString();
      popUpPoison.text = "Poison - " + ingredient.ingredients_Poison.ToString();
        for (int i = 0; i < ingredient.ingredients_Emotion.Length; i++)
        {
            popUpData.text += ingredient.ingredients_Emotion[i] + " = " + ingredient.ingredients_Value[i] + "\n";
        }
    }
    public void ClearUpPopUp()
    {
        infoPanel.SetActive(false);
        popUpPrice.text = "";
        popUpPoison.text = "";
        popUpData.text = "";
        
    }

    public void ShowCaseSpesificIng(string Key)
    {
        //If this is the first filter selected, set all other ingredients to false to show only the one clicked
            foreach (var item in ingredientsDic)
            {
                    //Set all ingredent in key yo false
                    ingredientsAtiveCheck[item.Key] = false;
                    foreach (var ing in item.Value)
                    {
                        ing.SetActive(false);
                    }
            }
     
            ingredientsAtiveCheck[Key] = true;
            foreach (var ing in ingredientsDic[Key])
            {
                ing.SetActive(true);
            }
        
    }
    public void ShowAllIng ()
    {
        foreach (var item in ingredientsDic)
        {
                ingredientsAtiveCheck[item.Key] = true;
                foreach (var ing in item.Value)
                {
                    ing.SetActive(true);
                }        

        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
