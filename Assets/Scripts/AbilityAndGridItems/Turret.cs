using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Turret : GridItem
{
    public ObjectPool projectilePool;
    private Vector2Int shootDir;
    private bool isShoot;
    public float shootRate;

    private float timer;
    private GameBoard board;

    public override bool CanPush()
    {
        return true;
    }

    public override void ResetItem(GameBoard b)
    {
        isShoot = false;
        timer = shootRate;

        transform.localScale = Vector3.zero;
        transform.rotation = Quaternion.identity;
        transform.DOScale(1, .1f);

        projectilePool.AddObjects();

        board = b;
    }

    public override void GridMove(Vector2Int gPos, Vector2Int mPos)
    {
        if (!isShoot)
        {
            shootDir = mPos - gPos;
            display.transform.eulerAngles = Vector3.forward * Mathf.Rad2Deg * Mathf.Atan2(shootDir.y, shootDir.x);
            isShoot = true;
        }
        
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
        CanCrush = false;
        gridPos = Vector2Int.RoundToInt(transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        
        if (isShoot && timer > 0)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                timer = shootRate;
                display.transform.DOPunchScale(transform.localScale * 1.2f, .2f);

                PlayerPushType pushType;
                Vector2Int spawnPos = gridPos + shootDir;
                if (board.CheckBound(spawnPos))
                {
                    GridItem item = board.GetGridItem(spawnPos.x, spawnPos.y);
                    if (item != null)
                    {
                        if (item.CanPush())
                        {
                            board.PlayerMove(gridPos, shootDir, false, out pushType);
                        }
                    }
                    else
                    {
                        GameObject projectile = projectilePool.GetObject();
                        projectile.GetComponent<GridItem>().ResetItem(board);
                        Projectile proj = projectile.GetComponent<Projectile>();
                        if (proj)
                        {
                            proj.ResetItem(board);
                            proj.moveDir = shootDir;
                        }


                        board.ReplacePosition(spawnPos.x, spawnPos.y, projectile.GetComponent<GridItem>());
                    }
                }
            }
        }
    }
}
