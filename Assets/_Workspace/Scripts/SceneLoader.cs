using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private float _timePreview = 3f;
    [SerializeField] private string[] _discriptions;
    [SerializeField] private TextMeshProUGUI _discriptionText;

    private static int gameScene_id = 1;

    private void OnEnable()
    {
        GameManager.onRestart += LoadGameScene;
        //GameManager.onRestartAfterFinish += LoadGameScene;  
    }
    private void OnDisable()
    {
        GameManager.onRestart -= LoadGameScene;
        //GameManager.onRestartAfterFinish -= LoadGameScene;
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
    private IEnumerator SceneLoading(bool renderDiscriptions = false)
    {
        //if (renderDiscriptions)
        //{
        //    List<string> discriptionsList = new List<string>();
        //    for (int i = 0; i < _discriptions.Length; i++)
        //        discriptionsList.Add(_discriptions[i]);

        //    RenderDiscription(ref discriptionsList);

        //    yield return new WaitForSeconds(_timePreview);

        //    RenderDiscription(ref discriptionsList);
        //}
        //else
        //    yield return new WaitForSeconds(GameManager.nextLevel_fade_duration);

        AsyncOperation async = SceneManager.LoadSceneAsync(gameScene_id);

        while (_discriptionText.enabled)
        {
            if (async.isDone)
            {
                for (int i = 0; i < transform.childCount; i++)
                {
                    transform.GetChild(i).gameObject.SetActive(false); 
                }
            }    

            //float progress = async.progress;
            //if (_loadImage != null && progress >= .3f) _loadImage.fillAmount = progress;
            yield return null;
        }
    }
    private void RenderDiscription(ref List<string> discriptionsList)
    {
        int r = Random.Range(0, discriptionsList.Count);
        if (discriptionsList[r] == null) return; 

        _discriptionText.text = "Help: " + discriptionsList[r];
        discriptionsList.RemoveAt(r);
    }
}
