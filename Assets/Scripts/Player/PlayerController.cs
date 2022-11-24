using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;

public class PlayerController : Singleton<PlayerController>
{
    [Header("References")]
    public StateAnimation animationState;
    public GameObject prefabFlameAnimationDead;

    [Space()]
    [Header("Settings")]
    public float hp = 10f;
    public float maxHp = 10f;

    public int xpNegativeBydead = 10;
    public int damage = 5;

    public float speed = 4f;
    public float fastSpeed = 8f;
    public float jumpHeight = 4f;
    public Ease jumpEaseAnimation = Ease.OutCubic;
    public float timeToWaitHit = .5f;

    public bool isGrounded = true;
    public bool isFalling = false;
    public bool canDead = true;

    public float timeToRespawn = 5f;

    [Space()]
    [Header("CallBacks")]
    public UnityEvent OnPlayerDead;
    public UnityEvent<GameObject> OnGathering;
    public UnityEvent<GameObject> OnStrikeItem;
    public UnityEvent OnHit;

    private float _currentSpeed;
    private bool _canGathering = false;
    private bool _isDead = false; // used when the player is dead
    private bool _isPause = false; // used when show menu (all function of player is deactivated)
    private bool _isFreezing = false; // similar at pause but not show menu
    private float _timeWaitToHit = 0;

    void Update()
    {
        if (_isPause || _isDead || _isFreezing)
            return;

        // time to wait to can attack again
        _timeWaitToHit += Time.deltaTime;

        Moviment();
        Gathering();
        Jump();
        Attack();
        UsePowerUp();
    }

    private void Start()
    {
        GameManager.Instance?.OnPauseEvent.AddListener(OnPause);
        GameManager.Instance?.OnResumeEvent.AddListener(OnResume);

        HpUI.Instance?.DisplayHP(hp);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.CompareTag("Ground") || collision.transform.CompareTag("Dead"))
        {
            isGrounded = true;
            isFalling = false;
        } 
        else
        {
            isFalling = true;
        }
       
        if(collision.transform.CompareTag("Dead"))
        {
            Dead();
        } 
        
        if(collision.transform.CompareTag("Projectile"))
        {
            ProjectileHelper projectile = collision.gameObject.GetComponent<ProjectileHelper>();

            if (projectile != null)
                Hit(projectile.damage);
        }

        if (collision.transform.CompareTag("Portal"))
        {
            Freezing();
            LevelManager.Instance?.NextLevel();

            Invoke(nameof(Unfreezing), 2f);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.transform.CompareTag("Item") && _canGathering)
        {
            OnGathering?.Invoke(null);
            _canGathering = false;
            animationState.SetState(AnimationType.Gathering);
        }
    }

    #region Player Actions
    public bool GetCurrentAnimation(AnimationType type)
    {
        return animationState.IsCurrentAnimation(type);
    }

    public bool CanAttackAgain()
    {
        bool valid = _timeWaitToHit >= timeToWaitHit;

        if (valid)
            _timeWaitToHit = 0;

        return valid;
    }

    public void Moviment()
    {
        if (Input.GetKey(KeyCode.LeftShift))
            _currentSpeed = fastSpeed;
        else
            _currentSpeed = speed;

        var verticalPosition = Input.GetAxis("Vertical");
        var horizontalPosition = Input.GetAxis("Horizontal");

        Vector3 forward = verticalPosition * transform.forward;
        Vector3 strafe = horizontalPosition * transform.right;
        var move = forward + strafe;

        if (move != Vector3.zero)
            gameObject.transform.position += move * _currentSpeed * Time.deltaTime;

        animationState.SetState(AnimationType.Walking, move == Vector3.zero ? 0 : _currentSpeed);
    }

    public void Jump()
    {
        // Changes the height position of the player..
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            animationState.SetState(AnimationType.Jump);
            transform.DOMoveY(transform.position.y + jumpHeight, .2f).SetEase(jumpEaseAnimation);
            isGrounded = false;
            isFalling = true;
        }

        animationState.SetState(AnimationType.Falling, isFalling);
    }

    public void Attack()
    {
        if(Input.GetButtonDown("Fire1") && CanAttackAgain())
        {
            animationState.SetState(AnimationType.Attack);
            GetComponent<PlayerSFX>().Play(SFXType.PUNCH);

        }
    }
    
    public void Gathering()
    {
        if (Input.GetKeyDown(KeyCode.F) && isGrounded)
        {
            _canGathering = true;
        }
    }

    public void Dead()
    {
        if (!canDead)
            return;

        // freezing the screen
        _isDead = true;
        Freezing();

        OnPlayerDead?.Invoke();

        animationState.SetState(AnimationType.Dead);
        GetComponent<PlayerSFX>().Play(SFXType.DEAD);

        GameManager.Instance?.SetScore(-xpNegativeBydead);
        
        if (prefabFlameAnimationDead != null)
        {
            GameObject instance = Instantiate(prefabFlameAnimationDead, transform.position, Quaternion.identity) as GameObject;
            instance.transform.localPosition = transform.position;
            instance.transform.eulerAngles = transform.eulerAngles;
            
            Destroy(instance, timeToRespawn);
        }

        if (GameManager.Instance?.lifes > 0)
        {
            Invoke(nameof(Respawn), timeToRespawn);
            GameManager.Instance?.SetLife(-1);
        }
        else
            GameManager.Instance?.FinishGame();

    }

    public void Hit(float damage)
    {
        hp -= damage;
        HpUI.Instance?.DisplayHP(hp);

        OnHit?.Invoke();

        if (hp < 0 && !_isDead)
            Dead();
    }

    public void Restore()
    {
        _isDead = false;
        Unfreezing();

        hp = maxHp;
    }

    public void Respawn()
    {
        animationState.SetState(AnimationType.Idle);

        Restore();

        GameObject sliceFloorSpawn = LevelManager.Instance?.GetSpawnFloorInit();

        if(sliceFloorSpawn != null) 
            transform.position = sliceFloorSpawn.transform.position + Vector3.up;
    }

    public void UsePowerUp()
    {
        if(Input.GetKeyDown(KeyCode.E))
            PowerUpManager.Instance?.ToggleActive();
    }
    #endregion

    #region Game Flow
    public void OnPause()
    {
        _isPause = true;
    }
    
    public void OnResume()
    {
        _isPause = false;
    }

    public void Freezing()
    {
        _isFreezing = true;
        CameraMoveHelper.Instance?.Freezing();
    }
    
    public void Unfreezing()
    {
        _isFreezing = false;
        CameraMoveHelper.Instance?.Unfreezing();
    }
    #endregion
}
