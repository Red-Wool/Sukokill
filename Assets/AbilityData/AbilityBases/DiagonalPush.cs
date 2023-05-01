using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Ability/Diagonal Push")]
public class DiagonalPush : Ability
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
        p.board.PlayerMove(p.gridPos, new Vector2Int(1, 1), false, out pushType);
        p.board.PlayerMove(p.gridPos, new Vector2Int(-1, 1), false, out pushType);
        p.board.PlayerMove(p.gridPos, new Vector2Int(1, -1), false, out pushType);
        p.board.PlayerMove(p.gridPos, new Vector2Int(-1, -1), false, out pushType);

        GameObject effect = particle.GetObject();
        effect.transform.position = p.transform.position;
        effect.GetComponent<ParticleSelfDisable>().Activate();
        
        return true;
    }
}
