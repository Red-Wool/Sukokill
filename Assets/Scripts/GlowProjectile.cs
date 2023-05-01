using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GlowProjectile : Projectile
{
    private Telekinesis telekinesis;
    private Player owner;

    public void ResetItem(GameBoard b, Player player, Telekinesis t)
    {
        base.ResetItem(b);
        owner = player;
        telekinesis = t;
    }

    public override void HitObject(GridItem item)
    {
        if (item != null && item.GetComponent<Player>() == null)
        {
            telekinesis.teleKinesisObject = item;
            item.GetDisplay().transform.DOShakePosition(telekinesis.lifeTime, .2f, 100);
            owner.AddStatus(telekinesis);
        }

        base.HitObject(item);
    }
}
