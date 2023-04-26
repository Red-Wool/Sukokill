using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class HelperFunctions
{
    //Rotate 2D 
    //https://answers.unity.com/questions/661383/whats-the-most-efficient-way-to-rotate-a-vector2-o.html

    public static Vector2 Rotate(this Vector2 v, float rad)
    {
        float sin = Mathf.Sin(rad);
        float cos = Mathf.Cos(rad);

        float tx = v.x;
        float ty = v.y;
        v.x = (cos * tx) - (sin * ty);
        v.y = (sin * tx) + (cos * ty);
        return v;
    }

}
