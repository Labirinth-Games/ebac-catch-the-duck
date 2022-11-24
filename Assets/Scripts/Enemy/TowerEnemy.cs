using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerEnemy : EnemyBase
{
    [Header("References")]
    public GunHelper gun;

    [Header("Settings")]
    public float fireRate = 2f;
    public float damage = 2f;
    public bool canContinueShotting = true;
    public bool shootContinuos = false;

    private Coroutine _coroutine = null;

    IEnumerator ShoottingSequency()
    {
        while (canContinueShotting)
        {
            gun.Shoot(target.transform);
            yield return new WaitForSeconds(fireRate);
        }
    }

    private void Update()
    {
        if (shootContinuos)
            _coroutine = StartCoroutine(ShoottingSequency());
        else if (_coroutine != null)
            StopCoroutine(_coroutine);
            
    }

    private void OnValidate()
    {
        if (gun == null)
        {
            gun = GetComponentInChildren<GunHelper>();
            gun.damage = damage;
        }

    }
}
