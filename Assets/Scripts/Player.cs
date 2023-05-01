using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Player : GridItem
{
    public GameBoard board;
    public PlayerControl controls;
    public Ability ability;
    
    public Image abilityDisplay;
    public Image abilityIcon;

    public float abilityEnergy;

    public ParticleSystem death;

    public bool canMove;
    public Vector2Int lastMoveDirection { private set; get; }
    public Vector2Int currentInput { private set; get; }

    private List<Status> statuses;

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

    public void ResetPlayer(Ability a)
    {
        a.ResetAbility();

        lastMoveDirection = Vector2Int.zero;
        canMove = true;

        abilityEnergy = 0;
        ability = a;
        abilityIcon.sprite = ability.displayImage;
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
            abilityDisplay.fillAmount = Mathf.Lerp(abilityDisplay.fillAmount, abilityEnergy / ability.cost, Time.deltaTime * 10f);
            abilityEnergy += Time.deltaTime * ability.incomeTime;
            Vector2Int dir = Vector2Int.right * ((Input.GetKeyDown(controls.right) ? 1 : 0) - (Input.GetKeyDown(controls.left) ? 1 : 0));
            if (dir.x == 0)
                dir.y = ((Input.GetKeyDown(controls.up) ? 1 : 0) - (Input.GetKeyDown(controls.down) ? 1 : 0));
            currentInput = dir;


            if (dir != Vector2.zero)
            {
                lastMoveDirection = dir;
                board.PlayerMove(gridPos, dir, true, out pushType);
                EvaluteAbilityIncome(pushType);
                foreach (Status s in statuses)
                {
                    s.OnMove(board, dir);
                }
            }

            if (Input.GetKeyDown(controls.ability) && ability.cost <= abilityEnergy)
            {
                if (ability.UseAbility(this, board))
                {
                    //board.PlayerMove(gridPos, lastMoveDirection, true, out pushType);
                    abilityEnergy = 0;
                }
                    
            }

            abilityEnergy = Mathf.Clamp(abilityEnergy, 0, ability.cost + ability.maxExtraIncome);
            for (int i = statuses.Count - 1; i >= 0; i--)
            {
                if (statuses[i].TimeDecay(Time.deltaTime))
                {
                    statuses.RemoveAt(i);
                }
            }
        }
        
    }

    public void EvaluteAbilityIncome(PlayerPushType p)
    {
        switch (p)
        {
            case PlayerPushType.None:
                abilityEnergy += ability.incomeMove;
                break;
            case PlayerPushType.Box:
                abilityEnergy += ability.incomePushBox;
                break;
            case PlayerPushType.Player:
                abilityEnergy += ability.incomePushPlayer;
                break;
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
