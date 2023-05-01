using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Ability/Launch Punch")]
public class LaunchPunch : Ability
{
    public int spaces;

    public override void ResetAbility()
    {
        base.ResetAbility();
    }

    public override bool UseAbility(Player p, GameBoard b)
    {
        Vector2Int dir = p.lastMoveDirection;
        if (!b.CheckBound(p.gridPos + dir))
            return false;

        GridItem nextItem;
        PlayerPushType pushType;
        for (int i = 0; i < spaces; i++)
        {
            nextItem = b.GetGridItem(p.gridPos + dir);
            b.PlayerMove(p.gridPos, dir, true, out pushType);
            if (pushType != PlayerPushType.None)
            {
                break;
            }
        }
        return true;
    }
}
