using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

public class OnColiderEnter : MonoBehaviour
{
    public Collider _Collider;
    public Collision _Collision;
    private void OnTriggerEnter(Collider other)
    {
        _Collider = other;
    }
    void OnCollisionEnter(Collision collision)
    {
        _Collision = collision;
    }
    
}
