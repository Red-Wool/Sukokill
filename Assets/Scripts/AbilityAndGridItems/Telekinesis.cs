using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Telekinesis : Status
{

    public GridItem teleKinesisObject;

    public override void OnMove(GameBoard b, Vector2Int dir)
    {
        PlayerPushType p;
        b.PlayerMove(teleKinesisObject.gridPos - dir, dir, false, out p);
    }
}
