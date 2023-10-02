using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class potionBehavior : MonoBehaviour
{
    Vector3 center;
    List<Vector3> nodes = new List<Vector3>();
    float unit;
    private void Start()
    {
        center = gameObject.transform.localPosition;

        //potion position init, resolve how many rings will be in use and what the base ring units will be
        int rings = 8;
        Vector3[] nodetemplate = new Vector3[rings];
        unit = 60f / rings;
        for (int i = 0; i < rings; i++)
        {
            nodetemplate[i] = center + new Vector3(i * unit, 0, 0);
        }
        
        nodes.Add(center);
        //nodes.AddRange(nodetemplate);
        //populate node graph
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < rings; j++)
            {
                Vector3 newNode;
                newNode = Quaternion.Euler(0, 0, (i * 45)) * nodetemplate[j];
                nodes.Add(newNode);
            }
        }
    }



    //intakes an ingredient name and calls the moveToward function with the appropriate values
   public void moveTowardsFixed(string ingredientIndex)
    {
        //ingredient switch case
        (Vector3, float) h = ingredientIndex switch
        {
            "reggie" => (new Vector3(60, 0, 0), unit * 2),
            "joy" => (new Vector3(0, 60, 0), unit * 2),
            "rage" => (new Vector3(-60, 0, 0), unit * 2),
            "grief" => (new Vector3(0,-60,0), unit * 2),
            _ => (new Vector3(60, 0, 0), 14f)
        };
        moveToward(h);
        
    }
    
    // moves toward a given endpoint by a certain distance
    void moveToward((Vector3, float) inputs) {
        (Vector3 node, float distance) = inputs;
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
}

