using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Status
{
    public bool hasLifeTime;
    public float lifeTime;

    public bool TimeDecay(float time)
    {
        lifeTime -= time;
        return lifeTime <= 0;
    }

    public virtual void OnMove(GameBoard b, Vector2Int dir)
    {
        return;
    }
}
