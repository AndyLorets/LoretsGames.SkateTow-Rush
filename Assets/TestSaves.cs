using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class TestSaves : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _debbugText;

    public void DeleteAll()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void Save()
    {
        GameDataManager.Save();       

        string debbugText = $"{PlayerPrefs.GetInt("FFF")} Save Json: \n {GameDataManager.GetJson()}";
        _debbugText.text = debbugText;
    }
    public void Load()
    {
        GameDataManager.Load(); 

        string debbugText = $"Saved - {PlayerPrefs.GetInt("FFF")} | Load Json: \n {GameDataManager.GetJson()}";
        _debbugText.text = debbugText;
    }
}