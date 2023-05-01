using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Ability/Block Shoot")]
public class BlockShoot : Ability
{
    public ObjectPool blockPool;

    public int rounds;
    public float fireRate;

    public override void ResetAbility()
    {
        blockPool.Reset();
        blockPool.AddObjects();
    }

    public override bool UseAbility(Player p, GameBoard b)
    {
        Vector2Int direction = Vector2Int.RoundToInt(p.lastMoveDirection);
        Vector2Int spawnPos = Vector2Int.RoundToInt(p.gridPos) + direction;

        if (!b.CheckBound(spawnPos))
            return false;

        GridItem item = b.GetGridItem(spawnPos.x, spawnPos.y);
        if (item != null && !item.CanPush())
            return false;

        AbilityCoroutine.Instance.StartCoroutine(ShootRounds(direction, p, b));
        /*GridItem item = b.GetGridItem(spawnPos.x, spawnPos.y);
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
        }*/

        
        return true;
    }

    public IEnumerator ShootRounds(Vector2Int dir, Player p, GameBoard b)
    {
        PlayerPushType pushType;
        Vector2Int spawnPos;
        for (int i = 0; i < rounds; i++)
        {
            spawnPos = p.gridPos + dir;
            if (b.CheckBound(spawnPos))
            {
                GridItem item = b.GetGridItem(spawnPos.x, spawnPos.y);
                if (item != null)
                {
                    if (item.CanPush())
                    {
                        p.board.PlayerMove(p.gridPos, dir, false, out pushType);
                    }
                }
                else
                {
                    GameObject block = blockPool.GetObject();
                    block.GetComponent<GridItem>().ResetItem(b);
                    Projectile proj = block.GetComponent<Projectile>();
                    if (proj)
                    {
                        proj.ResetItem(b);
                        proj.moveDir = dir;
                    }
                    

                    b.ReplacePosition(spawnPos.x, spawnPos.y, block.GetComponent<GridItem>());
                }
            }

            yield return new WaitForSeconds(fireRate);
        }
    }
}
