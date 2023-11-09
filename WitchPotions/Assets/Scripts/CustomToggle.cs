using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum ToggleStates
{
    Empty,
    Yes,
    No
}

public class CustomToggle : MonoBehaviour, IPointerClickHandler
{

    public UnityEvent customToggleClicked;

    [SerializeField]
    public ToggleStates toggleState = ToggleStates.Empty;

    public GameObject x;
    public GameObject check;
    public Emotions emotion;

    public BookUI book;

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Clicked custom toggle");
        ChangeToggleValue();
        book.UpdateUI();
        customToggleClicked.Invoke();
    }

    //Text 

    // Start is called before the first frame update
    void Start()
    {
        book = GameObject.Find("Book").GetComponent<BookUI>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateGraphics()
    {
        switch (toggleState)
        {
            case 0:
                //hide a certain graphic, show another one
                x.SetActive(false);
                check.SetActive(false);

                break;

            case ToggleStates.Yes:

                x.SetActive(false);
                check.SetActive(true);

                break;

            case ToggleStates.No:

                x.SetActive(true);
                check.SetActive(false);

                break;


        }
    }

    public void ChangeToggleValue()
    {
        switch(toggleState)
        {
            case 0:
                //hide a certain graphic, show another one
                x.SetActive(false);
                check.SetActive(true);

                //change states
                toggleState = ToggleStates.Yes;
                break;

            case ToggleStates.Yes:

                x.SetActive(true);
                check.SetActive(false);

                toggleState = ToggleStates.No;
                break;

            case ToggleStates.No:

                x.SetActive(false);
                check.SetActive(false);

                toggleState = ToggleStates.Empty;
                break;


        }
    }

}
