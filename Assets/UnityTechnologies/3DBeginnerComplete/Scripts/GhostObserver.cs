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
    public AudioSource ambientAudio;
    public AudioSource ambientDetectedAudio;
    public AudioSource booLaugh;
    public GameEnding gameEnding;

    public float detectionDelay = .5f;
    public float loseDetectionDelay = -5f;
    public float coneOfVisionAngle = 60f;
    public float coneOfVisionRadius = 10f;

    private float detectionTimer = 0f;
    private float loseDetectionTimer = 0f;

    public bool isGhostChasing = false;
    public bool areGhostsChasing = false;
    public bool areBennyHill = false;
    public bool isBooLaughing = false;

    private bool m_IsPlayerSeen = false;
    private bool m_IsPlayerCatchable = false;

    private WaypointPatrol pathFinder;

    void Start()
    {
        pathFinder = transform.parent.gameObject.GetComponent<WaypointPatrol>();
    }

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
                    //Debug.Log("Player seen");
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

        //Debug.Log("Detection timer: " + detectionTimer);
        //Debug.Log(m_IsPlayerSeen);
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

        if (detectionTimer >= detectionDelay && isGhostChasing == false)
        {
            //Debug.Log("isGhostChasing set");
            isGhostChasing = true;
            if (!areGhostsChasing)
            {
                detectAudio.Play();
            }
        }
        if (loseDetectionTimer <= loseDetectionDelay && isGhostChasing == true)
        {
            isGhostChasing = false;
        }

        if (isGhostChasing || areGhostsChasing)
        {
            if (!areBennyHill)
            {
                ambientAudio.Stop();
                ambientDetectedAudio.Play();
                areBennyHill = true;
            }
            if (m_IsPlayerCatchable)
            {
                if (!isBooLaughing)
                {
                    booLaugh.Play();
                    isBooLaughing = true;
                }
                gameEnding.CaughtPlayer();
            }
            else
            {
                //Debug.Log("pathfinding to player");
                pathFinder.caughtPlayer = true;
            }
        }
        else
        {
            pathFinder.caughtPlayer = false;
            if (areBennyHill)
            {
                ambientDetectedAudio.Stop();
                ambientAudio.Play();
                areBennyHill = false;
            }
        }
    }
}
