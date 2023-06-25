using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TemporaryWall : GridItem
{
    private float timer;

    public override bool CanPush()
    {
        return false;
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
        timer = 15f;
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
