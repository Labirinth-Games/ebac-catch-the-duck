using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneHelper : MonoBehaviour
{
    public void SetScene(int i)
    {
        SceneManager.LoadScene(i); // called scene X
    }
}
