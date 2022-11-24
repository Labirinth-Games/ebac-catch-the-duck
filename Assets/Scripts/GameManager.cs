using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : Singleton<GameManager>
{
    [Header("References Managers")]
    public EnemyManager enemyManager;
    public Portal portalManager;
    public LevelManager levelManager;
    public MenuManager menuManager;
    public LoadSceneHelper loadScene;

    [Header("References UI")]
    public GameObject UIMenu;

    [Space()]
    [Header("Game")]
    public int level = 1;
    public int score = 0;
    public int lifes = 3;

    // level
    public float levelProgression = .2f;

    // Progression Base Enemies
    public float enemiesDamageProgression = .2f;
    public float enemiesXpProgression = .3f;
    public float enemiesFireRateProgression = .2f;

    [Space()]
    [Header("Callback Game Flow")]
    public UnityEvent OnPauseEvent;
    public UnityEvent OnResumeEvent;
    public UnityEvent OnNextLevel;
    public UnityEvent OnFinishGame;
    public UnityEvent OnResetGame;

    [Header("Callback UI")]
    public UnityEvent<int> OnChangeScoreUI;
    public UnityEvent<int> OnChangeLevelUI;

    private bool _isPause = false;

    public void SetScore(int value)
    {
        score += value;
        OnChangeScoreUI?.Invoke(score); // invoke ui callback
        OnNextLevel?.Invoke(); // invoke flow game callback
    }
    
    public void SetUpLevel()
    {
        level++;
        OnChangeLevelUI?.Invoke(level);
    }

    public void CalledPortalToExit()
    {
        portalManager.Spawn();
    }

    public void SetLife(int value)
    {
        lifes += value;
    }

    #region Flow Game
    public void Pause()
    {
        UIMenu.SetActive(true);

        menuManager.GenerateMenu();
        OnPauseEvent?.Invoke();
    }

    public void Resume()
    {
        UIMenu.SetActive(false);

        menuManager.DestroyMenu();
        OnResumeEvent?.Invoke();
    }

    public void FinishGame()
    {
        if (lifes == 0)
            OnFinishGame?.Invoke();
    }

    #endregion

    #region --- Unity Events ---
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            _isPause = !_isPause;

            if (_isPause)
                Pause();
            else
                Resume();
        }
    }
    #endregion
}
