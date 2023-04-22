using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class ParticleSelfDisable : MonoBehaviour
{
    public float lifeTime;

    private ParticleSystem ps;
    private float timer;

    private void Start()
    {
        ps = GetComponent<ParticleSystem>();
    }

    public void Activate()
    {
        ps = GetComponent<ParticleSystem>();
        ps.Play();
        timer = lifeTime;
    }

    public void Update()
    {
        timer -= Time.deltaTime;
        if (timer < 0)
        {
            gameObject.SetActive(false);
        }
    }
}
