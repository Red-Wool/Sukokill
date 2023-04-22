using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BombBlock : GridItem
{
    public GameBoard board;
    public ParticleSystem explode;

    [SerializeField] private GameObject display; 
    private float timer;
    private bool hasExplode;
    public override bool CanPush()
    {
        return true;
    }

    public override void Destroy()
    {
        if (!hasExplode)
            StartCoroutine(DestroyBox());
    }

    public IEnumerator DestroyBox()
    {
        hasExplode = true;
        yield return 0;
        transform.DOScale(0, .3f);
        transform.DORotate(Vector3.forward * Random.Range(-720, 720), .3f).SetEase(Ease.InElastic);

        Vector2Int gInt = new Vector2Int((int)gridPos.x, (int)gridPos.y);
        board.DestroyPosition(gInt.x + 1, gInt.y);
        board.DestroyPosition(gInt.x - 1, gInt.y);
        board.DestroyPosition(gInt.x, gInt.y + 1);
        board.DestroyPosition(gInt.x, gInt.y - 1);

        explode.Play();

        yield return new WaitForSeconds(3.5f);
        gameObject.SetActive(false);

        

    }

    public override void ResetItem(GameBoard b)
    {
        hasExplode = false;

        timer = 3f;
        display.transform.DOShakePosition(3f,.2f,100);
        board = b;

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
                Destroy();
            }
        }
        
    }
}
