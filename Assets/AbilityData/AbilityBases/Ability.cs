using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Ability/Template")]
public class Ability : ScriptableObject
{
    [Header("Ability Info"), Space(10)]
    public Sprite displayImage;
    public string abilityName;
    [TextArea]
    public string abilityDescription;

    [Space(10),Header("Cost"),Space(10)]
    public float cost;
    public float maxExtraIncome;
    public float incomeTime;
    public float incomeMove;
    public float incomePushBox;
    public float incomePushPlayer;
    public float incomeGetPushed;
    

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