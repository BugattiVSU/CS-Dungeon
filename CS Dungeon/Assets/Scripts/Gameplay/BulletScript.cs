using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour{

    public float velX = 5f;
    public float velY = 0f;
    Rigidbody2D rb;
    public float distance;
    public LayerMask isLayer;
    

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Invoke("DestroyBullet",1.5f);
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit2D ray = Physics2D.Raycast(transform.position, transform.right, distance, isLayer);
        if(ray.collider != null)
        {
            DestroyBullet();
        }
        rb.velocity = new Vector2(velX,velY);   
    }
   

    void DestroyBullet()
    {
        Destroy(gameObject);
    }




}
