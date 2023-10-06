using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "ScriptableObjects/LevelScriptableObject", order = 1)]
public class Level_SO : ScriptableObject
{
    public enum NodeTypes
    {
        voidNode,
        bipolar,
        charger
    }

    //indices for emotions and endpoints are 0 = center, 1-10 is fear,
    //11-20 is between fear and admiration, 21-30 is admiration, etc.
    [SerializeField] int[] emotionIndices;
    [SerializeField] NodeTypes[] nodeType;
    [SerializeField] int endpoint_Index;
    [SerializeField] int coin_Reward;
    public int Endpoint_Index { get => endpoint_Index; }
    public (int[] emotionIndex, NodeTypes[] type) Special_Nodes_List
    {
        get
        {
            (int[] emotionIndex, NodeTypes[] type) special_Nodes_List;
            special_Nodes_List.emotionIndex = emotionIndices;
            special_Nodes_List.type = nodeType;

            return special_Nodes_List;
        }
    }
}
