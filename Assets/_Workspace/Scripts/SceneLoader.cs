using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    private static int gameScene_id = 1;

    private void OnEnable()
    {
        GameManager.onRestart += LoadGameScene;
    }
    private void OnDisable()
    {
        GameManager.onRestart -= LoadGameScene;
    }
    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        StartCoroutine(SceneLoading());
    }
    private void LoadGameScene()
    {
        StartCoroutine(SceneLoading());
    }
    private IEnumerator SceneLoading()
    {
        AsyncOperation async = SceneManager.LoadSceneAsync(gameScene_id);

        while (true)
        {
            if (async.isDone)
            {
                for (int i = 0; i < transform.childCount; i++)
                {
                    transform.GetChild(i).gameObject.SetActive(false); 
                }
            }    

            yield return null;
        }
    }
}
