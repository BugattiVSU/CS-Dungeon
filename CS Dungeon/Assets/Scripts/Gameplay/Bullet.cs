using System.Collections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed;
    public float distance;
    public LayerMask isLayer;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("DestroyBullet", 2);
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit2D ray = Physics2D.Raycast(transform.position, transform.right, distance, isLayer);
        if(ray.collider !=null)
        {
            if(ray.collider.tag =="Enemies")
            {
                Debug.Log("Hit!");
            }
            DestroyBullet();
        }

        float hor = Input.GetAxis("Horizontal");
        
            transform.Translate(new Vector3(Mathf.Abs(hor) * Time.deltaTime, 0, 0));
        if (hor > 0)
        {
            transform.Translate(transform.right * speed * Time.deltaTime);
            transform.eulerAngles = new Vector3(0, 0, 0);

        }
        else if (hor < 0)
        {
            transform.Translate(transform.right * -1 * speed * Time.deltaTime);
            transform.eulerAngles = new Vector3(0, 180, 0);
        }

        /*
        if (transform.rotation.y == 0)
        {
           transform.Translate(transform.right * speed * Time.deltaTime);
        }
        else
        {
            transform.Translate(transform.right * -1 * speed * Time.deltaTime);
        }

        */
    }

    void DestroyBullet()
    {
        Destroy(gameObject);
    }
}
