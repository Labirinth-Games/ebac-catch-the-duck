using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreUI : Singleton<ScoreUI>
{
    public TextMeshProUGUI display;

    public void DisplayScore(int score)
    {
        display.text = "Score: " + score;
    }

    private void OnValidate()
    {
        if (display == null)
            display = GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Start()
    {
        GameManager.Instance?.OnChangeScoreUI.AddListener(DisplayScore);
    }
}
