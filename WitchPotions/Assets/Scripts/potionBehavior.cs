using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class potionBehavior : MonoBehaviour
{
    Vector3 center;
    // Start is called before the first frame update
    void Start()
    {
        center = gameObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Move(float x, float y)
    {
        Transform t = gameObject.transform;
        t.Translate(new Vector3(x, y, 0) + t.position);
        gameObject.transform.position = t.position;
    }

    public void Rotate(float deg)
    {
        gameObject.transform.RotateAround(center,Vector3.forward, deg);
    }
}
