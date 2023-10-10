using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestionManager : MonoBehaviour
{

    public GameObject questionScrollBar;
    public GameObject characterText;

    public Text t;
    public Text name;
    public Text patience;
    public Button introButton;
    public Button nextButton;

    public List<Button> genericQuestions;
    public List<Button> emotionQuestions;

    public List<GameObject> characterPrefabs;
    CharacterList list;

    //index for the current character in the list
    public int currentChar = 0;

    public NPC startingCharacter;
    public NPC currentCharacter;
    public List<NPC> characters;

    //Added by Elad 10/10/2023 - UI and Patiance Bar
    public Slider bar;
    public Image face;
    public Sprite [] allFace;
    public int annoyedQuestionsCount = 0;


    // Start is called before the first frame update
    void Start()
    {
        list = GameObject.Find("TempManager").GetComponent<JSONManager>().list;
        characters = new List<NPC>();

        //spawn in all characters, enable only the first one
        for (int i = 0; i < characterPrefabs.Count; i++)
        {
            GameObject temp = Instantiate(characterPrefabs[i]);
            temp.transform.position = new Vector3(0.0f, 0.0f);
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

    public void ChangeCharacter(NPC character)
    {
        //NextCustomer();

        currentCharacter = character;
        annoyedQuestionsCount = 0;
        UpdatePatience();

    }

    public void ShowIntroText()
    {
        list = GameObject.Find("TempManager").GetComponent<JSONManager>().list;
        t.text = list.characters[currentChar].intro;
        introButton.gameObject.SetActive(false);

        //show all generic questions and emotion specific questions
        for(int i = 0; i < genericQuestions.Count; i++)
        {
            genericQuestions[i].interactable = true;
        }

        for(int i = 0; i < emotionQuestions.Count; i++)
        {
            emotionQuestions[i].interactable = true;
        }

    }

    //called by buttons to ask generic questions
    public void AskGenericQuestion(int index)
    {
        t.text = currentCharacter.GenericResponse(index);

        genericQuestions[index].gameObject.SetActive(false);

        UpdatePatience();
    }

    //called by buttons to ask emotion-specific questions
    public void AskEmotionQuestion(int index)
    {
        t.text = currentCharacter.SpecificResponse(index);

        emotionQuestions[index].gameObject.SetActive(false);

        UpdatePatience();
    }

    /*public void AnswerOne()
    {
        t.text = currentCharacter.GenericResponse(0);
        questionOne.gameObject.SetActive(false);

        if (!questionOne.gameObject.activeSelf && !questionTwo.gameObject.activeSelf)
        {
            nextButton.gameObject.SetActive(true);
        }

        UpdatePatience();

    } 

    public void AnswerTwo()
    {
        t.text = currentCharacter.GenericResponse(1);
        questionTwo.gameObject.SetActive(false);

        if (!questionOne.gameObject.activeSelf && !questionTwo.gameObject.activeSelf)
        {
            nextButton.gameObject.SetActive(true);
        }

        questionThree.gameObject.SetActive(true);
        questionThree.interactable = true;

        UpdatePatience();

    }

    public void AnswerThree()
    {
        t.text = currentCharacter.SpecificResponse(0);

        questionThree.gameObject.SetActive(false);

        if (!questionOne.gameObject.activeSelf && !questionTwo.gameObject.activeSelf)
        {
            nextButton.gameObject.SetActive(true);
        }

        UpdatePatience();
    }*/

    //prepares the scene for the next customer
    public void NextCustomer()
    {
        t.text = "Hi, I need help";

        //update current character index
        currentChar++;

        //disable the old character
        currentCharacter.Reset();
        currentCharacter.gameObject.SetActive(false);

        //go back to the start of the character list when its ran out ~~~ temp code
        if (currentChar == list.characters.Count)
        {
            currentChar = 0;
        }

        currentCharacter = characters[currentChar];

        //enable the new character
        currentCharacter.gameObject.SetActive(true);

        name.text = "Name: " + currentCharacter.charName;
        Debug.Log(currentCharacter.charName);

        introButton.gameObject.SetActive(true);
        //nextButton.gameObject.SetActive(false);

        //show all generic questions and emotion specific questions
        for (int i = 0; i < genericQuestions.Count; i++)
        {
            genericQuestions[i].interactable = false;
            genericQuestions[i].gameObject.SetActive(true);
        }

        for (int i = 0; i < emotionQuestions.Count; i++)
        {
            emotionQuestions[i].interactable = false;
            emotionQuestions[i].gameObject.SetActive(true);
        }

        UpdatePatience();

    }

    void UpdatePatience()
    {
        patience.text = "Patience: " + currentCharacter.Patience.ToString();
        if (currentCharacter.Patience >0)
        {
            bar.value = (currentCharacter.Patience / currentCharacter.maxPatience) * 100;
        }
        else
        {
            bar.value = 0;
            annoyedQuestionsCount++;
        }
        if (currentCharacter.Patience >75)
        {
            face.sprite = allFace[0];
        }
        else if (currentCharacter.Patience > 50)
        {
            face.sprite = allFace[1];
        }
        else if (currentCharacter.Patience > 25)
        {
            face.sprite = allFace[2];
        }
        else  
        {
            face.sprite = allFace[3];
        }
    }

    public void ShowScrollBar()
    {
        StartCoroutine(DisableCharacterText());
    }

    public void ShowCharacterText()
    {
        StartCoroutine(DisableScroll());
    }

    IEnumerator DisableScroll()
    {
        yield return new WaitForSeconds(0f);

        questionScrollBar.SetActive(false);
        characterText.SetActive(true);

    }

    IEnumerator DisableCharacterText()
    {
        yield return new WaitForSeconds(0f);

        questionScrollBar.SetActive(true);
        characterText.SetActive(false);
    }


}
