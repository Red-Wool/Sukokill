using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[CreateAssetMenu(menuName = "Ability/Glow")]
public class Glow : Ability
{

    public ObjectPool blockPool;
    public float controlTime;

    public override void ResetAbility()
    {
        base.ResetAbility();
    }

    public override bool UseAbility(Player p, GameBoard b)
    {

        Vector2Int dir = Vector2Int.RoundToInt(p.lastMoveDirection);
        Vector2Int spawnPos = Vector2Int.RoundToInt(p.gridPos) + dir;

        if (!b.CheckBound(spawnPos))
            return false;

        GridItem item = b.GetGridItem(spawnPos.x, spawnPos.y);
        if (item != null && !item.CanPush())
            return false;

        PlayerPushType pushType;
        Telekinesis t = new Telekinesis { hasLifeTime = true, lifeTime = controlTime };
        spawnPos = Vector2Int.RoundToInt(p.gridPos) + dir;

        if (item != null)
        {
            if (item.CanPush())
            {
                p.board.PlayerMove(p.gridPos, dir, false, out pushType);

                t.teleKinesisObject = item;
                item.GetDisplay().transform.DOShakePosition(3f, .2f, 100);
                p.AddStatus(t);
            }
        }
        else
        {
            GameObject block = blockPool.GetObject();
            block.GetComponent<GridItem>().ResetItem(b);
            GlowProjectile proj = block.GetComponent<GlowProjectile>();
            if (proj)
            {
                proj.ResetItem(b, p, t);
                proj.moveDir = dir;
            }


            b.ReplacePosition(spawnPos.x, spawnPos.y, block.GetComponent<GridItem>());
        }

        return true;
    }
}
