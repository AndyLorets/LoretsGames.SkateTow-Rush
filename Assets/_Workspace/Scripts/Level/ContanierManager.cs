using System;
using System.Collections.Generic;
using UnityEngine;

public class ContanierManager : MonoBehaviour
{
    private List<LevelContanier> _levelContanierList = new List<LevelContanier>();
    private Player _player; 

    private int _currentContanierIndex;
    private float _playerContanierDist;

    public static Action onReset; 

    private const int CONTANIER_INDEX_INTERVAL = 2; 
    private const float CONTANIER_INTERVAL = 217f;
    public const float LEVEL_DIST = 4314f;
    private const float PLAYER_DIST_OFFSET = 1f;

    private void Awake()
    {
        ServiceLocator.RegisterService(this);  
    }
    private void Start()
    {
        Construt(); 
    }
    private void SetPlayerContanierDist()
    {
        _playerContanierDist = _player.transform.position.z + (CONTANIER_INTERVAL * CONTANIER_INDEX_INTERVAL + PLAYER_DIST_OFFSET);
    }
    private void Construt()
    {
        _player = ServiceLocator.GetService<Player>();
        SetPlayerContanierDist(); 

        for (int i = 0; i < transform.childCount; i++)
        {
            LevelContanier levelContanier = transform.GetChild(i).GetComponent<LevelContanier>();
            levelContanier.gameObject.SetActive(false); 
            _levelContanierList.Add(levelContanier);
        }

        ActiveNearContaniers(); 
    }
    private void Update()
    {
        bool isContaniersActive = _player.transform.position.z >= _playerContanierDist;
        bool isRespawn = _player.transform.position.z >= LEVEL_DIST;

        if (isContaniersActive)
        {
            SetContaniersActive();

        }
        if (isRespawn)
        {
            RespawnAfterEnd(); 
        }
    }
    private void SetContaniersActive()
    {
        SetPlayerContanierDist();

        if (_currentContanierIndex < _levelContanierList.Count - (CONTANIER_INDEX_INTERVAL + 1))
            _currentContanierIndex += CONTANIER_INDEX_INTERVAL;

        for (int i = 0; i < _currentContanierIndex; i++)
        {
            _levelContanierList[i].gameObject.SetActive(false);
        }

        ActiveNearContaniers();
    }
    private void ActiveNearContaniers()
    {
        for (int i = _currentContanierIndex; i < _currentContanierIndex + CONTANIER_INDEX_INTERVAL + 1; i++)
        {
            if (i < _levelContanierList.Count)
                _levelContanierList[i].gameObject.SetActive(true);
        }
    }
    private void RespawnAfterEnd()
    {
        _player.RespawnAfterEnd();
        ResetAllContaniers();
        SetPlayerContanierDist();

        if (Time.timeScale < 1.5f)
            Time.timeScale += .1f; 
    }
    private void ResetAllContaniers()
    {
        _currentContanierIndex = 0;
        onReset?.Invoke();

        for (int i = 0; i < _levelContanierList.Count; i++)
        {
            _levelContanierList[i].gameObject.SetActive(false);
        }

        ActiveNearContaniers();
    }
    private void sort()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform current = transform.GetChild(i);
            Transform forward = transform.GetChild(i + 1);
            forward.position = current.position + Vector3.forward * CONTANIER_INTERVAL; 
        }
    }
    private void OnValidate()
    {
        //sort(); 
    }
}

[System.Serializable]
public struct LevelTimeInfo
{
    [Min(8)] public int goldTime;
    [Min(10)] public int silverTime;
    [Min(12)] public int bronzeTime;
}
