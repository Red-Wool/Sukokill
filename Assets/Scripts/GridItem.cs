using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public abstract class GridItem : MonoBehaviour
{
    public Vector2 gridPos { protected set; get; }
    public bool CanCrush { protected set; get; }

    public abstract bool CanPush();

    public virtual void Destroy()
    {
        return;
    }

    public virtual void ResetItem(GameBoard b)
    {
        return;
    }

    public virtual void Move(Vector2 gPos, Vector2 mPos)
    {
        gridPos = gPos;
        transform.DOMove(mPos, .05f).SetEase(Ease.OutCubic);
    }
}
