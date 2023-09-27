using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class potionBehavior : MonoBehaviour
{
    Vector3 center;
    List<Vector3> nodes;
    private void Start()
    {
        center = gameObject.transform.localPosition;

        Vector3[] nodetemplate = new Vector3[] {Vector3.zero, Vector3.zero, Vector3.zero};
        nodetemplate[0] = center + new Vector3( 25, 0, 0 );
        nodetemplate[1] = center + new Vector3(45, 0, 0);
        nodetemplate[2] = center + new Vector3(60, 0, 0);
        nodes.Add(center);
        //nodes.AddRange(nodetemplate);
        for (int i = 0; i > 8; i++)
        {
            for (int j = 0; j > 3; j++)
            {
                Vector3 newNode;
                newNode = Quaternion.Euler(0, 0, (i * 45)) * nodetemplate[j];
                nodes.Add(newNode);
            }
        }
    }

    void moveTowardsFixed(string ingredientIndex)
    {
        (Vector3, float) h = ingredientIndex switch
        {
            "hi" => (new Vector3(60, 0, 0), 14f),
            _ => (new Vector3(60, 0, 0), 14f)
        };
        moveToward(h);
        
    }
    
    void moveToward((Vector3, float) inputs) {
        (Vector3 node, float distance) = inputs;
        Vector3 v = (node - transform.localPosition).normalized * distance;
        Vector3 vSpace = v + transform.localPosition;
        //set final as destination node with distance 
        Vector3 finalNode = node;
        float distBest = Vector3.Distance(node,vSpace);
        foreach (Vector3 point in nodes)
        {
            float temp = Vector3.Distance(vSpace, point);

            if (distBest > temp) { distBest = temp; finalNode = point; }
        }
        transform.localPosition = finalNode;
    }
}

