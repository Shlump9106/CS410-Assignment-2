using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Security.Permissions;
using UnityEngine;
using static System.Net.Mime.MediaTypeNames;

public class GhostObserver : MonoBehaviour
{
    public Transform player;
    public AudioSource detectAudio;
    public GameEnding gameEnding;
    public float detectionDelay = 5f;
    public float coneOfVisionAngle = 60f;
    public float coneOfVisionRadius = 10f;

    private float detectionTimer = 0f;

    public bool isGhostChasing = false;
    public bool areGhostsChasing = false;

    private bool m_IsPlayerSeen;
    private bool m_IsPlayerCatchable;

    void OnTriggerEnter(Collider other)
    {
        if (other.transform == player)
        {
            m_IsPlayerCatchable = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.transform == player)
        {
            m_IsPlayerCatchable = false;
        }
    }

    private void SetSymbolVisibility(UnityEngine.UI.Image symbol, bool isVisible)
    {
        symbol.enabled = isVisible;
    }

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

    void Update()
    {
        Vector3 ghostForward = transform.forward;
        Vector3 toPlayer = player.position - transform.position;

        Seek(ghostForward, transform.position, player.position);

        if (m_IsPlayerSeen && detectionTimer <= detectionDelay)
        {
            detectionTimer += Time.deltaTime;
        }
        if (!m_IsPlayerSeen && detectionTimer >= 0)
        {
            detectionTimer -= Time.deltaTime;
        }

        if (detectionTimer >= detectionDelay && isGhostChasing == false)
        {
            detectAudio.Play();
            isGhostChasing = true;
        }
        if (detectionTimer <= 0 && isGhostChasing == true)
        {
            isGhostChasing = false;
        }

        WaypointPatrol pathFinder = transform.parent.gameObject.GetComponent<WaypointPatrol>();
        if (isGhostChasing || areGhostsChasing)
        {
            if (m_IsPlayerCatchable)
            {
                gameEnding.CaughtPlayer();
            }
            else
            {
                pathFinder.caughtPlayer = true;
            }
        }
        else
        {
            pathFinder.caughtPlayer = false;
        }
    }
}
