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
    public Text name;
    public Text patience;
    Button introButton;
    Button questionOne;
    Button questionTwo;
    Button questionThree;
    Button nextButton;

    public List<GameObject> characterPrefabs;
    CharacterList list;
    public int currentChar = 0;

    public NPC startingCharacter;
    public NPC currentCharacter;
    public List<NPC> characters;

    // Start is called before the first frame update
    void Start()
    {
        list = GameObject.Find("TempManager").GetComponent<JSONManager>().list;
        characters = new List<NPC>();

        introButton = GameObject.Find("IntroButton").GetComponent<Button>();
        questionOne = GameObject.Find("QuestionOne").GetComponent<Button>();
        questionTwo = GameObject.Find("QuestionTwo").GetComponent<Button>();
        questionThree = GameObject.Find("QuestionThree").GetComponent<Button>();
        nextButton = GameObject.Find("NextButton").GetComponent<Button>();
        //introButton = GameObject.Find("QuestionThree").GetComponent<Button>();

        introButton.onClick.AddListener(ShowIntroText);
        questionOne.onClick.AddListener(AnswerOne);
        questionTwo.onClick.AddListener(AnswerTwo);
        questionThree.onClick.AddListener(AnswerThree);
        nextButton.onClick.AddListener(NextCustomer);

        questionOne.gameObject.SetActive(false);
        questionTwo.gameObject.SetActive(false);
        nextButton.gameObject.SetActive(false);
        questionThree.gameObject.SetActive(false);

        //spawn in all characters, enable only the first one
        for(int i = 0; i < characterPrefabs.Count; i++)
        {
            GameObject temp = Instantiate(characterPrefabs[i]);
            temp.transform.position = new Vector3(-6.75f, -2.5f);
            characters.Add(temp.GetComponent<NPC>());
            temp.GetComponent<NPC>().LoadCharacterInfo();

            //assign the currentChar index
            if (i > 0)
            {
                temp.SetActive(false);
            }
            else
            {
                currentChar = temp.GetComponent<NPC>().ID;
            }
        }

        currentCharacter = characters[currentChar];
        name.text = "Name: " + currentCharacter.charName;
        ChangeCharacter(characters[currentChar]);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeCharacter(NPC character)
    {
        //NextCustomer();

        currentCharacter = character;

        UpdatePatience();

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
        t.text = currentCharacter.GenericResponse(0);
        questionOne.gameObject.SetActive(false);

        if(!questionOne.gameObject.activeSelf && !questionTwo.gameObject.activeSelf)
        {
            nextButton.gameObject.SetActive(true);
        }

        UpdatePatience();

    }

    void AnswerTwo()
    {
        t.text = currentCharacter.GenericResponse(1);
        questionTwo.gameObject.SetActive(false);

        /*if (!questionOne.gameObject.activeSelf && !questionTwo.gameObject.activeSelf)
        {
            nextButton.gameObject.SetActive(true);
        }*/

        questionThree.gameObject.SetActive(true);

        UpdatePatience();

    }

    void AnswerThree()
    {
        t.text = currentCharacter.SpecificResponse(0);

        questionThree.gameObject.SetActive(false);

        if (!questionOne.gameObject.activeSelf && !questionTwo.gameObject.activeSelf)
        {
            nextButton.gameObject.SetActive(true);
        }

        UpdatePatience();
    }

    void NextCustomer()
    {
        t.text = "Hi, I need help";

        currentChar++;

        //disable the old character
        currentCharacter.Reset();
        currentCharacter.gameObject.SetActive(false);

        if(currentChar == list.characters.Count)
        {
            currentChar = 0;
        }

        currentCharacter = characters[currentChar];

        //enable the new character
        currentCharacter.gameObject.SetActive(true);

        name.text = "Name: " + currentCharacter.charName;
        Debug.Log(currentCharacter.charName);

        introButton.gameObject.SetActive(true);
        nextButton.gameObject.SetActive(false);

        UpdatePatience();

    }

    void UpdatePatience()
    {
        patience.text = "Patience: " + currentCharacter.Patience.ToString();
    }

}
