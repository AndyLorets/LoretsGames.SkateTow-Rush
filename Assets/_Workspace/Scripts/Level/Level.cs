using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    [SerializeField, Min(5)] private int _startLevelTime = 15;
    [SerializeField, Min(20)] private int _carsSpeed = 20;
    [SerializeField] private Transform _carsParent; 
    private Car[] _cars;
    [field: SerializeField] public LevelTimeInfo LevelTimeInfo { get; private set; }
    [Space(7)]
    [SerializeField] private Transform _levelContaniersParent;
    [SerializeField] private float _contanierOffset = 216.5f;
    [SerializeField] private LevelContanierInfo[] _levelContaniersInfo;

    private List<LevelContanier> _levelContanierList = new List<LevelContanier>();

    private int _currentLevelContanier = 0;
    private int _checkPointCallCount = 0;

    private const int Interval_ÑheckPointCallCount = 2;

    private void Start()
    {
        TimerManager.Init(_startLevelTime);
        ConstructCars();
        ConstructLevelContaniers();

        if (_levelContaniersInfo.Length % 2 == 0 && GameManager.DEBBUG_WARNINGLOG)
            Debug.LogWarning("The length of <color=yellow>LevelContanierInfo</color> should be odd!");
    }

    private void ConstructLevelContaniers()
    {
        for (int i = 0; i < _levelContaniersParent.childCount; i++)
            _levelContaniersParent.GetChild(i).gameObject.SetActive(false);

        for (int i = 0; i < _levelContaniersInfo.Length; i++)
        {
            GameObject levelContanierObj = Instantiate(_levelContaniersInfo[i].Contanier, _levelContaniersParent).gameObject;
            levelContanierObj.transform.localPosition = Vector3.zero;
            levelContanierObj.transform.name = $"Level Contanier {i + 1}"; 

            if (_levelContanierList.Count > 0)
            { 
                levelContanierObj.transform.localPosition = new Vector3(0, 0, _levelContanierList[i - 1].transform.position.z + _contanierOffset);
                if(i > 1)
                    levelContanierObj.SetActive(false);
            }

            LevelContanier levelContanier = levelContanierObj.GetComponent<LevelContanier>();
            bool notHaveCheckPoint = i % Interval_ÑheckPointCallCount == 0; 
            levelContanier.isFinishContanier = i == _levelContaniersInfo.Length - 1 ? true : false;
            levelContanier.onLevelEnter += CheckPoint;
            levelContanier.SetActiveInteractiveObjects(_levelContaniersInfo[i].HasJumper, _levelContaniersInfo[i].HasBooster,
                _levelContaniersInfo[i].HasBarier, _levelContaniersInfo[i].HasInteractiveBarier, _levelContaniersInfo[i].HasAirplane, _levelContaniersInfo[i].HasPicked, !notHaveCheckPoint);
            _levelContanierList.Add(levelContanier);
        }
    }
    private void ConstructCars()
    {
        _cars = new Car[_carsParent.childCount]; 
        for (int i = 0; i < _cars.Length; i++)
        {
            _cars[i] = _carsParent.GetChild(i).GetComponent<Car>();
            _cars[i].speed = _carsSpeed; 
        }
    }
    private void CheckPoint()
    {
        _checkPointCallCount++;
        if (_checkPointCallCount % Interval_ÑheckPointCallCount == 0)
            TimerManager.AddTime();

        EnableNearRoads();
    }
    private void EnableNearRoads()
    {
        if (_currentLevelContanier > 0)
            _levelContanierList[_currentLevelContanier - 1].gameObject.SetActive(false);

        _levelContanierList[_currentLevelContanier].gameObject.SetActive(true);
        _levelContanierList[_currentLevelContanier + 2].gameObject.SetActive(true);

        if (_currentLevelContanier < _levelContanierList.Count - 3)
            _currentLevelContanier++;
    }
    private void OnDestroy()
    {
        for (int i = 0; i < _levelContanierList.Count; i++)
        {
            _levelContanierList[i].onLevelEnter -= CheckPoint;
        }
    }

}
[System.Serializable]
public struct LevelContanierInfo
{
    [field: SerializeField] public LevelContanier Contanier { get; private set; }
    [field: SerializeField] public bool HasJumper { get; private set; } 
    [field: SerializeField] public bool HasBooster { get; private set; } 
    [field: SerializeField] public bool HasBarier { get; private set; }
    [field: SerializeField] public bool HasInteractiveBarier { get; private set; }
    [field: SerializeField] public bool HasAirplane { get; private set; }
    [field: SerializeField] public bool HasPicked { get; private set; }
}
[System.Serializable]
public struct LevelTimeInfo
{
    [Min(8)] public int goldTime;
    [Min(10)] public int silverTime;
    [Min(12)] public int bronzeTime;
}

