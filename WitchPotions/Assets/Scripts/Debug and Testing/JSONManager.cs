using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class JSONManager : MonoBehaviour
{

    public CharacterList list;
    public TextAsset t;

    // Start is called before the first frame update
    void Awake()
    {
        //list = JsonUtility.FromJson<CharacterList>(File.ReadAllText("CharacterInfo.json"));
        list = JsonUtility.FromJson<CharacterList>(t.text);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
