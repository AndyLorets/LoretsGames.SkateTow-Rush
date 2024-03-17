using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirplaneManager : MonoBehaviour
{
    [SerializeField] private Airplane _airplanePrefab;
    [Space(5)]
    [SerializeField, Range(10, 20)] private int _airplaneLifeDuration = 12;
    [SerializeField, Range(4, 10)] private int _spawnDelay = 5;

    private const int pool_count = 20;

    private List<Airplane> _airplanesList = new List<Airplane>();

    void Start()   
    {
        gameObject.SetActive(false);

        //ConstructAirplanes();
        //StartCoroutine(AirplaneActivizator());  
    }

    private void ConstructAirplanes()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Airplane airplane = transform.GetChild(i).GetComponent<Airplane>();
            if (airplane == null) return;

            airplane.lifeMaxDuration = _airplaneLifeDuration;
            airplane.transform.name = $"Airplane {_airplanesList.Count + 1}";

            _airplanesList.Add(airplane);
        }

        for (int i = 0; i < pool_count; i++)
        {
            Airplane airplane = Instantiate(_airplanePrefab, transform).gameObject.GetComponent<Airplane>();   
            if (airplane == null) return;

            airplane.lifeMaxDuration = _airplaneLifeDuration;
            airplane.name = $"Airplane {_airplanesList.Count + 1}";
            airplane.transform.transform.localPosition = Vector3.zero;
            airplane.gameObject.SetActive(false);

            _airplanesList.Add(airplane);
        }
    }

    private IEnumerator AirplaneActivizator()
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(_spawnDelay); 
        while (true)
        {
            GetDeactivePlane()?.gameObject.SetActive(true);

            yield return waitForSeconds;  
        }

    }

    private GameObject GetDeactivePlane()
    {
        for (int i = 0; i < _airplanesList.Count; i++)
            if (!_airplanesList[i].gameObject.activeSelf)
                return _airplanesList[i].gameObject;

        return null; 
    }
}
