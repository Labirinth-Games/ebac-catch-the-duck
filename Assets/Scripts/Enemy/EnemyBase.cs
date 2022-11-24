using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyBase : MonoBehaviour
{
    [Header("References")]
    public GameObject target;
    public Material hitHighlightMaterial;

    [Header("Super Settings")]
    public float timeToWaitHit = .5f;
    public float timeToShowHighlight = 1f;

    [Header("Callbacks")]
    public UnityEvent<GameObject> OnDead;
    public UnityEvent<int> OnHit;


    private float _timeWaitToHit = 0;
    private Material[] _oldMaterial;

    #region Flow Game
    protected virtual void Hit(int damage)
    {
        OnHit?.Invoke(damage);
        StartCoroutine(HitAnimation());
    }
    private bool CanHitAgain()
    {
        bool valid = _timeWaitToHit >= timeToWaitHit;

        if (valid)
            _timeWaitToHit = 0;

        return valid;
    }
    #endregion

    #region Animations
    // this coroutiine, added a other new material above to the previous
    // and before a time remove a highlight marerial
    IEnumerator HitAnimation()
    {
        var meshRenderer = GetComponentInChildren<MeshRenderer>();
        _oldMaterial = meshRenderer.materials;

        Material[] m = new Material[] { _oldMaterial[0], hitHighlightMaterial };

        meshRenderer.materials = m;

        yield return new WaitForSeconds(timeToShowHighlight);

        meshRenderer.materials = new Material[] { _oldMaterial[0] };
    }
    #endregion

    #region --- unity event ---
    private void OnTriggerStay(Collider other)
    {
        // valid if has melee
        if (other.transform.CompareTag("Melee"))
        {
            PlayerController player = PlayerController.Instance;

            if (player.GetCurrentAnimation(AnimationType.Attack) && CanHitAgain())
                Hit(player.damage);
        }
    }

    private void Update()
    {
        // time to wait to can attack again
        _timeWaitToHit += Time.deltaTime;
    }
    #endregion
}
