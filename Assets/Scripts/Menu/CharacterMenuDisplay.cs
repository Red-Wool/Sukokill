using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterMenuDisplay : MonoBehaviour
{
    [SerializeField] private CharacterData character;
    [SerializeField] private Image render;

    // Start is called before the first frame update
    void Start()
    {
        render.sprite = character.characterSprite;
    }

    public CharacterData GetCharacter()
    {
        return character;
    }
}
