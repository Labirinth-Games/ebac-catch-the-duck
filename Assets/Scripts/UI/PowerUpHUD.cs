using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class PowerUpHUD : Singleton<PowerUpHUD>
{
    [Header("References")]
    public List<PowerUpBase> powerUps;
    public GameObject UIPrefab;

    [Space()]
    [Header("Settings")]
    public float marginLeft = 20f;

    [Space()]
    [Header("Callback Game Flow")]
    public UnityEvent<int> OnChangeSelect;

    private List<GameObject> _instances;
    private bool _hasActivedPowerUp = false;
    private int selectIndex = 0;

    public void PowerUpListRender()
    {
        InstancesClear();

        for(var i = 0; i < PowerUpManager.Instance?.myPowerUps.Count; i++)
        {
            GameObject instance = Instantiate(UIPrefab, gameObject.transform.Find("Container").transform);
            instance.transform.position += Vector3.up * i * 56;

            TextMeshProUGUI displayName = instance.transform.Find("Display").gameObject.GetComponent<TextMeshProUGUI>();
            GameObject selectField = instance.transform.Find("Selected").gameObject;

            displayName.text = PowerUpManager.Instance?.myPowerUps[i]?.displayName;

            selectField.SetActive(selectIndex == i);

            _instances.Add(instance);
        }
    }

    private void InstancesClear()
    {
        _instances.ForEach(i => Destroy(i));
        _instances.Clear();
    }

    private void ChangeSelect()
    {
        // clean all selects
        _instances.ForEach(i => i.transform.Find("Selected").gameObject.SetActive(false));

        int newValue = selectIndex + 1;
        int count = (int)PowerUpManager.Instance?.myPowerUps.Count;

        if (newValue >= count)
            selectIndex = 0;
        else if (newValue < 0)
            selectIndex = count;
        else
            selectIndex = newValue;

        _instances[selectIndex].transform.Find("Selected").gameObject.SetActive(true);
        OnChangeSelect?.Invoke(selectIndex);
    }

    private void OnActivePowerUp(bool isActive)
    {
        _hasActivedPowerUp = isActive;

        if (isActive)
            _instances[selectIndex].transform.position -= Vector3.left * marginLeft;
        else
            _instances[selectIndex].transform.position += Vector3.left * marginLeft;
    }

    #region -- unity event --
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && !_hasActivedPowerUp)
        {
            ChangeSelect();
        } 
    }

    private void FixedUpdate()
    {
        // show tip when whas power up
       transform.Find("Tips").gameObject.SetActive(_instances.Count > 0);
    }

    private void Start()
    {
        PowerUpManager.Instance?.OnPickUp.AddListener(PowerUpListRender);
        PowerUpManager.Instance?.OnActivePowerUp.AddListener(OnActivePowerUp);
        _instances = new List<GameObject>();
    }

    private void OnDestroy()
    {
        PowerUpManager.Instance?.OnPickUp.RemoveListener(PowerUpListRender);
        PowerUpManager.Instance?.OnActivePowerUp.RemoveListener(OnActivePowerUp);
    }
    #endregion
}
