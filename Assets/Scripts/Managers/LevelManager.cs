using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

public class LevelManager : Singleton<LevelManager>
{
    [Header("Settings")]
    public GameObject prefabSliceFloor;

    [Space()]
    [Header("Settings")]
    public int initialLevelSize = 3; // create a level with 3x3
    public float marginBetween = .1f;
    public float heightLevel = 1f;
    public float timeBetweenAnimationFloor = .2f;

    [Space()]
    [Header("Animation")]
    public Ease ease = Ease.OutCirc;

    [Space()]
    [Header("CallBacks")]
    public UnityEvent OnFinishLoadLevels;

    private List<GameObjectInstance> _instance;
    private List<GameObjectInstance> _instanceToStart;

    private int _sizeFloorLevel = 0;

    #region Flow Game
    public void NextLevel()
    {
        GameManager.Instance?.SetUpLevel();

        decimal newValue = (decimal)((GameManager.Instance?.level * GameManager.Instance?.levelProgression) + _sizeFloorLevel);
        _sizeFloorLevel = (int)System.Math.Round(newValue);

        DestroyAll();
        GenerateLevel();
    }
    #endregion

    public void GenerateLevel()
    {
        for(var i = 0; i < _sizeFloorLevel; i++)
            for(var j = 0; j < _sizeFloorLevel; j++)
            {
                GameObject instance = Instantiate(prefabSliceFloor, transform);
                var (z, x) = GetFloorSize(instance);
                Vector3 scale = instance.transform.localScale;
                float posX = (x * i * scale.x) + marginBetween * i * scale.x;
                float posZ = (z * j * scale.z) + marginBetween * j * scale.z;

                instance.transform.position = new Vector3(posX + transform.position.x, 0, posZ + transform.position.z);

                if((j % _sizeFloorLevel) == 0)
                    _instanceToStart.Add(new GameObjectInstance(instance));
                else
                    _instance.Add(new GameObjectInstance(instance));
            }

        StartCoroutine(LevelSliceMove());
        OnFinishLoadLevels?.Invoke();
    }

    public void DestroyAll()
    {
        _instance.ForEach(i => Destroy(i.instance));
        _instanceToStart.ForEach(i => Destroy(i.instance));

        _instance.Clear();
        _instanceToStart.Clear();
    }

    public (float z, float x) GetFloorSize(GameObject target)
    {
        var mesh = target.GetComponentInChildren<MeshFilter>().mesh;
       
        var z = mesh.bounds.size.z;
        var x = mesh.bounds.size.x;

        return (z, x);
    }

    public GameObject GetSpawnFloorInit()
    {
        return _instanceToStart[Random.Range(0, _instanceToStart.Count)].instance;
    }

    public GameObject GetSpawnRandom()
    {
        return _instance[Random.Range(0, _instance.Count)].instance;
    }

    IEnumerator LevelSliceMove()
    {
        float levelSizeCanMove = _sizeFloorLevel * _sizeFloorLevel - _sizeFloorLevel;
        float numberLevels = levelSizeCanMove / 2;
        float sliceByLevel = levelSizeCanMove / numberLevels;
        List<GameObjectInstance> instanceClone = new List<GameObjectInstance>();
        instanceClone.AddRange(_instance);

        for(var i = 1; i <= numberLevels; i++)
            for(var j = 0; j < sliceByLevel; j++)
            {
                var sliceFloor = instanceClone[Random.Range(0, instanceClone.Count)];
                sliceFloor.instance.transform.DOMoveY(heightLevel * i, .2f).SetEase(ease);

                instanceClone.Remove(sliceFloor);

                yield return new WaitForSeconds(timeBetweenAnimationFloor);
            }
    }


    private void Start()
    {
        _instance = new List<GameObjectInstance>();
        _instanceToStart = new List<GameObjectInstance>();

        _sizeFloorLevel = initialLevelSize;

        GenerateLevel();
    }
}

public class GameObjectInstance
{
    public bool hasChanged = false;
    public GameObject instance;

    public GameObjectInstance(GameObject instance)
    {
        this.instance = instance;
    }
}
