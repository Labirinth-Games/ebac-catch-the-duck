using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(Rigidbody))]
public class LaunchController : MonoBehaviour
{
    [Header("Settings")]
    public float force = 10f;
    public void Strike(GameObject item)
    {
        GetComponent<Rigidbody>().velocity = new Vector3(0, 1f, 1f) * force;
    }
}
