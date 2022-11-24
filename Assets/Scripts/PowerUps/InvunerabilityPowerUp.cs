using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvunerabilityPowerUp : PowerUpBase
{
    [Header("Settings")]
    public float timeoutEffect = .3f;

    private PlayerController _player;

    public override void ActiveEffect()
    {
       _player.canDead = false;

        //Invoke(nameof(TimeoutEffect), timeoutEffect);
    }

    public override void DeactiveEffect()
    {
        _player.canDead = true;
    }

    public void TimeoutEffect()
    {
        DeactiveEffect();
    }

    private void Awake()
    {
        base.displayName = "Invunerability";
    }

    private void Start()
    {
         _player = PlayerController.Instance;

        if (_player == null)
            Debug.LogWarning("PlayerController not found to use power up");
    }
}
