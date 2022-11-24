using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScreenNextLevelUI : MonoBehaviour
{
    [Header("References")]
    public ScoreSO score;
    public TextMeshProUGUI scoreDisplay;
    public TextMeshProUGUI lifeDisplay;
    public TextMeshProUGUI newLevelDisplay;

    [Header("Settings")]
    public float timeToShow = 2f;

    #region --- Unity Events --
    private void Start()
    {
        GameManager.Instance?.OnChangeLevelUI.AddListener(DisplayLevel);

        gameObject.SetActive(false); // default not show
    }
    #endregion

    #region --- Functions --
   
    public void DisplayLevel(int level)
    {
        ShowScreen();
        newLevelDisplay.text = level.ToString();
        scoreDisplay.text = GameManager.Instance?.score.ToString();
        lifeDisplay.text = GameManager.Instance?.lifes.ToString();
    }

    void ShowScreen()
    {
        gameObject.SetActive(true);

        Invoke(nameof(HideScreen), timeToShow);
    }
    
    void HideScreen()
    {
        gameObject.SetActive(false);
    }
    #endregion
}
