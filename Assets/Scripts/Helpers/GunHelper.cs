using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using DG.Tweening;

public class GunHelper : MonoBehaviour
{
    [Header("References")]
    public GameObject prefabProjectile;

    [Header("Settings")]
    public float force = 3f;
    public float velocity = 2f;
    public float damage = 3f;

    private float _timeToKill = 3f;
    private float offsetY = .15f;

    public void Shoot(Transform target)
    {
        var instance = Instantiate(prefabProjectile);
        instance.transform.position = transform.position;

        ProjectileHelper projectile = instance.GetComponent<ProjectileHelper>();
        projectile.damage = damage;

        instance.transform
            .DOLocalJump(new Vector3(target.position.x, target.position.y + offsetY, target.position.z), force, 1, velocity)
            .SetEase(Ease.InQuad)
            .OnComplete(projectile.ImpactFloor);

        if (instance != null)
            Destroy(instance, _timeToKill);
    }
}
