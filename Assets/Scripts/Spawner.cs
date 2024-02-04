using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject _spawnObj;
    [SerializeField, Range(5, 25)] private float _spawnInterval = 5;

    private int _currentObjIndex = 0; 
    private List <GameObject> _objList = new List<GameObject>();

    private const int pool_count = 10;  
    void Start()
    {
        Construct(); 
        StartCoroutine(SpawnCoroutine());
    }
    private void Construct()
    {
        for (int i = 0; i < pool_count; i++)
        {
            GameObject spawnObj = Instantiate(_spawnObj);
            spawnObj.transform.position = transform.position;
            spawnObj.SetActive(false);
            _objList.Add(spawnObj); 
        }

        _objList[_currentObjIndex].SetActive(true);    
    }
    private IEnumerator SpawnCoroutine()
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(_spawnInterval);
        while (_currentObjIndex < pool_count)
        {
            yield return waitForSeconds;
            _currentObjIndex++; 
            _objList[_currentObjIndex].SetActive(true);    
        }
    }
}
