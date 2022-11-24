using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BloobEnemy : EnemyBase
{
    [Header("References")]
    public GunHelper gun;

    [Header("Settings")]
    public bool continueShooting = true;
    public int hp = 50;
    public int maxHP = 10;
    public int xp = 10;
    public float damage = 5f;
    public float fireRate = 1f;

    void UpdateSettingsToNextLevel()
    {
        damage += damage * (float) GameManager.Instance?.enemiesDamageProgression;
        xp += xp * (int) GameManager.Instance?.enemiesXpProgression;
        fireRate += fireRate * (int) GameManager.Instance?.enemiesFireRateProgression;
    }

    #region Flow Game
    IEnumerator Shooting()
    {
        while (continueShooting)
        {
            yield return new WaitForSeconds(fireRate);
            gun.Shoot(target.transform);
        }
    }

    protected override void Hit(int damage)
    {
        base.Hit(damage);

        hp -= damage;

        if (hp < 0)
        {
            Death();
        }
    }

    public void Death()
    {
        GameManager.Instance?.SetScore(xp);

        base.OnDead?.Invoke(gameObject); // called callback on parent class
        Destroy(gameObject);
    }
    #endregion

    #region -- unity events --
    private void Start()
    {
        StartCoroutine(Shooting());
    }

    private void OnValidate()
    {
        if (gun == null)
        {
            gun = GetComponentInChildren<GunHelper>();
            gun.damage = damage;
        }

        GameManager.Instance?.OnNextLevel.AddListener(UpdateSettingsToNextLevel);
    }
    #endregion
}
