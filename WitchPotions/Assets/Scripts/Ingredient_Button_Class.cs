using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Ingredient_Button_Class : MonoBehaviour
{
    // Start is called before the first frame update
    public PotionBehavior PotionBehaviorManager;
    public Ingredients_SO ingredient;
    public TextMeshProUGUI ingName;
    public Image image;
    Ingredient_Menu menu;
    

    public void SetUp(PotionBehavior behavior, Ingredients_SO ingredientSO, Ingredient_Menu menu)
    {
        //Set up buttton info
        ingredient = ingredientSO;
        PotionBehaviorManager = behavior;
        ingName.text = ingredient.ingredients_Name;
        image.sprite = ingredient.ingredients_Sprite;
        this.menu = menu;
       
    }
    public void SendIngredient()
    {
        PotionBehaviorManager.AddIngredient(ingredient);
        PotionBehaviorManager.HoverOverIngredeint(ingredient);

    }
 
    public void HoverStart()
    {
        menu.SetUpPopUp(ingredient, this.gameObject);

        PotionBehaviorManager.HoverOverIngredeint(ingredient);
    }
    public void HoverEnd()
    {
        menu.ClearUpPopUp();

        PotionBehaviorManager.HoverEnd();

    }
}
