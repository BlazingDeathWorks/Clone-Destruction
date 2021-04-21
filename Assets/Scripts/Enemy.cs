using Pathfinding;
using UnityEngine;
using System.Collections.Generic;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private Camera mainCamera = null;
    [SerializeField]
    private int health = 1;
    [SerializeField]
    private AudioClip hitAudio = null;
    [SerializeField]
    AudioClip enemyDeathClip = null;

    EscapePod[] escapePods = new EscapePod[4];
    AIDestinationSetter destinationSetter = null;

    const string nameToLayerBullet = "Bullet";
    const string nameToLayerPlayer = "Player";

    private void Awake()
    {
        destinationSetter = GetComponent<AIDestinationSetter>();
    }

    private void Start()
    {
        escapePods = ReturnAllEscapePods();
        FindNearestEscapePod();
    }

    private EscapePod[] ReturnAllEscapePods()
    {
        return (EscapePod[])FindObjectsOfType(typeof(EscapePod));
    }

    private void FindNearestEscapePod()
    {
        EscapePod nearestEscapePod = null;
        float smallestDistance = Mathf.Infinity;
        foreach(EscapePod escapePod in escapePods)
        {
            float distance = Vector2.Distance(transform.position, escapePod.transform.position);
            if (distance < smallestDistance)
            {
                smallestDistance = distance;
                nearestEscapePod = escapePod;
            }
        }
        SetDestination(nearestEscapePod);
    }

    private void SetDestination(EscapePod escapePod)
    {
        if (escapePod == null) return;
        destinationSetter.target = escapePod.transform;
    }

    private void OnDestroy()
    {
        transform.parent.GetComponent<EnemySpawner>().AddToKillPerWave();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer(nameToLayerBullet) || collision.gameObject.layer == LayerMask.NameToLayer(nameToLayerPlayer))
        {
            health--;
            if(hitAudio != null)
            {
                AudioSource.PlayClipAtPoint(hitAudio, mainCamera.transform.position);
            }
            if (enemyDeathClip != null)
            {
                AudioSource.PlayClipAtPoint(enemyDeathClip, mainCamera.transform.position);
            }
            CheckHealth();
        }
    }

    private void CheckHealth()
    {
        if(health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
