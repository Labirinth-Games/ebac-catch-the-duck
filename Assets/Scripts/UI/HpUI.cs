using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HpUI : Singleton<HpUI>
{
    public TextMeshProUGUI display;

    public void DisplayHP(float hp)
    {
        display.text = "HP: " + hp;
    }

    private void OnValidate()
    {
        if (display == null)
            display = GetComponentInChildren<TextMeshProUGUI>();
    }
}
