using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Ability/OmniPush")]
public class OmniPush : Ability
{
    public ObjectPool particle;

    public override void ResetAbility()
    {
        particle.Reset();
        particle.AddObjects();
    }

    public override bool UseAbility(Player p, GameBoard b)
    {
        PlayerPushType pushType;
        p.board.PlayerMove(p.gridPos, Vector2.up, false, out pushType);
        p.board.PlayerMove(p.gridPos, Vector2.down, false, out pushType);
        p.board.PlayerMove(p.gridPos, Vector2.left, false, out pushType);
        p.board.PlayerMove(p.gridPos, Vector2.right, false, out pushType);

        GameObject effect = particle.GetObject();
        effect.transform.position = p.transform.position;
        effect.GetComponent<ParticleSelfDisable>().Activate();

        return true;
    }
}