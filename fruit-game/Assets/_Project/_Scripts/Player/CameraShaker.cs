using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class CameraShaker : MonoBehaviour
{
    [SerializeField] private Transform cam;
    [SerializeField] private Vector3 positionStrength;
    [SerializeField] private Vector3 rotationStrength;

    private static event Action Shake;

    public static void Invoke()
    {
        Shake?.Invoke();
    }
    private void OnEnable() => Shake += CameraShake;
    private void OnDisable() => Shake -= CameraShake;


    private void CameraShake()
    {
        cam.DOComplete();
        cam.DOShakePosition(0.3f, positionStrength);
        cam.DOShakeRotation(0.3f, rotationStrength);
    }
}
