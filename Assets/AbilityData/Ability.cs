using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Ability/Template")]
public class Ability : ScriptableObject
{
    public float cost;
    public float incomeTime;
    public float incomePushBox;
    public float incomePushPlayer;
    public float incomeGetPushed;

    public Sprite displayImage;

    public virtual void ResetAbility()
    {
        return;
    }

    public virtual bool UseAbility(Player p, GameBoard b)
    {
        //Debug.Log("Works!");
        return true;
    }
}

[System.Serializable]
public enum AbilityType
{
    None,
    Time,
    Input
}

/* Template
[CreateAssetMenu(menuName = "Ability/Template")]
public class Ability : Ability
{

    public override bool UseAbility(Player p, GameBoard b)
    {
        
        return;
    }
}
 * 
 * 
 */