using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Ability/MultiPush")]
public class MultiPush : Ability
{
    public ObjectPool particle;

    public override void ResetAbility()
    {
        particle.Reset();
        particle.AddObjects();
    }

    public override bool UseAbility(Player p, GameBoard b)
    {
        if (!b.CheckBound(p.gridPos + p.lastMoveDirection))
            return false;

        PlayerPushType pushType;
        p.board.PlayerMove(p.gridPos, p.lastMoveDirection, false, out pushType);
        p.board.PlayerMove(p.gridPos + p.lastMoveDirection, p.lastMoveDirection, false, out pushType);

        GameObject effect = particle.GetObject();
        effect.transform.position = p.transform.position;
        effect.transform.rotation = Quaternion.AngleAxis(Mathf.Atan2(p.lastMoveDirection.y, p.lastMoveDirection.x) * Mathf.Rad2Deg, Vector3.forward);
        effect.GetComponent<ParticleSelfDisable>().Activate();

        return true;
    }
}