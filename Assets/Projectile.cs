using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Projectile : GridItem
{
    public Vector2 moveDir;
    [SerializeField] private float tickMove;

    private float timer;
    private GameBoard board;

    private PlayerPushType pushType;

    public override bool CanPush()
    {
        return true;
    }

    public override void Destroy()
    {
        StartCoroutine(DestroyBox());
    }

    public IEnumerator DestroyBox()
    {
        transform.DOScale(0, .3f);
        transform.DORotate(Vector3.forward * Random.Range(-720, 720), .3f).SetEase(Ease.InElastic);
        yield return new WaitForSeconds(.3f);
        gameObject.SetActive(false);

    }

    public override void ResetItem(GameBoard b)
    {
        board = b;
        timer = tickMove;

        transform.localScale = Vector3.zero;
        transform.rotation = Quaternion.identity;
        transform.DOScale(1, .1f);
    }

    // Start is called before the first frame update
    void Start()
    {
        CanCrush = false;
        gridPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {

        
        if (timer > 0f)
        {
            timer -= Time.deltaTime;
            if (timer <= 0f)
            {
                board.PlayerMove(gridPos, moveDir, false, out pushType);
                if (pushType != PlayerPushType.None)
                {
                    Destroy();
                }
                else
                {
                    timer = tickMove;
                }
                board.PlayerMove(gridPos - moveDir, moveDir, false, out pushType);
                

                
            }
        }
    }
}
