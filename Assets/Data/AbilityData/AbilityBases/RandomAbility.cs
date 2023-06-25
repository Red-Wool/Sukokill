using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Ability/Random Ability")]
public class RandomAbility : Ability
{
    public Ability[] abilityList;

    public override void ResetAbility()
    {
        base.ResetAbility();
        for (int i = 0; i < abilityList.Length; i++)
        {
            abilityList[i].ResetAbility();
        }
    }

    public override bool UseAbility(Player p, GameBoard b)
    {
        return abilityList[Random.Range(0, abilityList.Length)].UseAbility(p, b);
    }
}
