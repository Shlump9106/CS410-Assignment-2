using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Whispers : MonoBehaviour
{
    public Transform player;
    public AudioSource whispers; 
    bool mirrorLooking = false;
    public ParticleSystem blood;

    void Update()
    {
        if(this.tag == "Mirror"){
        Vector3 mirrorDirection = player.position-transform.position;
        Ray mirrorRay = new Ray(transform.position, mirrorDirection);
        RaycastHit mirrorRayHit;
        if (Physics.Raycast (mirrorRay, out mirrorRayHit))
        {
            if (mirrorRayHit.collider.transform == player)
            {
                float distance = Vector3.Distance (transform.position, player.transform.position);
                float dot = Vector3.Dot(Vector3.Normalize(player.position-transform.position+Vector3.up), player.forward);
                //Debug.Log(dot);
                if(distance < 3 && dot < -0.94 && mirrorLooking == false)
                {
                    
                    blood.Play();
                    whispers.Play();
                    mirrorLooking = true;
                }
                if(distance > 3 || dot > -0.9)
                {
                    blood.Stop();
                    whispers.Stop();
                    mirrorLooking = false;
                }

                
            }
        }
        }
        
    }
    
}
