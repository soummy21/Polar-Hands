using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;
using DG.Tweening;
public class CameraFunctions : MonoBehaviour
{

    //Singleton
    public static CameraFunctions instance;

    [Header("Camera Shaking")]
    [Header("General Shake Properties")]
    [SerializeField] float shakeMagnitude;
    [SerializeField] float shakeRoughness;
    [SerializeField] float shakeFadeInTime;
    [SerializeField] float shakeFadeOutTime;

    //tilting properties 
    Vector3 baseRotation;
    [Header("Camera Tilting")]
    [SerializeField] float tiltDuration;
    [SerializeField] float tiltZFactor;
    [SerializeField] float tiltXFactor;
    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    private void Start()
    {
        baseRotation = transform.eulerAngles;
        //Invoke("ShakeCameraOnCallGeneral",4f);
    }
    public void ShakeCameraOnCallGeneral()
    {
        CameraShaker.Instance.ShakeOnce(shakeMagnitude,shakeRoughness,shakeFadeInTime,shakeFadeOutTime);
    }
    public void ShakeCameraOnCallSpecific(float a,float b, float c, float d)
    {
        CameraShaker.Instance.ShakeOnce(a, b, c, d);
    }


    public void QuickTilt(Vector3 direction,bool rightMovemet)
    {
        if(rightMovemet)
            transform.DORotate(direction * tiltXFactor, tiltDuration).OnComplete(() => transform.DORotate(baseRotation, tiltDuration));

        transform.DORotate(direction * tiltZFactor, tiltDuration).OnComplete(() => transform.DORotate(baseRotation, tiltDuration));

    }
}
