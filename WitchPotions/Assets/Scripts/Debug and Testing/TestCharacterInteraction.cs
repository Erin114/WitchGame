using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum QuestionPhases
{
    Intro,
    Questioning
}

public class TestCharacterInteraction : MonoBehaviour
{
    QuestionPhases phase = QuestionPhases.Intro;
    public Text t;
    Button introButton;
    Button questionOne;
    Button questionTwo;
    Button questionThree;
    Button nextButton;

    CharacterList list;
    public int currentChar;

    // Start is called before the first frame update
    void Start()
    {
        list = GameObject.Find("TempManager").GetComponent<JSONManager>().list;

        introButton = GameObject.Find("IntroButton").GetComponent<Button>();
        questionOne = GameObject.Find("QuestionOne").GetComponent<Button>();
        questionTwo = GameObject.Find("QuestionTwo").GetComponent<Button>();
        nextButton = GameObject.Find("NextButton").GetComponent<Button>();
        //introButton = GameObject.Find("QuestionThree").GetComponent<Button>();

        introButton.onClick.AddListener(ShowIntroText);
        questionOne.onClick.AddListener(AnswerOne);
        questionTwo.onClick.AddListener(AnswerTwo);
        nextButton.onClick.AddListener(NextCustomer);

        questionOne.gameObject.SetActive(false);
        questionTwo.gameObject.SetActive(false);
        nextButton.gameObject.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ShowIntroText()
    {
        list = GameObject.Find("TempManager").GetComponent<JSONManager>().list;
        t.text = list.characters[currentChar].intro;
        introButton.gameObject.SetActive(false);
        questionOne.gameObject.SetActive(true);
        questionTwo.gameObject.SetActive(true);
    }

    void AnswerOne()
    {
        t.text = list.characters[currentChar].responses[0];
        questionOne.gameObject.SetActive(false);

        if(!questionOne.gameObject.activeSelf && !questionTwo.gameObject.activeSelf)
        {
            nextButton.gameObject.SetActive(true);
        }

    }

    void AnswerTwo()
    {
        t.text = list.characters[currentChar].responses[1];
        questionTwo.gameObject.SetActive(false);

        if (!questionOne.gameObject.activeSelf && !questionTwo.gameObject.activeSelf)
        {
            nextButton.gameObject.SetActive(true);
        }

    }

    void NextCustomer()
    {
        t.text = "Hi, I need help";

        currentChar++;
        
        if(currentChar == list.characters.Count)
        {
            currentChar = 0;
        }

        introButton.gameObject.SetActive(true);
        nextButton.gameObject.SetActive(false);
    }

}
