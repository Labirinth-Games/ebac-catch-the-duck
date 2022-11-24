using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashScreenManager : MonoBehaviour
{
    public float timeToExit = 2f;

    IEnumerator WaitToExit()
    {
        yield return new WaitForSeconds(timeToExit);
        SceneManager.LoadScene(1); // called scene 1 (menu initial)
    }

    private void Start()
    {
        StartCoroutine(WaitToExit());
    }
}
