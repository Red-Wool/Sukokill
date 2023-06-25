using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Ability/Block Place")]
public class BlockPlace : Ability
{
    public ObjectPool blockPool;
    public float placeTime;

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
        GameObject block = blockPool.GetObject();
        if (block == null)
        {
            blockPool.Reset();
            blockPool.AddObjects();

            block = blockPool.GetObject();
        }

        Vector2Int spawnPos = new Vector2Int((int)(p.gridPos.x+p.lastMoveDirection.x), (int)(p.gridPos.y+ p.lastMoveDirection.y));

        if (!b.CheckBound(spawnPos))
        {
            block.SetActive(false);
            return false;
        }
            

        block.GetComponent<GridItem>().ResetItem(b);
        block.transform.rotation = Quaternion.identity;
        b.DelaySpawn(placeTime, spawnPos.x, spawnPos.y, block.GetComponent<GridItem>());
        return true;
    }
}