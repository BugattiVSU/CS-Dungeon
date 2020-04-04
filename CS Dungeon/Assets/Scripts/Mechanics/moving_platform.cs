using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moving_platform : MonoBehaviour
{
    public float speed = 5.0f;
    public float distance = 10.0f;
    public float initialPoint;
    public float endPoint;
    public bool endReached = false;
    // Start is called before the first frame update
    void Start()
    {
        initialPoint = transform.position.x;
        endPoint = initialPoint + 10.0f;
    }

    // Update is called once per frame
    void Update()
    {
        float translation = speed * Time.deltaTime;
        
        //transform.Translate(sineTranslate, 0, 0);

        //If the x position of the platform is around the value of its starting position,
        //then endReached becomes false. If it reaches the endPoint x value, then endReached 
        //becomes true
        if(transform.position.x <= initialPoint)
        {
            endReached = false;
        }
        else if(transform.position.x >= endPoint)
        {
            endReached = true;
        }
        //if endReached is false then the platform moves to the right. If true then to the left
        if(endReached == false)
        {
            transform.Translate(translation, 0, 0);
        }
        else
        {
            transform.Translate(-translation, 0, 0);
        }
    }
}
