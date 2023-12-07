using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_Collider : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log($"{collision.collider.name} is collided");
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"{other.name} is triggered");
    }
}
