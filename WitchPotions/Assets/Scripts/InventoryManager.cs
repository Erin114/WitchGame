using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{

    public Dictionary<string, float[]> ingredients = new Dictionary<string, float[]>();
    // Start is called before the first frame update
    void Start()
    {
        ingredients.Add("rotate banane", new float[] { 0f,0f,15f});
        ingredients.Add("anger banen", new float[] { 5f, 0f, 0f });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
