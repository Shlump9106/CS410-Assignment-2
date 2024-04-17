using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Security.Permissions;
using UnityEngine;

public class GargoyleObserver : MonoBehaviour
{

    public Transform player;
    public AudioSource detectAudio;
    public float detectionDelay = .5f;
    public float loseDetectionDelay = -5f;
    public float coneOfVisionAngle = 60f;
    public float coneOfVisionRadius = 10f;

    public ParticleSystem fire;

    private float detectionTimer = 0f;
    private float loseDetectionTimer = 0f;

    bool m_IsPlayerSeen;
    bool areGhostsChasing = false;

    void Seek(Vector3 seekerForward, Vector3 seekerPosition, Vector3 hiderPosition)
    {
        Vector3 toHider = hiderPosition - seekerPosition;
        float angleSeekerHider = Vector3.Angle(seekerForward, toHider);

        if (angleSeekerHider < coneOfVisionAngle * 0.5f && toHider.magnitude < coneOfVisionRadius)
        {
            Vector3 direction = hiderPosition - seekerPosition + Vector3.up;
            Ray ray = new Ray(seekerPosition, direction);
            RaycastHit raycastHit;
            if (Physics.Raycast(ray, out raycastHit))
            {
                if (raycastHit.collider.transform == player)
                {
                    m_IsPlayerSeen = true;
                }
                else
                {
                    m_IsPlayerSeen = false;
                }
            }
        }
        else
        {
            m_IsPlayerSeen = false;
        }
    }

    void ModifyGhostChasing(bool areGhostsChasing)
    {
        GameObject[] ghosts = GameObject.FindGameObjectsWithTag("Ghost");
        foreach (GameObject ghost in ghosts)
        {
            GhostObserver observer = ghost.GetComponentInChildren<GhostObserver>();
            if (observer != null)
            {
                observer.areGhostsChasing = areGhostsChasing;
            }
        }
    }

    void Update()
    {
        Vector3 gargoyleForward = transform.forward;
        Vector3 toPlayer = player.position - transform.position;

        Seek(gargoyleForward, transform.position, player.position);

        if (m_IsPlayerSeen)
        {
            if (detectionTimer <= detectionDelay)
            {
                detectionTimer += Time.deltaTime;
            }
            if (loseDetectionTimer <= 0)
            {
                loseDetectionTimer += Time.deltaTime;
            }
        }
        if (!m_IsPlayerSeen)
        {
            if (detectionTimer >= 0)
            {
                detectionTimer -= Time.deltaTime;
            }
            if (loseDetectionTimer >= loseDetectionDelay)
            {
                loseDetectionTimer -= Time.deltaTime;
            }
        }

        if (detectionTimer >= detectionDelay && areGhostsChasing == false)
        {
            detectAudio.Play();
            fire.Play();
            areGhostsChasing = true;
            ModifyGhostChasing(true);
        }
        if (loseDetectionTimer <= loseDetectionDelay && areGhostsChasing == true)
        {
            fire.Stop();
            areGhostsChasing = false;
            ModifyGhostChasing(false);
        }
    }
}
