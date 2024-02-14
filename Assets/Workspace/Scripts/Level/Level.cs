using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    [SerializeField, Min(5)] private int _startLevelTime = 15;
    [SerializeField, Min(5)] private int _addLevelTime = 5;
    [Space(7)]
    [SerializeField] private Transform _levelContaniersParent; 
    [SerializeField] private float _contanierOffset = 216.5f;
    [Space(7)]
    [SerializeField] private LevelContanierInfo[] _levelContaniers;

    private List<LevelContanier> _levelContanierList = new List<LevelContanier>();

    private int _currentLevelContanier = 0;
    private int _checkPointCallCount = 0;

    private const int Interval_ÑheckPointCallCount = 2;

    private void Start()
    {
        TimerManager.AddTime(_startLevelTime);
        ConstructLevelContaniers();
    }

    private void ConstructLevelContaniers()
    {
        for (int i = 0; i < _levelContaniersParent.childCount; i++)
            _levelContaniersParent.GetChild(i).gameObject.SetActive(false);

        for (int i = 0; i < _levelContaniers.Length; i++)
        {
            GameObject levelContanierObj = Instantiate(_levelContaniers[i].Contanier, _levelContaniersParent).gameObject;
            levelContanierObj.transform.localPosition = Vector3.zero;
            levelContanierObj.transform.name = $"Level Contanier {i + 1}"; 

            if (_levelContanierList.Count > 0)
            { 
                levelContanierObj.transform.localPosition = new Vector3(0, 0, _levelContanierList[i - 1].transform.position.z + _contanierOffset);
                if(i > 1)
                    levelContanierObj.SetActive(false);
            }

            LevelContanier levelContanier = levelContanierObj.GetComponent<LevelContanier>();
            levelContanier.isFinishContanier = i == _levelContaniers.Length - 1 ? true : false;
            levelContanier.onLevelEnter += CheckPoint;
            levelContanier.SetActiveInteractiveObjects(_levelContaniers[i].HasJumper, _levelContaniers[i].HasBooster,
                _levelContaniers[i].HasBarier, _levelContaniers[i].HasAirplane, _levelContaniers[i].HasCoins);
            _levelContanierList.Add(levelContanier);
        }
    }
    private void CheckPoint()
    {
        _checkPointCallCount++;
        if (_checkPointCallCount % Interval_ÑheckPointCallCount == 0)
            TimerManager.AddTime(_addLevelTime);

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
public class LevelContanierInfo
{
    [field: SerializeField] public LevelContanier Contanier { get; private set; }
    [field: SerializeField] public bool HasJumper { get; private set; } = true; 
    [field: SerializeField] public bool HasBooster { get; private set; } = true;
    [field: SerializeField] public bool HasBarier { get; private set; } = true; 
    [field: SerializeField] public bool HasAirplane { get; private set; }
    [field: SerializeField] public bool HasCoins { get; private set; }
}

