using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionBehavior : MonoBehaviour
{
    /*
     * OVERALL ARCHITECTURE:
     * Start() creates a radial grid of nodes, which are each points around the emotional spectrum.
     * MoveToward((Vector3, float)) moves the parent gameobject of PotionBehavior so many nodes across the 
     * grid towards a target node. AddIngredient(Ingredients_SO) interfaces with MoveToward by taking an object that 
     * provides the necessary args to call MoveTowards(), and calculate the total cost and poison value of the potion.
     * Each call of AddIngredient also calls SpecialNodeUpdate to see if the end point of your ingredient has hit
     * any special nodes such as voids (instant fail of potion), bipolar(teleports gameobject), or charger (extra value of potion).
     */
    Vector3 center;
    List<Vector3> nodes = new List<Vector3>();
    //one grid unit
    float unit;

    // array of special nodes, given the node index and node type
    (int nodeIndex, NodeTypes type)[] specials;
    //types of nodes that need to be handled by SpecialNodeUpdate
    enum NodeTypes
    {
        voidNode,
        bipolar,
        charger
    }

    //positions of each emotional extreme
    Dictionary<string, Vector3> emotionValues = new Dictionary<string, Vector3>()
    {
        {"Rage", new Vector3(-60, 0) },
        {"Terror", new Vector3(60,0) },
        {"Joy", new Vector3(0,60) },
        {"Grief", new Vector3(0,-60)},
        {"Loathing", new Vector3(-30 * Mathf.Sqrt(2),-30 * Mathf.Sqrt(2)) },
        {"Amazement", new Vector3(30 * Mathf.Sqrt(2),-30 * Mathf.Sqrt(2)) },
        {"Vigilance",new Vector3(-30 * Mathf.Sqrt(2),30 * Mathf.Sqrt(2)) },
        {"Admiration", new Vector3(30 * Mathf.Sqrt(2),30 * Mathf.Sqrt(2)) },
    };

    //relevant potion statistics
    int poison = 0;
    int cost = 0;
    int chargersHit = 0;
    
    private void Start()
    {
        center = gameObject.transform.localPosition;

        //potion position init, resolve how many rings will be in use and what the base ring units will be
        int rings = 10;
        int slices = 16;
        float theta = 360f / slices;
        Vector3[] nodetemplate = new Vector3[rings];
        unit = 60f / rings;
        for (int i = 0; i < rings; i++)
        {
            nodetemplate[i] = center + new Vector3(i * unit, 0, 0);
        }

        // 0 center, 1-10 fear, 11-20 fear/amazement, 21-30 amazement, etc. 
        
        nodes.Add(center);
        //nodes.AddRange(nodetemplate);
        //populate node graph
        for (int i = 0; i < slices; i++)
        {
            for (int j = 0; j < rings; j++)
            {
                Vector3 newNode;
                newNode = Quaternion.Euler(0, 0, (i * theta)) * nodetemplate[j];
                nodes.Add(newNode);
            }
        }
    }



   
    //ingredients 2.0
    //TODO takes IngredientObject
    /*
     * IngredientObject Properties:
     * (String, int)[] emoProp
     * int price
     * int poison
     * String name
     */
    public void AddIngredient(Ingredients_SO ingredient) 
    {
        Vector3 move1 = emotionValues[ingredient.Ingredients_Vector.emotion[0]];
        float move1Amount = ingredient.Ingredients_Vector.value[0];
        MoveToward((move1, move1Amount));
        if (ingredient.Ingredients_Vector.emotion.Length > 1)
        {
            Vector3 move2 = emotionValues[ingredient.Ingredients_Vector.emotion[1]];
            float move2Amount = ingredient.Ingredients_Vector.value[1];

            MoveToward((move2, move2Amount));
        }
        cost += ingredient.ingredients_Price;
        poison += ingredient.ingredients_Poison;
    }

    // moves toward a given endpoint by a certain distance
    void MoveToward((Vector3, float) inputs) {

        (Vector3 node, float distance) = inputs;

        if (node == transform.localPosition) return;
        Vector3 v = (node - transform.localPosition).normalized * distance;
        Vector3 vSpace = v + transform.localPosition;
        //set final as destination node with distance 
        Vector3 finalNode = node;
        float distBest = Vector3.Distance(node,vSpace);

        //calcualtes the closest node (that is NOT the current node) and locks the point's position to it
        foreach (Vector3 point in nodes)
        {
            float temp = Vector3.Distance(vSpace, point);

            if (distBest > temp && point != transform.localPosition) { distBest = temp; finalNode = point; }
        }
        transform.localPosition = finalNode;

    }

    Vector3 CreatePathEndpoint(Vector3 node, float distance)
    {
        Vector3 v = (node - transform.localPosition).normalized * distance;
        Vector3 vSpace = v + transform.localPosition;

        return vSpace;
    }


    //called at the end of movetoward to see if we landed on any special nodes
    void SpecialNodeUpdate()
    {
        for (int i = 0; i < specials.Length; i++)
        {
            if (nodes[specials[i].nodeIndex] == transform.localPosition)
            {
                switch(specials[i].type)
                {
                    case NodeTypes.voidNode:
                        break;
                    case NodeTypes.charger:
                        chargersHit++;
                        break;
                    case NodeTypes.bipolar:
                        for (int j = 0; j < specials.Length; j++)
                        {
                            if (i != j && specials[j].type == NodeTypes.bipolar)
                            {
                                transform.localPosition = nodes[specials[j].nodeIndex];
                            }
                        }
                        break;
                    default:
                        break;
                }
                return;
            }
        }

    }

    //finish brewing, probably to be called by InventoryManager or another GameManager
    //TODO by Elad
    PotionObject FinishBrew() { return new PotionObject(); }
}

