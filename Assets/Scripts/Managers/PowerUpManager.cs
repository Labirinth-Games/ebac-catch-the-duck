using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PowerUpManager : Singleton<PowerUpManager>
{
    [Header("References")]
    public List<PowerUpBase> myPowerUps;
    public List<GameObject> allPowerUpsPrefab;
    public GameObject powerUpSelected;

    [Space()]
    [Header("Callbacks Game Flow")]
    public UnityEvent OnPickUp;
    public UnityEvent<bool> OnActivePowerUp;

    private bool _isActiveEffect = false;
    private int _itemSelected = 0;

    #region Actions
    public void ToggleActive()
    {
        if (myPowerUps.Count == 0)
            return;

        if (_isActiveEffect)
            myPowerUps[_itemSelected].DeactiveEffect();
        else
            myPowerUps[_itemSelected].ActiveEffect();

        _isActiveEffect = !_isActiveEffect;

        OnActivePowerUp?.Invoke(_isActiveEffect);
    }

    #endregion

    #region Utils
    public void AddPowerUp(PowerUpBase power)
    {
        // only added when there isnt
        var has = myPowerUps.Find(i => i == power);

        if(!has)
        {
            myPowerUps.Add(power);
            OnPickUp?.Invoke();
        }
    }

    public void SpawnNewPowerUp()
    {
        if (allPowerUpsPrefab.Count == 0)
            return;

        GameObject slab = LevelManager.Instance?.GetSpawnRandom();
        GameObject instance = Instantiate(allPowerUpsPrefab[Random.Range(0, allPowerUpsPrefab.Count)]);

        instance.transform.SetParent(slab.transform);
        instance.transform.position = slab.transform.position;
        instance.transform.localPosition += Vector3.up * 6;
    }

    public void ChangeSelectItem(int index)
    {
        _itemSelected = index;
    }
    #endregion

    #region --- Unity Events ---
    private void Start()
    {
        PowerUpHUD.Instance?.OnChangeSelect.AddListener(ChangeSelectItem);
    }

    private void OnDestroy()
    {
        PowerUpHUD.Instance?.OnChangeSelect.RemoveListener(ChangeSelectItem);
    }
    #endregion
}
