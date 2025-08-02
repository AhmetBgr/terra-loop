using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    public float rotationSpeed = 36f; // 360 degrees in 10 seconds


    void Start()
    {
    }
    void Update()
    {
        // Rotate around Z axis
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);


    }
}
