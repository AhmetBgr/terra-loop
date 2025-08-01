using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    public Transform earth;
    public Camera cam;

    void Start()
    {
        Controller.GameEnded += OnGameEnded;
        cam = Camera.main;
    }
    private void OnDestroy()
    {
        Controller.GameEnded -= OnGameEnded;
    }

    private void OnGameEnded()
    {
        cam = Camera.main;
        var pos = earth.position;
        pos.z = -10f; // Set the camera's z position to ensure it's behind the Earth
        transform.DOMove(pos, 3f);

        //cam.DOOrthoSize(1.3f, 3f).SetEase(Ease.OutCubic);
        //var curSize = cam.orthographicSize;
        DOVirtual.Float(2f, 1.3f, 3f, (value) =>
        {
            Debug.Log($"Setting camera orthographic size to: {value}");
            Camera.main.orthographicSize = value;
        });
    }

}
