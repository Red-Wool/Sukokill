using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundTile : MonoBehaviour
{
    public ParticleSystem warn;

    public void Warn()
    {
        warn.Play();
    }
}
