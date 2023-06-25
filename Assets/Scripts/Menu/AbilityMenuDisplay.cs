using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityMenuDisplay : MonoBehaviour
{
    [SerializeField] private Image displayImage; 
    [SerializeField] private Ability ability;

    public void Start()
    {
        SetUp(ability);
    }

    public void SetUp(Ability a)
    {
        ability = a;
        displayImage.sprite = ability.displayImage;
    }

    public Ability GetAbility()
    {
        return ability;
    }
}
