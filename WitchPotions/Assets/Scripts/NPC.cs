using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    [SerializeField]
    public string charName;
    public int ID;
    public float maxPatience = 100;
    public float patience;

    //public int[] emotionalIndicees;
    //public Level_SO.NodeTypes[] nodes;

    public bool[] hasBeenDiscovered;

    [SerializeField]
    public TestCharacter characterInfo;

    //ntch
    public List<int> genericQuestionSpriteOrder;
    public List<int> specificQuestionSpriteOrder;

    public List<Sprite> spriteVariants;

    private CharacterList list;

    public List<Level_SO> potionPossibilities;

    public Dialogue[] introConversation;

    public Sprite [] iconFaces;

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
        patience = maxPatience;
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
        if (characterInfo.genericConvo.Length > index)
        {
            patience -= 25f;

            ChangeSprite(spriteVariants, genericQuestionSpriteOrder, index);

            return characterInfo.genericConvo[index].initialResponse;
        }
        else
        {
            Debug.Log("Generic response index is out of bounds" + "|" + characterInfo.genericConvo.Length);
            return "";
        }
    }

    //return the emotion-specific response at a given index, decrease patience

    public string SpecificResponse(int index)
    {
        if (characterInfo.specificConvo.Length > index)
        {
            patience -= 50f;

            ChangeSprite(spriteVariants, specificQuestionSpriteOrder, index);

            return characterInfo.specificConvo[index].initialResponse;
        }
        else
        {
            Debug.Log("Specific response index is out of bounds");
            return "";
        }
    }

    public void ChangeSprite(List<Sprite> sprites, List<int> order, int index)
    {
        if(index < order.Count)
        {
            GetComponent<SpriteRenderer>().sprite = spriteVariants[order[index]];
        }
    }
    public Sprite GetNPCiconsGeneric ( int index)
    {
        return iconFaces[genericQuestionSpriteOrder[index]];
    }
    public Sprite GetNPCiconsSpesific(int index)
    {
        return iconFaces[specificQuestionSpriteOrder[index]];
    }

    public void getCurrentImage()
    {

    }

    //reset NPC info
    public void Reset()
    {
        patience = 100;
    }

}
