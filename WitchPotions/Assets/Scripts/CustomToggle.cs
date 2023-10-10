using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CustomToggle : MonoBehaviour, IPointerClickHandler
{

    public UnityEvent customToggleClicked;

    [SerializeField]
    private int toggleState = 0;

    public GameObject x;
    public GameObject check;

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Clicked custom toggle");
        ChangeToggleValue();
        //customToggleClicked.Invoke();
    }

    //Text 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
                toggleState++;
                break;

            case 1:

                x.SetActive(true);
                check.SetActive(false);

                toggleState++;
                break;

            case 2:

                x.SetActive(false);
                check.SetActive(false);

                toggleState = 0;
                break;


        }
    }

}
