using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cauldron : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if(GetComponent<BoxCollider2D>().OverlapPoint(mousePos))
            {
                //display brewing UI where charges,voids,bipolars and the final potion are revealed
            }

        }
    }
}
