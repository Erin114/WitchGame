using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    [SerializeField]
    public string charName;
    public int ID;
    public float patience = 100;

    [SerializeField]
    protected TestCharacter characterInfo;

    private CharacterList list;

    // Start is called before the first frame update
    protected virtual void Start()
    {

    }

    // Update is called once per frame
    protected virtual void Update()
    {
        
    }

    //load in this character's information
    public void LoadCharacterInfo()
    {
        list = GameObject.Find("TempManager").GetComponent<JSONManager>().list;

        for (int i = 0; i < list.characters.Count; i++)
        {
            if (list.characters[i].ID == ID)
            {
                characterInfo = list.characters[i];
                break;
            }
        }

        charName = characterInfo.name;
    }

    public float Patience
    {
        get
        {
            return patience;
        }
    }

    public string Intro
    {
        get
        {
            return characterInfo.intro;
        }
    }

    public string Emotion
    {
        get
        {
            return characterInfo.emotion;
        }
    }

    //return the generic response at a given index, decrease patience
    public string GenericResponse(int index)
    {
        if (characterInfo.responses.Length > index)
        {
            patience -= 25f;
            return characterInfo.responses[index];
        }
        else
        {
            Debug.Log("Generic response index is out of bounds" + "|" + characterInfo.responses.Length);
            return "";
        }
    }

    //return the emotion-specific response at a given index, decrease patience

    public string SpecificResponse(int index)
    {
        if (characterInfo.specificResponses.Length > index)
        {
            patience -= 50f;
            return characterInfo.specificResponses[index];
        }
        else
        {
            Debug.Log("Specific response index is out of bounds");
            return "";
        }
    }

    //reset NPC info
    public void Reset()
    {
        patience = 100;
    }

}
