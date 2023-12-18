
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/CharcterSpriteSO", order = 3)]
public class CharcterSpriteSO : ScriptableObject
{
    public List<Sprite> FullSprites;
    public List<Sprite> IconSprites;

}