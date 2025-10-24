using UnityEngine;
using System.Collections;

[RequireComponent(typeof(ParticleSystem))]
public class SmokeScript : MonoBehaviour
{
    ParticleSystem ps;

    void Awake()
    {
        ps = GetComponent<ParticleSystem>();
        if (ps == null) ps = GetComponentInChildren<ParticleSystem>();
    }

    void Start()
    {
        // Start coroutine to destroy when particles (including children) are finished
        StartCoroutine(DestroyWhenDone());
    }

    IEnumerator DestroyWhenDone()
    {
        // Wait until the particle system (and children) are no longer alive
        yield return new WaitUntil(() => ps == null || !ps.IsAlive(true));
        Destroy(gameObject);
    }
}
