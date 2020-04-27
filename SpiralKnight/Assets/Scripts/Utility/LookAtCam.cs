using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCam : MonoBehaviour
{
    [SerializeField] private Vector3 dir;

    void Update()
    {
        transform.LookAt(transform.position + dir);
    }
}