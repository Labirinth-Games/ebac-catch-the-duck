using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyManager : Singleton<EnemyManager>
{
    [Header("References")]
    public List<GameObject> prefabsEnemiesList;
    public GameObject target;

    [Header("Settings")]
    public int amountEnemiesSpawned = 2;

    [Header("Settings")]
    public UnityEvent OnAllEnemiesDead;

    private List<GameObject> _instanceEnemies;

    #region Flow Game
    public void EnemyDead(GameObject instance)
    {
        _instanceEnemies.Remove(instance);

        if (_instanceEnemies.Count <= 0)
        {
            OnAllEnemiesDead?.Invoke();
        }
    }

    #endregion

    public void SpawnEnemies()
    {
        for(int i = 0; i < amountEnemiesSpawned; i++)
        {
            GameObject slabPostition = LevelManager.Instance.GetSpawnRandom();
            GameObject instance = Instantiate(prefabsEnemiesList[Random.Range(0, prefabsEnemiesList.Count)]);

            instance.transform.SetParent(slabPostition.transform);
            instance.transform.position = slabPostition.transform.position;

            EnemyBase enemy = instance.GetComponent<EnemyBase>();
            enemy.target = target;

            // added event when enemy is dead
            enemy.OnDead.AddListener(EnemyDead);

            _instanceEnemies.Add(instance);
        }
    }

    #region --- event unity ---
    private void Awake()
    {
        _instanceEnemies = new List<GameObject>();
    }
    #endregion
}
