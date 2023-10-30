using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.IO;

public class CharacterJSONFormatter : MonoBehaviour
{
    public TMP_InputField nameIF;
    public TMP_InputField uniqueIDIF;
    public TMP_InputField introIF;
    public List<TMP_InputField> genericQuestionIFs;
    public List<TMP_InputField> specificQuestionIFs;

    CharacterList list;

    // Start is called before the first frame update
    void Start()
    {
        list = GameObject.Find("Manager").GetComponent<JSONManager>().list;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //validate all the inputs for the new character
    public void ValidateInputs()
    {

    }

    //save to new JSON file
    public void SaveJSON()
    {
        //create and add the new character to the list
        TestCharacter newCharacter = new TestCharacter();
        newCharacter.ID = int.Parse(uniqueIDIF.text);
        newCharacter.name = nameIF.text;

        newCharacter.intro = introIF.text;
        newCharacter.emotion = "Happy"; //default happy for now

        string[] generic = new string[genericQuestionIFs.Count];
        string[] specific = new string[specificQuestionIFs.Count];

        //save generic responses
        for(int i = 0; i < genericQuestionIFs.Count; i++)
        {
            generic[i] = genericQuestionIFs[i].text;
        }

        //save specific responses
        for(int i = 0; i < specificQuestionIFs.Count; i++)
        {
            specific[i] = specificQuestionIFs[i].text;
        }

        //newCharacter.responses = generic;
        //newCharacter.specificResponses = specific;

        list.characters.Add(newCharacter);

        File.WriteAllText("CharacterJSON.json", JsonUtility.ToJson(list));
        Debug.Log(JsonUtility.ToJson(list));

    }

}
