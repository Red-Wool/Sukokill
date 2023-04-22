using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Ability/Block Shoot")]
public class BlockShoot : Ability
{
    public ObjectPool blockPool;

    public override void ResetAbility()
    {
        blockPool.Reset();
        blockPool.AddObjects();
    }

    public override bool UseAbility(Player p, GameBoard b)
    {
        PlayerPushType pushType;
        Vector2Int spawnPos = new Vector2Int((int)(p.gridPos.x + p.lastMoveDirection.x), (int)(p.gridPos.y + p.lastMoveDirection.y));

        if (!b.CheckBound(spawnPos))
        {
            return false;
        }


        GridItem item = b.GetGridItem(spawnPos.x, spawnPos.y);
        if (item != null)
        {
            if (item.CanPush())
            {
                p.board.PlayerMove(p.gridPos, p.lastMoveDirection, false, out pushType);
            }
            else
            {
                return false;
            }
        }
        else
        {
            GameObject block = blockPool.GetObject();
            block.GetComponent<Projectile>().ResetItem(b);
            block.GetComponent<Projectile>().moveDir = p.lastMoveDirection;

            b.ReplacePosition(spawnPos.x, spawnPos.y, block.GetComponent<GridItem>());
        }

        
        return true;
    }
}
