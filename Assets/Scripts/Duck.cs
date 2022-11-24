using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Duck : MonoBehaviour
{
    [Header("Settings")]
    public float strikeForce = 4f;
    public bool hasAttached = false;
    public int xp = 20;

    private GameObject _parent;

    public void SetSpawn()
    {
        GameObject _slab = LevelManager.Instance.GetSpawnFloorInit();
        transform.SetParent(_slab.transform);

        transform.rotation = Quaternion.Euler(new Vector3(-90, 0, 0));
        transform.localScale = Vector3.one;
        transform.position = Vector3.zero;
        transform.localPosition = new Vector3(0, .4f, 0);
    }

    // when get duck added on the back
    public void OnGatheringDuck(GameObject place)
    {
        Restore();
        hasAttached = true;
        _parent = place;

        transform.SetParent(place.transform);
        transform.localScale *= .8f;
        transform.localPosition = new Vector3(0, .4f, -.01f);
        transform.localRotation = Quaternion.Euler(new Vector3(-180, 0, 0));

        // added xp to score
        GameManager.Instance.SetScore(xp);
    }

    public void OnStrickeItem(GameObject user)
    {
        if (hasAttached)
        {
            _parent.transform.DetachChildren();
            transform.LookAt(user.transform);

            transform.DOLocalJump(user.transform.forward * strikeForce, strikeForce, 1, 3f);

            hasAttached = false;

            Invoke(nameof(SetSpawn), 3f);
        }
    }

    public void Restore()
    {
        transform.localScale = Vector3.one;
        transform.position = Vector3.one;
        transform.rotation = Quaternion.Euler(Vector3.zero);
        transform.eulerAngles = Vector3.zero;
    }
}
