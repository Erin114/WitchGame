using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;


public class PotionUI : MonoBehaviour, IPointerClickHandler
{

    public bool active = false;
    public bool isLocked = false;

    public GameObject indicator;

    BookUI book;

    public void OnPointerClick(PointerEventData eventData)
    {

        book.UpdatePotions();

        if(!isLocked)
        {
            active = !active;
            UpdateGraphic(active);
        }

        //if(active)
        //{
            bool isValidPotionChoice = GameObject.Find("Desk").transform.Find("Cauldron").GetComponent<Cauldron>().ValidPotionChoice(this);

            //if(isValidPotionChoice)
            //{
                //display the arrow above the cauldron
            //}

        //}

    }

    public void UpdateGraphic(bool a)
    {
        if(a)
        {
            indicator.SetActive(true);
        }
        else
        {
            indicator.SetActive(false);
        }

        //Debug.Log("cliek");
    }

    void Start()
    {
        book = GameObject.Find("Book").GetComponent<BookUI>();
    }

}
