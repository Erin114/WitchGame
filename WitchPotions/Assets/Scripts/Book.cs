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

    public AudioSource soundEffect;

    QuestionManager m;

    public List<int> discoveredIndexes;
    public List<Level_SO.NodeTypes> discoveredNodes;

    // Start is called before the first frame update
    void Start()
    {
        m = GameObject.Find("TempManager").GetComponent<QuestionManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && m.currentCharacter != null)
        {
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if (coll.OverlapPoint(mousePos))
            {
                //display book UI
                bookUI.SetActive(true);
                bookUI.GetComponent<BookUI>().ShowRevealedInfo(discoveredIndexes, discoveredNodes);
                soundEffect.Play();
            }

        }

    }

    private void OnMouseEnter()
    {       
        if(m.currentCharacter != null)
        {
            GetComponent<SpriteOutline>().enabled = true;
            GetComponent<SpriteRenderer>().material = outline;
        }
    }
    private void OnMouseExit()
    {
        GetComponent<SpriteOutline>().enabled = false;
        GetComponent<SpriteRenderer>().material = normal;

    }
}