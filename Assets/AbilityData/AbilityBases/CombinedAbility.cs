using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Ability/Combined Ability")]
public class CombinedAbility : Ability
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
        bool flag = false;
        for (int i = 0; i < abilityList.Length; i++)
        {
            if (abilityList[i].UseAbility(p, b))
                flag = true;
        }
        return flag;
    }
}
