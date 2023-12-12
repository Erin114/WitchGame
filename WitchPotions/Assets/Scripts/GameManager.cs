using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public enum Character
{
    Vivian,
    Grimoire,
    John_Johnson,
    Pippin_Pobblestone,
    Mabel,
    Agnes
}

[System.Serializable]
public struct TestCharacter
{
    public int ID;
    public string name;
    public string emotion;
    public string intro;
    //public string[] responses;
    //public string[] specificResponses;
    public Conversation[] genericConvo;
    public Conversation[] specificConvo;
}

[System.Serializable]
public class CharacterList
{
    public List<TestCharacter> characters = new List<TestCharacter>();
}

[System.Serializable]
//class that is used for asking questions. When a question button is pressed, it checks against a character's conversation objects
//A.K.A genericConvo and specificConvo
public class Conversation
{
    public string question;
    public string initialResponse;
    public Dialogue[] dialogue;
}

[System.Serializable]
public struct Dialogue
{
    public Character character;
    public string text;
}

public enum GameStates
{
    BeforeCustomer, //in the scene, before a character walks through the door
    Questioning, //when your in the questioning/deduction phase
    PotionBrewing, //when your brewing
    NextCustomer //when brewing phase is over, go back to the original scene
}

public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    [SerializeField]
    private float suspicion = 100;

    [SerializeField]
    private float money = 20;

    public GameStates currentState;

    [SerializeField]
    public NPC currentCharacter;
    [SerializeField]
    public bool[] currentCharacterDiscoveredInfo;

    PotionBehavior potBehavior;

    public Level_SO[] dayOneLevels;

    public Level_SO currentLevel;

    public GameObject[] dayOneCharacters;

    [SerializeField]
    public int currentDay;

    [SerializeField]
    public int currentCustomerIndex;

    public List<int[]> emotionalIndexes;

    public static GameManager Instance
    {
        get
        {
            return instance;
        }
    }

    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        emotionalIndexes = new List<int[]>();

        int[] terror = {1,2,3,4,5,6,7,8,9,10};
        emotionalIndexes.Add(terror);

        int[] grief = {181,182,183,184,186,187,188,189,190};
        emotionalIndexes.Add(grief);

        int[] Vigilance = {91,92,93,94,95,96,97,98,99,100};
        emotionalIndexes.Add(Vigilance);

        int[] Loathing = {151,152,153,154,155,156,157,158,159,160};
        emotionalIndexes.Add(Loathing);

        int[] Rage = {121,122,123,124,125,126,127,128,129,130};
        emotionalIndexes.Add(Rage);

        int[] Joy = { 61, 62, 63, 64, 65, 66, 67, 68, 69, 70};
        emotionalIndexes.Add(Joy);

        int[] Amazement = {211,212,213,214,215,216,217,218,219,220};
        emotionalIndexes.Add(Amazement);

        int[] Admiration = {31,32,33,34,35,36,37,38,39,40};
        emotionalIndexes.Add(Admiration);

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void NextDay()
    {
        if(currentDay < 5)
        {
            currentDay++;
        }
    }
    public float Suspicion
    {
        get
        {
            return suspicion;
        }

        set
        {
            if (value > 100)
            {
                suspicion = 100;
            }
            else if (value < 0)
            {
                suspicion = 0;
            }
            else
            {
                suspicion = value;
            }
        }
    }

    public float Money
    {
        get
        {
            return money;
        }

        set
        {
            //if (value < 0)
            //{
                //money = 0;
            //}
            //else
            //{
                money = value;
            //}
        }
    }

    /*public GameObject[,] Characters
    {
        get
        {
            return characters;
        }
    }*/

    public void ChangeSuspicion(float value)
    {
        suspicion += value;

        if (suspicion > 100)
        {
            suspicion = 100;
        }
        else if (suspicion < 0)
        {
            suspicion = 0;
        }

    }

    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void UpdateFSM()
    {
        switch(currentState)
        {
            case GameStates.BeforeCustomer:

                break;


            case GameStates.Questioning:

                break;


            case GameStates.PotionBrewing:

                break;


            case GameStates.NextCustomer:

                break;
        }
    }

    public void SwitchToPotionScene(Level_SO levelObj, bool[] discoveredEmotionalInfo)
    {
        //set the current level
        currentLevel = levelObj;

        //go to potions scene
        ChangeScene("PotionBrewingScene");
    }

    public void NextCustomer()
    {

    }

    public void JohnJohnson()
    {
        currentCharacterDiscoveredInfo = GameObject.Find("TempManager").GetComponent<QuestionManager>().currentCharacter.hasBeenDiscovered;

        SwitchToPotionScene(dayOneLevels[0], currentCharacterDiscoveredInfo);
    }

    public void SendInfoToPotionScene(Level_SO level, bool[] discoveredInfo)
    {
        currentCharacterDiscoveredInfo = discoveredInfo;
        SwitchToPotionScene(level, currentCharacterDiscoveredInfo);
    }

    public void ProgressDay()
    {
        currentCustomerIndex = 0;



        //currentCharacter = characters[currentDay, currentCustomerIndex].GetComponent<NPC>();


    }


}
