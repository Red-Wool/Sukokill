using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

public class CameraManager : MonoBehaviour
{
    private Camera mainCam;
    public static Vector2 cameraBounds { get; private set; }

    public void Start()
    {
        mainCam = Camera.main;
    }

    public void Update()
    {
        cameraBounds = mainCam.ScreenToWorldPoint(new Vector3(mainCam.scaledPixelWidth, mainCam.scaledPixelHeight, 0));
    }

    public void ChangeFOV(float fov, bool smooth = true, float time = .5f)
    {
        ;
    }

    public IEnumerator SmoothFOV(float fov, float time)
    {
        mainCam.DOOrthoSize(fov, time).SetEase(Ease.OutSine);
        yield return new WaitForSeconds(time);


    }
}
