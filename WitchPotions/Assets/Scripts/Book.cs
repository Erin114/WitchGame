using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Book : MonoBehaviour
{
    public GameObject bookUI;
    public BoxCollider2D coll;

    public Vector2 mousePos;

    public Material outline;
    public Material normal;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if (coll.OverlapPoint(mousePos))
            {
                //display book UI
                bookUI.SetActive(true);
            }

        }

    }

    private void OnMouseEnter()
    {
        GetComponent<SpriteOutline>().enabled = true;
        GetComponent<SpriteRenderer>().material = outline;
    }
    private void OnMouseExit()
    {
        GetComponent<SpriteOutline>().enabled = false;
        GetComponent<SpriteRenderer>().material = normal;

    }
}