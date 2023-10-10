using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Emotions
{
    Terror,
    Grief,
    Vigilance,
    Loathing,
    Rage,
    Joy,
    Amazement,
    Admiration
}

public class Potion : MonoBehaviour
{
    public List<Emotions> tags;
    public string potionName;
}
