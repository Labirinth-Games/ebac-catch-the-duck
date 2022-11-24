using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public GameObject portalPrefab;

    [Header("Settings")]
    public float delayToDestroy = 1f;

    private GameObject _instance;

    public void Spawn()
    {
        GameObject slab = LevelManager.Instance?.GetSpawnRandom();
        GameObject instance = Instantiate(portalPrefab);

        instance.transform.SetParent(slab.transform);
        instance.transform.localPosition = new Vector3(0, 10f, 0);

        _instance = instance;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player"))
            Destroy(_instance, delayToDestroy);
    }
}
