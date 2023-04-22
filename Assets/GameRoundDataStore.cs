using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Round Data")]
public class GameRoundDataStore : ScriptableObject
{
    public int playerNum;

    public int[] playerWin;
    public Ability[] playerAbility;
}
