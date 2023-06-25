using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Character Data")]
public class CharacterData : ScriptableObject
{
    public string characterName;
    public Sprite characterSprite;
    public Gradient characterGradient;
    public Color baseColor;
    public Color supportColor;
    public Color textColor;
}
