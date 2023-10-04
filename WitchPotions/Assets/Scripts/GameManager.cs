using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private float suspicion = 100;

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
            if(value > 100)
            {
                suspicion = 100;
            }
            else if(value < 0)
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

}
