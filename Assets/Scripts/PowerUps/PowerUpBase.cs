using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using UnityEngine.Events;
using DG.Tweening;

public class PowerUpBase : MonoBehaviour
{
    [Header("Rerferences")]
    public VisualEffect vfx;

    [Header("Settings")]
    public string displayName;

    #region Flow Game
    public virtual void ActiveEffect() { }
    public virtual void DeactiveEffect() { }
    #endregion

    #region Utils
    public void Hide()
    {
        transform.DOScale(0, .5f).SetEase(Ease.InOutBounce);
        vfx?.Stop();
    }
    #endregion

    #region --- Unity Events ---
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            PowerUpManager.Instance?.AddPowerUp(this);
            Hide();
            Destroy(gameObject, 5f);
        }
    }
    #endregion
}
