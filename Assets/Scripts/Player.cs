using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Player : GridItem
{
    public GameBoard board;
    public SpriteRenderer render;
    public PlayerControl controls;
    public PlayerAbility abilityPrimary;
    public PlayerAbility abilitySecondary;
    
    /*public Image abilityDisplayPrimary;
    public Image abilityIconPrimary;
    public Image abilityDisplaySecondary;
    public Image abilityIconSecondary;

    public float abilityEnergyPrimary;
    public float abilityEnergySecondary;*/

    public ParticleSystem death;
    public ParticleSystem[] deathExtra;

    //public Image abilityRender;
    //public Image abilityBackgroundRender;

    public bool canMove;
    public Vector2Int lastMoveDirection { private set; get; }
    public Vector2Int currentInput { private set; get; }

    private List<Status> statuses;

    private int stock;
    private Vector2Int respawnPoint;

    //public delegate 
    //public event 

    private PlayerPushType pushType;
    
    // Start is called before the first frame update
    void Start()
    {
        statuses = new List<Status>();
        gridPos = Vector2Int.zero;
        CanCrush = true;
        canMove = true;
    }

    public void ResetPlayer(CharacterData data, Ability primary, Ability secondary)
    {

        lastMoveDirection = Vector2Int.zero;
        canMove = true;

        render.sprite = data.characterSprite;

        var d = death.colorOverLifetime;
        d.color = data.characterGradient;
        var e = death.trails;
        e.colorOverLifetime = data.characterGradient;

        for (int i = 0; i < deathExtra.Length; i++)
        {
            var t = deathExtra[i].trails;
            t.colorOverLifetime = data.characterGradient;
        }

        abilityPrimary.Reset(primary);
        abilitySecondary.Reset(secondary);
    }

    public void AddStatus(Status s)
    {
        statuses.Add(s);
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove)
        {
            abilityPrimary.Income();
            abilitySecondary.Income();
            //abilityDisplayPrimary.fillAmount = Mathf.Lerp(abilityDisplayPrimary.fillAmount, abilityEnergyPrimary / abilityPrimary.cost, Time.deltaTime * 10f);
            //abilityEnergyPrimary += Time.deltaTime * abilityPrimary.incomeTime;
            Vector2Int dir = Vector2Int.right * ((Input.GetKeyDown(controls.right) ? 1 : 0) - (Input.GetKeyDown(controls.left) ? 1 : 0));
            if (dir.x == 0)
                dir.y = ((Input.GetKeyDown(controls.up) ? 1 : 0) - (Input.GetKeyDown(controls.down) ? 1 : 0));
            currentInput = dir;


            if (dir != Vector2.zero)
            {
                lastMoveDirection = dir;
                board.PlayerMove(gridPos, dir, true, out pushType);

                abilityPrimary.EvaluteAbilityIncome(pushType);
                abilitySecondary.EvaluteAbilityIncome(pushType);
                //EvaluteAbilityIncome(pushType);
                foreach (Status s in statuses)
                {
                    s.OnMove(board, dir);
                }
            }

            if (Input.GetKeyDown(controls.abilityPrimary) && abilityPrimary.ability.cost <= abilityPrimary.energy)
            {
                if (abilityPrimary.UseAbility(this, board))
                {
                    //board.PlayerMove(gridPos, lastMoveDirection, true, out pushType);
                    abilityPrimary.energy = 0;
                }
            }
            if (Input.GetKeyDown(controls.abilitySecondary) && abilitySecondary.ability.cost <= abilitySecondary.energy)
            {
                if (abilitySecondary.UseAbility(this, board))
                {
                    //board.PlayerMove(gridPos, lastMoveDirection, true, out pushType);
                    abilitySecondary.energy = 0;
                }
            }

            for (int i = statuses.Count - 1; i >= 0; i--)
            {
                if (statuses[i].TimeDecay(Time.deltaTime))
                {
                    statuses.RemoveAt(i);
                }
            }
        }
        
    }

    public override void Destroy()
    {
        canMove = false;
        //death.Play();
        StartCoroutine(Death());
    }

    public override void Move(Vector2Int gPos, Vector2 mPos)
    {
        base.Move(gPos, mPos);

        //abilityEnergy += ability.incomeGetPushed;
    }

    public IEnumerator Death()
    {
        yield return new WaitForSeconds(.1f);
        transform.DOScale(0, .5f);
        death.Play();
    }

    public override bool CanPush()
    {
        return true;
    }
}

[System.Serializable]
public class PlayerAbility
{
    public Ability ability;
    public Image display;
    public Image icon;
    public float energy;
    public float energyMultiplier;

    public void Reset(Ability newAbility)
    {
        newAbility.ResetAbility();
        energy = 0;
        ability = newAbility;
        icon.sprite = ability.displayImage;
    }

    public bool UseAbility(Player p, GameBoard b)
    {
        return ability.UseAbility(p, b);
    }

    public void AddEnergy(float e)
    {
        energy += e * energyMultiplier;
        energy = Mathf.Clamp(energy, 0, ability.cost + ability.maxExtraIncome);
        display.fillAmount = Mathf.Lerp(display.fillAmount, energy / ability.cost, Time.deltaTime * 10f);
    }

    public void Income()
    {
        AddEnergy(Time.deltaTime * ability.incomeTime);
    }

    public void EvaluteAbilityIncome(PlayerPushType p)
    {
        switch (p)
        {
            case PlayerPushType.None:
                AddEnergy(ability.incomeMove);
                break;
            case PlayerPushType.Box:
                AddEnergy(ability.incomePushBox);
                break;
            case PlayerPushType.Player:
                AddEnergy(ability.incomePushPlayer);
                break;
        }
    }
}
