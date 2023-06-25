using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Ability/Multi Place")]
public class MultiPlace : Ability
{
    public ObjectPool blockPool;
    public float placeTime;
    public Vector2[] spawnPositions;

    public override void ResetAbility()
    {
        blockPool.Reset();
        blockPool.AddObjects();
    }

    public override bool UseAbility(Player p, GameBoard b)
    {
        if (blockPool.Pool.Count == 0)
        {
            blockPool.AddObjects();
        }
        

        bool flag = false;
        List<Vector2Int> validSpots = new List<Vector2Int>();

        float a = Mathf.Atan2(p.lastMoveDirection.y, p.lastMoveDirection.x);

        for (int i = 0; i < spawnPositions.Length; i++)
        {
            Vector2 pos = new Vector2(spawnPositions[i].x, spawnPositions[i].y).Rotate(a);
            //Debug.Log(spawnPositions[i] + " " + pos);
            Vector2Int spawnPos = new Vector2Int(Mathf.RoundToInt(p.gridPos.x + pos.x), Mathf.RoundToInt(p.gridPos.y + pos.y));
            if (!b.CheckBound(spawnPos))
                continue;

            validSpots.Add(spawnPos);
            flag = true;

            
        }

        if (!flag)
            return false;

        //Vector2Int spawnPos = new Vector2Int((int)(p.gridPos.x + p.lastMoveDirection.x), (int)(p.gridPos.y + p.lastMoveDirection.y));
        for (int i = 0; i < validSpots.Count; i++)
        {
            GameObject block = blockPool.GetObject();
            if (block == null)
            {
                blockPool.Reset();
                blockPool.AddObjects();

                block = blockPool.GetObject();
            }

            block.GetComponent<GridItem>().ResetItem(b);
            block.transform.rotation = Quaternion.identity;
            b.JumpSpawn(placeTime, Vector2Int.RoundToInt(p.gridPos), validSpots[i], block.GetComponent<GridItem>());
        }


        return true;
    }
}
