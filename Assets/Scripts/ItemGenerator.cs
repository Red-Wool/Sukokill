using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ItemGenerator : MonoBehaviour
{
    public ObjectPool box;
    public GameBoard gameBoard;
    public SpawnerStat stat;

    private int maxSpace;

    private float timer;
    private float gameTime;
    private bool playing = false;
    private Board board;

    public void Start()
    {
        box.AddObjects();
    }

    private void Update()
    {
        

        if (playing)
        {
            gameTime += Time.deltaTime;
            timer -= Time.deltaTime;

            if (timer < 0f)
            {
                Debug.Log("Game");
                timer = stat.spawnRate;
                SpawnBoxes();
            }

        }
    }

    public void StartGame(Board b)
    {
        board = b;
        timer = stat.spawnRate;
        gameTime = 0;
        playing = true;

        maxSpace = 0;
        for (int y = 0; y < b.y.Length; y++)
        {
            for (int x = 0; x < b.y[y].xPos.Length; x++)
            {
                if (b.y[y].xPos[x])
                    maxSpace++;
            }
        }
    }

    public void EndGame()
    {
        playing = false;
    }

    public void SpawnBoxes()
    {
        List<Vector2Int> pos = new List<Vector2Int>();

        for (int y = 0; y < board.y.Length; y++)
        {
            for (int x = 0; x < board.y[y].xPos.Length; x++)
            {
                //Debug.Log(.01f * gameTime);
                if (board.y[y].xPos[x] && Random.Range(0f, 1f) < stat.spawnScale * gameTime)
                {
                    pos.Add(new Vector2Int(x, y));
                }
            }
        }

        while (pos.Count > maxSpace - stat.maxItemsFromSpace && pos.Count != 0)
        {
            pos.RemoveAt(Random.Range(0, pos.Count));
        }

        StartCoroutine(BoxWarning(pos.ToArray()));
    }

    public IEnumerator BoxWarning(Vector2Int[] pos)
    {
        foreach (Vector2Int p in pos)
        {
            gameBoard.board.y[p.y].bg[p.x].Warn();
        }
        yield return new WaitForSeconds(2.5f);

        GridItem[] item = new GridItem[pos.Length];
        GameObject obj;
        Vector2 target;

        for (int i = 0; i < pos.Length; i++)
        {
            obj = box.GetObject();
            item[i] = obj.GetComponent<GridItem>();

            obj.transform.rotation = Quaternion.identity;
            obj.transform.localScale = Vector3.zero;

            target = gameBoard.CalculatePosition(pos[i].x, pos[i].y, .5f);
            obj.transform.position = target + Vector2.up * 10f;

            obj.transform.DOMove(target, .5f).SetEase(Ease.Linear);
            obj.transform.DOScale(1, .5f);

        }
        yield return new WaitForSeconds(.5f);
        for (int i = 0; i < pos.Length; i++)
        {
            gameBoard.ReplacePosition(pos[i].x, pos[i].y, item[i]);
        }
    }
}
