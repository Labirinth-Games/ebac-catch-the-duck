using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using DG.Tweening;

public class ProjectileHelper : MonoBehaviour
{
    [Header("References")]
    public VisualEffect impactVFX;

    [Header("Settings")]
    public float damage = 3f;

    public void ImpactFloor()
    {
        if (impactVFX != null)
        {
            impactVFX.Play();
        }
    }
}
