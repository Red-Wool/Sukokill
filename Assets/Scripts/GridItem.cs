using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public abstract class GridItem : MonoBehaviour
{
    [SerializeField] protected SpriteRenderer display; public SpriteRenderer GetDisplay() { return display; }
    public Vector2Int gridPos { protected set; get; }
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

    public virtual void Move(Vector2Int gPos, Vector2 mPos)
    {
        gridPos = gPos;
        transform.DOMove(mPos, .05f).SetEase(Ease.OutCubic);
    }

    public virtual void GridMove(Vector2Int startPos, Vector2Int endPos)
    {
        return;
    }
}
