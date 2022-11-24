using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPowerUp : PowerUpBase
{
    [Header("Settings")]
    public float forceJumpPercent = .3f;
    public bool isActive = false;
    
    private float _originalJumpHeight;
    private float _newJumpHeight;
    private PlayerController _player;

    public override void ActiveEffect()
    {
       _player.jumpHeight = _newJumpHeight;
    }

    public override void DeactiveEffect()
    {
        _player.jumpHeight = _originalJumpHeight;
    }

    private void Awake()
    {
        base.displayName = "Super Jump";
    }

    private void Start()
    {
         _player = PlayerController.Instance;

        if (_player != null && _player.isGrounded)
        {
            _originalJumpHeight = _player.jumpHeight;
            _newJumpHeight = (_player.jumpHeight * forceJumpPercent) + _player.jumpHeight;
        }
        else
            Debug.LogWarning("PlayerController not found to use power up");
    }
}
