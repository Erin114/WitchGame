using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

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
public class Conversation
{
    public string initialResponse;
    public string[] dialogue;
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
    private float money = 0;

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

        int[] grief = {121,122,123,124,126,127,128,129,130};
        emotionalIndexes.Add(grief);

        int[] Vigilance = {61,62,63,64,65,66,67,68,69,70};
        emotionalIndexes.Add(Vigilance);

        int[] Loathing = {101,102,103,104,105,106,107,108,109,110};
        emotionalIndexes.Add(Loathing);

        int[] Rage = {81,82,83,84,85,86,87,88,89,90};
        emotionalIndexes.Add(Rage);

        int[] Joy = {41,42,43,44,45,46,47,48,49,50};
        emotionalIndexes.Add(Joy);

        int[] Amazement = {141,142,143,144,145,146,147,148,149,150};
        emotionalIndexes.Add(Amazement);

        int[] Admiration = {21,22,23,24,25,26,27,28,29,30};
        emotionalIndexes.Add(Admiration);

    }

    // Update is called once per frame
    void Update()
    {

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
                suspicion = value;
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

    public void NextDay()
    {
        currentDay++;

        currentCustomerIndex = 0;

        //currentCharacter = characters[currentDay, currentCustomerIndex].GetComponent<NPC>();


    }


}
