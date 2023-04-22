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
    public Vector2 lastMoveDirection { private set; get; }
    public Vector2 currentInput { private set; get; }

    private PlayerPushType pushType;
    
    // Start is called before the first frame update
    void Start()
    {
        gridPos = Vector2.zero;
        CanCrush = true;
        canMove = true;
    }

    public void ResetPlayer(Ability a)
    {
        a.ResetAbility();

        lastMoveDirection = Vector2.zero;
        canMove = true;

        abilityEnergy = 0;
        ability = a;
        abilityIcon.sprite = ability.displayImage;
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove)
        {
            abilityDisplay.fillAmount = Mathf.Lerp(abilityDisplay.fillAmount, abilityEnergy / ability.cost, Time.deltaTime * 10f);
            abilityEnergy += Time.deltaTime * ability.incomeTime;
            Vector2 dir = Vector2.right * ((Input.GetKeyDown(controls.right) ? 1 : 0) - (Input.GetKeyDown(controls.left) ? 1 : 0));
            if (dir.x == 0)
                dir.y = ((Input.GetKeyDown(controls.up) ? 1 : 0) - (Input.GetKeyDown(controls.down) ? 1 : 0));
            currentInput = dir;


            if (dir != Vector2.zero)
            {
                lastMoveDirection = dir;
                //Debug.Log("Move");
                board.PlayerMove(gridPos, dir, true, out pushType);
                EvaluteAbilityIncome(pushType);
            }

            if (Input.GetKeyDown(controls.ability) && ability.cost <= abilityEnergy)
            {
                if (ability.UseAbility(this, board))
                {
                    //board.PlayerMove(gridPos, lastMoveDirection, true, out pushType);
                    abilityEnergy = 0;
                }
                    
            }
        }
        
    }

    public void EvaluteAbilityIncome(PlayerPushType p)
    {
        switch (p)
        {
            case PlayerPushType.None:
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

    public override void Move(Vector2 gPos, Vector2 mPos)
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
