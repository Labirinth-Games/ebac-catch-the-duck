using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [Header("References")]
    public GameObject prefabText;

    [Header("Settings")]
    public List<MenuUIItem> menus;
    public int size = 72;
    public bool generateMenuOnStart = false;

    private MenuUIItem _currentSelected = null;
    private int _currentIndex = 0;

    public void GenerateMenu()
    {
        for(int i = 0; i < menus.Count; i++)
        {
            GameObject instance = Instantiate(prefabText, transform);
            instance.transform.localPosition = Vector3.zero;
            instance.transform.localPosition = Vector3.down * size * i;

            instance.GetComponent<TextMeshProUGUI>().text = menus[i].display;

            if (i == 0)
                _currentSelected = menus[i];

            menus[i].instance = instance;
        }
    }

    public void DestroyMenu()
    {
        menus.ForEach(i => Destroy(i.instance));
    }

    public void SetHighlight()
    {
        SetClean();

        if (_currentSelected != null && _currentSelected.instance != null)
            _currentSelected.instance.GetComponentInChildren<TextMeshProUGUI>().color = Color.yellow;
    }

    public void SetClean()
    {
        if (menus != null)
            menus.ForEach(i => { 
                if(i.instance != null)
                {
                    i.instance.GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
                }
            });
    }

    public void SelectItemMenu()
    {
        if(Input.GetKeyDown(KeyCode.Space))
            _currentSelected.OnCallback?.Invoke();
    }

    private void Start()
    {
        if(generateMenuOnStart)
            GenerateMenu();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            _currentIndex += 1;

            if (_currentIndex >= menus.Count)
                _currentIndex = 0;

            _currentSelected = menus[_currentIndex];
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            _currentIndex -= 1;

            if (_currentIndex < 0)
                _currentIndex = menus.Count - 1;

            _currentSelected = menus[_currentIndex];
        }

        SetHighlight();

        SelectItemMenu();

    }
}

[System.Serializable]
public class MenuUIItem
{
    public string display;
    public UnityEvent OnCallback;
    public GameObject instance;
}
