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

    // Start is called before the first frame update
    void Start()
    {
        buttons = new GameObject[ingredientsObjectArr.Length];
        for (int i = 0; i < ingredientsObjectArr.Length; i++)
        {
            buttons[i] = Instantiate(buttonPrefeb, ingredientMenuPanel.transform);
            buttons[i].GetComponent<Ingredient_Button_Class>().SetUp(PotionBehaviorManager, ingredientsObjectArr[i],this);
        }        
            
        
    }



    //Used to show pop up
    public float moveX = 80;
    public float moveY = 80;

    public void SetUpPopUp(Ingredients_SO ingredient, GameObject button)
    { 
        infoPanel.SetActive(true);
        Vector3 popUpLoction = button.transform.position;
        popUpLoction.x += moveX;
        popUpLoction.y += moveY;

        infoPanel.transform.position = popUpLoction;

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

    // Update is called once per frame
    void Update()
    {
        
    }
}
