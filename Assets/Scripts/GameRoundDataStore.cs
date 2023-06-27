using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Round Data")]
public class GameRoundDataStore : ScriptableObject
{
    public int playerNum;
    public bool[] activePlayers;

    public int[] playerWin;
    public Ability[] playerAbilityPrimary;
    public Ability[] playerAbilitySecondary;

    public CharacterData[] character;

    public Map gameMap;
}
