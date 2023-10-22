using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;


public class PotionUI : MonoBehaviour, IPointerClickHandler
{

    public bool active = false;

    public GameObject indicator;

    BookUI book;

    public void OnPointerClick(PointerEventData eventData)
    {

        book.UpdatePotions();

        active = !active;
        UpdateGraphic(active);
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

        Debug.Log("cliek");
    }

    void Start()
    {
        book = GameObject.Find("Book").GetComponent<BookUI>();
    }

}
