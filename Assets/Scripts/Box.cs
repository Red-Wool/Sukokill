using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Box : GridItem
{
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
        CanCrush = false;
        gridPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
