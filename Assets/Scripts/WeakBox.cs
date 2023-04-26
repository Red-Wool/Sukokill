using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class WeakBox : GridItem
{
    

    // Start is called before the first frame update
    void Start()
    {
        CanCrush = true;
        gridPos = transform.position;
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


    public override bool CanPush()
    {
        return true;
    }
}
