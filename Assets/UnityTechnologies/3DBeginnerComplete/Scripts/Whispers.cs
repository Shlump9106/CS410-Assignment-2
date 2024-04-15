using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Whispers : MonoBehaviour
{
    public Transform player;
    public AudioSource whispers; 
    bool mirrorLooking = false;
    bool whispersPlaying = false;

    // Update is called once per frame
    void Update()
    {
        if(this.tag == "Mirror"){
        Vector3 directionm = transform.forward-player.position;
        Ray raym = new Ray(transform.position, directionm);
        RaycastHit raycastHitm;
        if (Physics.Raycast (raym, out raycastHitm))
        {
            if (raycastHitm.collider.transform == player)
            {
                float distance = Vector3.Distance (transform.position, player.transform.position);
                float dot = Vector3.Dot(Vector3.Normalize(player.position-transform.position+Vector3.up), player.forward);
                if(distance < 3 && dot < -0.9 && mirrorLooking == false)
                {
                    whispers.Play();
                    mirrorLooking = true;
                }
                if(distance > 3 || dot > -0.9)
                {
                    whispers.Stop();
                    mirrorLooking = false;
                }

                
            }
        } /*
        if(mirrorLooking == true && whispersPlaying == false)
        {
            Debug.Log("play");
            whispers.Play();
            whispersPlaying = true;
        }
        if(mirrorLooking == false)
        {
            Debug.Log("stop");
            whispers.Stop();
            whispersPlaying = false;
        }*/
        /*

        
        {
            mirrorLooking = false; 
        }
        
        }
        if (mirrorLooking == false && whispersPlaying == true)
        {
            whispers.Stop();
            whispersPlaying = false;
            
        }
        if (mirrorLooking == true && whispersPlaying == false )
        {
            whispers.Play();
            whispersPlaying = true;
        }*/
        }
        
    }
    
}
