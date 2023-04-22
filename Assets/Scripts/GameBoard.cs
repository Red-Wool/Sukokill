using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GameBoard : MonoBehaviour
{
    public Board board;
    public Vector2 gridSize;
    public Vector2 gridOffset;

    public ObjectPool background;
    

    // Start is called before the first frame update
    void Start()
    { 
        background.AddObjects();
        CreateBackground();
        UpdatePosition();
    }

    private void FixedUpdate()
    {

        UpdatePosition();
    }

    public bool CheckBound(Vector2 pos)
    {
        return pos.x >= 0 && pos.y >= 0 && pos.y <= board.y.Length - 1 && pos.x <= board.y[(int)pos.y].xPos.Length - 1 && board.y[(int)pos.y].xPos[(int)pos.x];
        //return ()
    }

    public GridItem GetGridItem(int x, int y)
    {
        return board.y[y].item[x];
    }

    public void CreateBackground()
    {
        GameObject obj;
        //Vector2 pos;

        
        for(int y = 0; y < board.y.Length; y++)
        {
            board.y[y].bg = new BackgroundTile[board.y[y].xPos.Length];
            for (int x = 0; x < board.y[y].xPos.Length; x++)
            {
                
                if (board.y[y].xPos[x])
                {
                    if (board.y[y].bg[x] != null)
                        board.y[y].bg[x].gameObject.SetActive(false);

                    obj = background.GetObject();
                    board.y[y].bg[x] = obj.GetComponent<BackgroundTile>();

                    //pos = CalculatePosition(x, y);
                    //obj.transform.position = pos + Vector2.up * 10f;
                    //obj.transform.DOMove(CalculatePosition(x, y), 1f).SetEase(Ease.Flash);

                }
            }
        }
    }

    public void UpdatePosition()
    {
        GridItem i;
        BackgroundTile bg;
        
        for (int y = 0; y < board.y.Length; y++)
        {
            for (int x = 0; x < board.y[y].item.Length; x++)
            {
                i = board.y[y].item[x];
                if (i != null)
                {
                    if (!i.gameObject.activeSelf)
                    {
                        board.y[y].item[x] = null;
                        
                    }
                    else
                    {
                        board.y[y].item[x].Move(new Vector2(x, y), CalculatePosition(x, y));
                    }
                    
                }
                    

                bg = board.y[y].bg[x];
                if (bg != null)
                    bg.transform.position = CalculatePosition(x, y);

            }
        }
    }

    public void DelaySpawn(float delay, int x, int y, GridItem item)
    {
        if (!CheckBound(new Vector2(x, y)))
        {
            Debug.Log("Out of Bounds! " + x + " " + y);
            item.gameObject.SetActive(false);
            return;
        }

        board.y[y].bg[x].Warn();
        StartCoroutine(DelaySpawnEnumerator(delay, x, y, item));
    }

    public IEnumerator DelaySpawnEnumerator(float delay, int x, int y, GridItem item)
    {
        float tick = Mathf.Min(delay, .5f);

        item.transform.rotation = Quaternion.identity;
        item.transform.position = Vector2.up * 1000f;
        item.transform.localScale = Vector3.zero;

        yield return new WaitForSeconds(Mathf.Max(delay - .5f, 0));

        Vector2 target = CalculatePosition(x, y, tick);
        item.transform.position = target + Vector2.up * 10f;
        item.transform.DOMove(target, tick).SetEase(Ease.Linear);
        item.transform.DOScale(1, .5f);

        yield return new WaitForSeconds(tick);

        ReplacePosition(x, y, item);
    }

    public void DestroyPosition(int x, int y)
    {
        if (!CheckBound(new Vector2(x, y)))
        {
            Debug.Log("Out of Bounds! " + x + " " + y);
            return;
        }
        if (board.y[y].item[x] != null)
            board.y[y].item[x].Destroy();

        board.y[y].item[x] = null;
    }

    public void ReplacePosition(int x, int y, GridItem item)
    {
        DestroyPosition(x, y);

        board.y[y].item[x] = item;
        item.transform.position = CalculatePosition(x, y);
    }

    public Vector2 CalculatePosition(int x, int y, float addTime = 0f)
    {
        float time = Time.fixedTime + addTime;
        Vector2 sizeChange = Vector3.one;
        Vector2 gridChange = new Vector2(Mathf.Cos(time + (y) * .3f) * .3f, Mathf.Sin(time + (x)*.3f)*.3f);
        return (new Vector2(x, y) + gridOffset + gridChange) * (gridSize + sizeChange);
    }

    public void PlayerMove(Vector2 pos, Vector2 dir, bool movePlayer, out PlayerPushType pushType)
    {
        Vector2 curPos = pos + dir;
        pushType = PlayerPushType.None;

        Stack<MoveOperation> op = new Stack<MoveOperation>();
        if (movePlayer) op.Push(new MoveOperation { item = board.y[(int)pos.y].item[(int)pos.x], origin = pos, result = curPos });

        if (!CheckBound(curPos))
        {
            return;
        }
        //Debug.Log(curPos);
        GridItem item = board.y[(int)curPos.y].item[(int)curPos.x];

        if (item != null)
        {
            if (item.GetComponent<Player>())
            {
                pushType = PlayerPushType.Player;
            }
            else
            {
                pushType = PlayerPushType.Box;
            }
        }
        

        while (item != null)
        {
            //Debug.Log(curPos);
            if (item.CanPush())
            {
                op.Push(new MoveOperation { item = item, origin = curPos, result = curPos + dir });
                curPos += dir;


                if (!CheckBound(curPos)) break;

                item = board.y[(int)curPos.y].item[(int)curPos.x];


                if (item != null && item.CanCrush) 
                {
                    op.Push(new MoveOperation { item = item, origin = curPos, result = curPos + dir, invalid = true });
                    break; 
                } 
            }
            else
            {
                return;
            }
        }

        foreach (MoveOperation o in op)
        {
            //Debug.Log(item);
            board.y[(int)o.origin.y].item[(int)o.origin.x] = null;
            o.item.Move(o.result, CalculatePosition((int)o.result.x, (int)o.result.y));
            if (CheckBound(o.result) && (!o.invalid || board.y[(int)o.result.y].item[(int)o.result.x] == null))
            {
                board.y[(int)o.result.y].item[(int)o.result.x] = o.item;
            }
            else
            {
                o.item.Destroy();
            }
        }
    }
}

[System.Serializable]
public enum PlayerPushType
{
    None,
    Box,
    Player
}

[System.Serializable]
public class Board
{
    public Line[] y;
}

[System.Serializable]
public class MoveOperation
{
    public GridItem item;
    public Vector2 origin;
    public Vector2 result;
    public bool invalid;
}

[System.Serializable]
public class Line
{
    public bool[] xPos;
    public GridItem[] item;
    //[HideInInspector] 
    public BackgroundTile[] bg;
}
