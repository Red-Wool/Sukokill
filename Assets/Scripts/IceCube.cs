using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class IceCube : GridItem
{
    [SerializeField] private float tickMove;

    private Vector2Int moveDir;
    private bool isMove;

    private GameBoard board;
    private float timer;

    private PlayerPushType pushType;

    public override void ResetItem(GameBoard b)
    {
        isMove = false;
        timer = tickMove;

        transform.localScale = Vector3.zero;
        transform.rotation = Quaternion.identity;
        transform.DOScale(1, .1f);

        board = b;
    }

    public override void Move(Vector2Int gPos, Vector2 mPos)
    {
        gridPos = gPos;
        transform.DOMove(mPos, tickMove).SetEase(Ease.OutCubic);
    }

    public override void GridMove(Vector2Int gPos, Vector2Int mPos)
    {

        moveDir = mPos - gPos;
        isMove = true;
    }

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

    // Start is called before the first frame update
    void Start()
    {
        isMove = false;
        CanCrush = false;
        gridPos = Vector2Int.RoundToInt(transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        if (isMove && timer > 0f)
        {
            timer -= Time.deltaTime;
            if (timer <= 0f)
            {
                board.PlayerMove(gridPos, moveDir, false, out pushType);
                if (pushType != PlayerPushType.None)
                {
                    board.DestroyPosition((int)gridPos.x, (int)gridPos.y);
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
