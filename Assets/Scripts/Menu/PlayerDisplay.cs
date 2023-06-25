using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerDisplay : MonoBehaviour
{
    public TMP_Text playerName;
    public Image characterDisplay;
    public Image background;

    public Image selectedAbility;

    public Image abilityDisplay;
    public TMP_Text abilityName;
    public TMP_Text abilityDescription;

    private bool isOn;
    // Start is called before the first frame update
    void Start()
    {
        StopInspection();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SelectCharacter(CharacterData data)
    {
        isOn = true;

        playerName.text = data.characterName;
        characterDisplay.sprite = data.characterSprite;
        background.color = data.supportColor;
        Color col = data.baseColor;
        col.a = .2f;
        abilityDisplay.color = col;
    }

    public void NoCharacter(int num)
    {
        isOn = false;

        playerName.text = "Player " + (num + 1).ToString();

        string result = "";
        switch (num)
        {
            case 0:
                result = "WASD\nE to begin";
                break;
            case 1:
                result = "Arrow Keys\nRShift to begin";
                break;
            case 2:
                result = "IJKL\nO to begin";
                break;
            case 3:
                result = "TFGH\nY to begin";
                break;
        }
        abilityDescription.text = result;
        abilityName.text = "";


        characterDisplay.sprite = null;
        selectedAbility.enabled = false;


        Color col = Color.white;
        background.color = col;
        col.a = .2f;
        abilityDisplay.color = col;
    }

    public void SelectAbility(Ability ability)
    {
        selectedAbility.enabled = true;
        selectedAbility.sprite = ability.displayImage;
    }

    public void InspectAbility(Ability ability)
    {
        if (!isOn)
            return;

        abilityName.text = ability.abilityName;
        abilityDescription.text = ability.abilityDescription;

        characterDisplay.enabled = false;
        abilityDisplay.enabled = true;
        abilityDisplay.sprite = ability.displayImage;
    }

    public void StopInspection()
    {
        if (!isOn)
            return;

        abilityName.text = "";
        abilityDescription.text = "";

        characterDisplay.enabled = true;
        abilityDisplay.enabled = false;
    }
}
