using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{   
    
    public int tokenCount = 0;
    private AudioSource audio;
    public AudioClip pickUpItem;

    private void Start()
    {
        this.audio = this.gameObject.AddComponent<AudioSource>();
        this.audio.clip = this.pickUpItem;
        this.audio.loop = false;
    }
    public void PickupItem(GameObject obj)
    {
        switch (obj.tag)
        {
            case "Token":
                tokenCount++;
                break;
            
            default:
                this.audio.Play();
                break;
            
        }

        
    }
}
