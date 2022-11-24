using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScreenDeadUI : MonoBehaviour
{
    [Header("References")]
    public TextMeshProUGUI scoreDisplay;

    #region --- Unity Events --
    private void Start()
    {
        GameManager.Instance?.OnFinishGame.AddListener(ShowDisplay);

        gameObject.SetActive(false); // default not show
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            GameManager.Instance?.loadScene.SetScene(1);
    }
    #endregion

    #region --- Functions --
    public void ShowDisplay()
    {
        gameObject.SetActive(true);
        scoreDisplay.text = GameManager.Instance?.score.ToString();
    }
    #endregion
}
