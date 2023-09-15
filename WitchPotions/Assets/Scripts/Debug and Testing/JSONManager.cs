using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public struct TestCharacter
{
    public string emotion;
    public string intro;
    public string[] responses;
}

[System.Serializable]
public class CharacterList
{
    public List<TestCharacter> characters = new List<TestCharacter>();
}

public class JSONManager : MonoBehaviour
{

    public CharacterList list;

    // Start is called before the first frame update
    void Awake()
    {
        list = JsonUtility.FromJson<CharacterList>(File.ReadAllText("CharacterInfo.json"));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
