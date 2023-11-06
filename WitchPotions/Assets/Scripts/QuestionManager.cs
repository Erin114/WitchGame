using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestionManager : MonoBehaviour
{
    public GameObject questionScrollBar;
    public GameObject characterText;
    public GameObject convoButton;
    public GameObject conversationBox;

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
    public int currentDay;

    public GameObject spawnLocation;

    public NPC startingCharacter;
    public NPC currentCharacter;
    public List<NPC> characters;

    public int amountOfDiscovered = 0;

    //Added by Elad 10/10/2023 - UI and Patiance Bar
    public Slider bar;
    public Image face;
    public Sprite [] allFace;
    public int annoyedQuestionsCount = 0;

    //variables to progress through a conversation
    public Dialogue[] convo; //array of dialogue = conversation
    int convoIndex = 0;
    public bool convoStarted = false;

    public Dialogue[] firstDayBookIntroConversation;

    //Question Manager handles one character at a time
    //Press door for character to spawn and walk up to the desk
    //Character says intro text
    //Ask questions
    //Mark in the book and click the correct potion
    //Once correct potion is clicked, UI will pop up showing discovered charges, voids, and bipolars
    //Button is pressed to advance to the next level
    // Start is called before the first frame update
    void Start()
    {
        currentDay = GameManager.Instance.currentDay;
        currentChar = GameManager.Instance.currentCustomerIndex;

        //convo = 

        list = GameObject.Find("TempManager").GetComponent<JSONManager>().list;
        //characters = new List<NPC>();

        //spawn in all characters, enable only the first one
        /*for (int i = 0; i < characterPrefabs.Count; i++)
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
        }*/

        //currentCharacter = characters[currentChar];
        //name.text = "Name: " + currentCharacter.charName;
        //ChangeCharacter(characters[currentChar]);

        //if its day 1, set up the starting conversation
        convo = firstDayBookIntroConversation;

    }

    public void SpawnInCurrentCharacter()
    {
        //TODO: support more than just one day
        GameObject prefab = GameManager.Instance.dayOneCharacters[currentChar];
        GameObject character = Instantiate(prefab, spawnLocation.transform.position, Quaternion.identity);

        currentCharacter = character.GetComponent<NPC>();
        //currentCharacter.LoadCharacterInfo();

        //name.text = "Vivian";//"Name: " + currentCharacter.characterInfo.name;
        //t.text = "Hello, welcome to [APOTHECARY NAME]!";

        convo = currentCharacter.introConversation;

        nextButton.gameObject.SetActive(false);

        UpdatePatience();

        //enable the conversation box
        conversationBox.SetActive(true);
        GoThroughConversation();
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
        //t.text = list.characters[currentChar].intro;
        t.text = currentCharacter.characterInfo.intro;
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

        convo = currentCharacter.characterInfo.genericConvo[index].dialogue;
        convoIndex = 0;

        UpdatePatience();
    }

    //called by buttons to ask emotion-specific questions
    public void AskEmotionQuestion(int index)
    {
        t.text = currentCharacter.SpecificResponse(index);

        emotionQuestions[index].gameObject.SetActive(false);

        convo = currentCharacter.characterInfo.specificConvo[index].dialogue;
        convoIndex = 0;

        //reveal emotional info

        //loop through all the emotional info, seeing if the indices match
        for (int i = 0; i < currentCharacter.GetComponent<NPC>().potionPossibilities.Count; i++)
        {
            for(int a = 0; a < GameManager.Instance.emotionalIndexes[index].Length; a++)
            {

                for(int p = 0; p < currentCharacter.GetComponent<NPC>().potionPossibilities[i].emotionIndices.Length; p++)
                {
                    if (currentCharacter.GetComponent<NPC>().potionPossibilities[i].emotionIndices[p] == GameManager.Instance.emotionalIndexes[index][a])
                    {
                        //set the proper "discovered" bool on the NPC script
                        currentCharacter.GetComponent<NPC>().hasBeenDiscovered[p] = true;
                        amountOfDiscovered++;
                    }
                }
            }
        }


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
        //patience.text = "Patience: " + currentCharacter.Patience.ToString();
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

    //increase convo index and display new text
    //once the end of the convo is reached, display questions again
    public void GoThroughConversation()
    {

        /*if(convo != null && convoIndex < convo.Length - 1)
        {

        }*/

        Debug.Log("why");

        //if there is a convo to display, and we arent at the end
        if (convo != null && convoIndex < convo.Length - 1)
        {
            //start the conversation with vivian's first response
            if(!convoStarted)
            {
                convoStarted = true;
                convoIndex = 0;
                name.text = convo[convoIndex].character.ToString();
                t.text = convo[convoIndex].text;
            }
            //continue the conversation till the end
            else
            {
                convoIndex++;
                t.text = convo[convoIndex].text;

                name.text = convo[convoIndex].character.ToString();

            }
        }
        //either there is no convo to display (showing intro text, or none exists, etc.) or we reached the end of it
        else
        {
            if(currentCharacter != null)
            {
                ShowScrollBar();
                name.text = currentCharacter.characterInfo.name;
                convoIndex = 0;
                convo = null;
                convoStarted = false;
            }
            else
            {
                conversationBox.SetActive(false);
                convoIndex = 0;
                convo = null;
                convoStarted = false;
            }
        }
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

        if(currentCharacter.patience <= 0)
        {
            conversationBox.SetActive(false);
        }

    }


}
