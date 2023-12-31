using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
    public GameObject nextButton;

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
    public GameObject emotionInfoTextPopUp;

    //Added by Elad 10/10/2023 - UI and Patiance Bar
    public Slider bar;
    public Image face;
    public Sprite [] allFace;
    public int annoyedQuestionsCount = 0;

    public GameObject patienceDecrementText;

    //variables to progress through a conversation
    public Dialogue[] convo; //array of dialogue = conversation
    int convoIndex = 0;
    public bool convoStarted = false;

    public Dialogue[] firstDayBookIntroConversation;
    bool isFirstInteraction;

    public Dialogue[] conclusionConversation;
    bool startedConclusionConvo = false;

    public GameObject doorArrow;

    public Image charIcon;
    private Sprite charIconReferenceHolder;
    public Sprite VivianIcon;
    public Sprite [] VivianIconOptions;

    public Sprite GrimoireIcon;
    private int currentQuestionIndex;
    private bool isCurrentQuestionGeneric;
    private bool spacialConvo = true;
    private bool isIntro = false;

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

        //if we just served a customer a potion, spawn them in and set up the outro/goodbye conversation
        if(GameManager.Instance.servedPotion)
        {
            SpawnInCharacterForOutro();
            GameManager.Instance.currentCustomerIndex++; //update the current customer index
        }

        //get the current customer from Game manager
        currentDay = GameManager.Instance.currentDay;
        currentChar = GameManager.Instance.currentCustomerIndex;

        //if its the first day, set up the tutorial arrows/objects
        if (currentDay == 0 && currentChar == 0)
        {
            //if its day 1, set up the starting conversation
            convo = firstDayBookIntroConversation;
            spacialConvo = true;
            isIntro = true;
        }

    }

    public void SpawnInCurrentCharacter()
    {
        if(currentChar < GameManager.Instance.dayOneCharacters.Length && convo == null)
        {
            //TODO: support more than just one day
            GameObject prefab = GameManager.Instance.dayOneCharacters[currentChar];
            GameObject character = Instantiate(prefab, spawnLocation.transform.position, Quaternion.identity);

            currentCharacter = character.GetComponent<NPC>();
            //currentCharacter.LoadCharacterInfo();

            //name.text = "Vivian";//"Name: " + currentCharacter.characterInfo.name;
            //t.text = "Hello, welcome to [APOTHECARY NAME]!";

            convo = currentCharacter.introConversation;
            spacialConvo = true;
            charIconReferenceHolder = charIcon.sprite = currentCharacter.iconFaces[0];
            isIntro = true;
            nextButton.SetActive(false);

            UpdatePatience(0);

            //update the generic and specific questions
            for(int g = 0; g < genericQuestions.Count; g++)
            {
                genericQuestions[g].gameObject.GetComponentInChildren<Text>().text = currentCharacter.GetComponent<NPC>().characterInfo.genericConvo[g].question;
            }

            for (int s = 0; s < emotionQuestions.Count; s++)
            {               
                emotionQuestions[s].gameObject.GetComponentInChildren<Text>().text = currentCharacter.GetComponent<NPC>().characterInfo.specificConvo[s].question;
            }

            //enable the conversation box
            conversationBox.SetActive(true);
            GoThroughConversation();
        }

        //if we reached the end of the day 1 characters, and there is currently no conversation happening
        //set up the conclusion conversation
        else if(currentChar >= GameManager.Instance.dayOneCharacters.Length && convo == null)
        {
            convo = conclusionConversation;
            conversationBox.SetActive(true);
            nextButton.SetActive(false);
            t.text = "...";
            startedConclusionConvo = true;
        }
    }

    //spawns in the character object and sets up the outro/goodbye dialogue
    public void SpawnInCharacterForOutro()
    {
        currentChar = GameManager.Instance.currentCustomerIndex;

        if (currentChar < GameManager.Instance.dayOneCharacters.Length)
        {
            //TODO: support more than just one day
            GameObject prefab = GameManager.Instance.dayOneCharacters[currentChar];
            GameObject character = Instantiate(prefab, spawnLocation.transform.position, Quaternion.identity);

            currentCharacter = character.GetComponent<NPC>();

            convo = currentCharacter.exitConversation;
            spacialConvo = true;
            charIconReferenceHolder = currentCharacter.iconFaces[0];

            //GameManager.Instance.servedPotion = false;
        }
    }

    public void ChangeCharacter(NPC character)
    {
        //NextCustomer();

        currentCharacter = character;
        annoyedQuestionsCount = 0;
        UpdatePatience(0);

    }

    public void ShowIntroText()
    {
        list = GameObject.Find("TempManager").GetComponent<JSONManager>().list;
        //t.text = list.characters[currentChar].intro;
    
        t.text = currentCharacter.characterInfo.intro;
        charIconReferenceHolder = charIcon.sprite = currentCharacter.iconFaces[0];
        introButton.gameObject.SetActive(false);
      
        //show all generic questions and emotion specific questions
        for (int i = 0; i < genericQuestions.Count; i++)
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
        spacialConvo = false;
        t.text = currentCharacter.GenericResponse(index);
      
        genericQuestions[index].gameObject.SetActive(false);

        isCurrentQuestionGeneric = true;
        convo = currentCharacter.characterInfo.genericConvo[index].dialogue;
        convoIndex = 0;
        currentQuestionIndex = index;
        int imageIndexindex = currentCharacter.genericQuestionSprites[currentQuestionIndex].SpriteIndex[0];
        charIcon.sprite = currentCharacter.CharSprites.IconSprites[imageIndexindex];
        VivianIcon = currentCharacter.vivianSprites.IconSprites[imageIndexindex];
        currentCharacter.gameObject.GetComponent<SpriteRenderer>().sprite = currentCharacter.CharSprites.FullSprites[imageIndexindex];
        charIcon.gameObject.SetActive(true);
        name.gameObject.SetActive(true);
        UpdatePatience(10);
    }

    //called by buttons to ask emotion-specific questions
    public void AskEmotionQuestion(int index)
    {
        spacialConvo = false;
        t.text = currentCharacter.SpecificResponse(index);
    

        emotionQuestions[index].gameObject.SetActive(false);
        
        currentQuestionIndex = index;
        isCurrentQuestionGeneric = false;
        convo = currentCharacter.characterInfo.specificConvo[index].dialogue;
        convoIndex = 0;
        int imageIndexindex = currentCharacter.specificQuestionSprites[currentQuestionIndex].SpriteIndex[0];
        charIcon.sprite = currentCharacter.CharSprites.IconSprites[imageIndexindex];
        VivianIcon = currentCharacter.vivianSprites.IconSprites[imageIndexindex];
        currentCharacter.gameObject.GetComponent<SpriteRenderer>().sprite = currentCharacter.CharSprites.FullSprites[imageIndexindex];
        charIcon.gameObject.SetActive(true);
        name.gameObject.SetActive(true);
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
                        //increase amount discovered
                        if(!currentCharacter.GetComponent<NPC>().hasBeenDiscovered[p])
                        {
                            amountOfDiscovered++;
                        }

                        //set the proper "discovered" bool on the NPC script
                        currentCharacter.GetComponent<NPC>().hasBeenDiscovered[p] = true;
                        emotionInfoTextPopUp.GetComponent<TMP_Text>().text = "New " + currentCharacter.GetComponent<NPC>().potionPossibilities[i].nodeType[p] + " discovered!";
                        emotionInfoTextPopUp.GetComponent<Animator>().SetTrigger("Reset");
                        //GameObject.Find("Book").GetComponent<BookUI>().RevealInfoInBook(currentCharacter.GetComponent<NPC>().potionPossibilities[i].emotionIndices[p],
                                                                                        //currentCharacter.GetComponent<NPC>().potionPossibilities[i].nodeType[p]);
                        GameObject.Find("BookImage").GetComponent<Book>().discoveredIndexes.Add(currentCharacter.GetComponent<NPC>().potionPossibilities[i].emotionIndices[p]);
                        GameObject.Find("BookImage").GetComponent<Book>().discoveredNodes.Add(currentCharacter.GetComponent<NPC>().potionPossibilities[i].nodeType[p]);
                        
                    }
                }
            }
        }

        //hard-coded patience value for UI
        UpdatePatience(20);
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

        //UpdatePatience();

    }

    void UpdatePatience(float value)
    {

        //play animations
        if (value != 0)
        {
            bar.GetComponent<Animator>().SetTrigger("Update");

            patienceDecrementText.GetComponent<TMP_Text>().text = "-" + value;
            patienceDecrementText.GetComponent<Animator>().SetTrigger("Play");
        }

        patience.text = currentCharacter.patience + "/" + currentCharacter.maxPatience;

        float currentPatienceRatio = (currentCharacter.patience / currentCharacter.maxPatience);

        //patience.text = "Patience: " + currentCharacter.Patience.ToString();
        if (currentCharacter.Patience >0)
        {
            bar.value = (currentCharacter.Patience / currentCharacter.maxPatience) * 100;
        }
        else
        {
            bar.value = 0;
            annoyedQuestionsCount++;
            currentPatienceRatio = -1;
        }
        if (currentPatienceRatio > 0.75)
        {
            face.sprite = allFace[0];
        }
        else if (currentPatienceRatio > 0.50)
        {
            face.sprite = allFace[1];
        }
        else if (currentPatienceRatio > 0.25)
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

        //Debug.Log("why");

        //if there is a convo to display, and we arent at the end
        if (convo != null && convoIndex < convo.Length - 1)
        {
            charIcon.gameObject.SetActive(true);
            name.gameObject.SetActive(true);
            //start the conversation with vivian's first response
            if (!convoStarted)
            {
                convoStarted = true;
                convoIndex = 0;
                name.text = convo[convoIndex].character.ToString().Replace('_',' ');
                t.text = convo[convoIndex].text;
            }
            //continue the conversation till the end
            else
            {
                t.text = convo[convoIndex].text;

                name.text = convo[convoIndex].character.ToString().Replace('_', ' ');

            }

            //Start of new icon and image switch code
            if (isCurrentQuestionGeneric && currentCharacter!=null)
            {  
                if (convoStarted)
                    convoIndex++;
                int imageIndexindex = currentCharacter.genericQuestionSprites[currentQuestionIndex].SpriteIndex[convoIndex];
             


                if (name.text ==   "Grimoire")
                {
                    charIcon.sprite = GrimoireIcon;

                }
                else if (name.text == "Vivian")
                {
                    charIcon.sprite = currentCharacter.vivianSprites.IconSprites[imageIndexindex];
                }
                else
                {
                    charIcon.sprite = currentCharacter.CharSprites.IconSprites[imageIndexindex];
                    currentCharacter.gameObject.GetComponent<SpriteRenderer>().sprite = currentCharacter.CharSprites.FullSprites[imageIndexindex];
                }
            }else if (currentCharacter != null  && !spacialConvo)
            { 
                if (convoStarted)
                    convoIndex++;
                int imageIndexindex = currentCharacter.specificQuestionSprites[currentQuestionIndex].SpriteIndex[convoIndex];

               

                if (name.text == "Grimoire")
                {
                    charIcon.sprite = GrimoireIcon;

                }
                else if (name.text == "Vivian")
                {
                    charIcon.sprite = currentCharacter.vivianSprites.IconSprites[imageIndexindex];
                }
                else
                {
                    charIcon.sprite = currentCharacter.CharSprites.IconSprites[imageIndexindex];
                    currentCharacter.gameObject.GetComponent<SpriteRenderer>().sprite = currentCharacter.CharSprites.FullSprites[imageIndexindex];
                }
            }
            else if (spacialConvo)
            {

                if (convoStarted)
                    convoIndex++;

                int imageIndexindex = 0;
                if(isIntro&&currentCharacter!=null)
                imageIndexindex = currentCharacter.spriteOrderForIntro[convoIndex];
            
                if (name.text == "Grimoire")
                {
                    charIcon.sprite = GrimoireIcon;

                }
                else if (name.text == "Vivian")
                {
                    charIcon.sprite = currentCharacter.vivianSprites.IconSprites[imageIndexindex];
                }
                else
                {
                    charIcon.sprite = currentCharacter.CharSprites.IconSprites[imageIndexindex];
                    currentCharacter.gameObject.GetComponent<SpriteRenderer>().sprite = currentCharacter.CharSprites.FullSprites[imageIndexindex];
                }
            }
            else
            {
                if (convoStarted)
                    convoIndex++;
                if (name.text == "Grimoire")
                {
                    charIcon.sprite = GrimoireIcon;

                }
                else if (name.text == "Vivian")
                {
                    charIcon.sprite = currentCharacter.vivianSprites.IconSprites[0];
                }
            }
        
        }
        
        //either there is no convo to display (showing intro text, or none exists, etc.) or we reached the end of it
        else
        {
            if(currentCharacter != null && !GameManager.Instance.servedPotion)
            {
                ShowScrollBar();
                name.text = currentCharacter.characterInfo.name;
                convoIndex = 0;
                convo = null;
                convoStarted = false;
                isIntro = false;
            }
            else
            {
                conversationBox.SetActive(false);
                convoIndex = 0;                
                convoStarted = false;

                //if its the end of that first intro conversation with the book
                //(the one that tells you to open to door)
                //display the arrow showing where the door is
                if(convo == firstDayBookIntroConversation)
                {
                    doorArrow.SetActive(true);
                }

                convo = null;
                
                //if this code runs, despawn the current character
                if(GameManager.Instance.servedPotion)
                {
                    Destroy(currentCharacter.gameObject);
                    GameManager.Instance.servedPotion = false;
                }

            }
            charIcon.gameObject.SetActive(false);
            name.gameObject.SetActive(false);

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
