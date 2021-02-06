using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    float rotateSpeed = 50.0f;
    void Update()
    {
        transform.Rotate(Vector3.up * Time.deltaTime * rotateSpeed);
    }
}
