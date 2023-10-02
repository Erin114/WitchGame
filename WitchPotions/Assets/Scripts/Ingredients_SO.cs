
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/IngredientsScriptableObject", order = 1)]
public class Ingredients_SO : ScriptableObject
{
    public string ingredients_Name;
    public int ingredients_Price;
    public int ingredients_Poison;
    public Sprite ingredients_Sprite;
    public string [] ingredients_Emotion;
    public int [] ingredients_Value;
    public (string [] emotion, int [] value) Ingredients_Vector
    {
        get {
            (string[] emotion, int[] value) ingredients_Vector;
            ingredients_Vector.emotion = ingredients_Emotion;
            ingredients_Vector.value = ingredients_Value;

            return ingredients_Vector;
        }
    }

}