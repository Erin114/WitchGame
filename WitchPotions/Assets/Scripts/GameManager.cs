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
    public string[] responses;
    public string[] specificResponses;
}

[System.Serializable]
public class CharacterList
{
    public List<TestCharacter> characters = new List<TestCharacter>();
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
    [SerializeField]
    private float suspicion = 100;

    public GameStates currentState;

    [SerializeField]
    NPC currentCharacter;

    // Start is called before the first frame update
    void Start()
    {

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
        //go to potions scene
        ChangeScene("PotionBrewing");

        //get reference to the potionBehavior script somehow


    }



}
