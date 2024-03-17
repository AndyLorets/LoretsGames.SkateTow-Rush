using System;
using UnityEngine;

public class LevelContanier : MonoBehaviour
{
    public bool isFinishContanier { get; set; } 
    public Action onLevelEnter;

    [SerializeField] private GameObject _finish;
    [SerializeField] private GameObject _airplanes;
    [SerializeField] private GameObject _checkPoint;
    [SerializeField] private GameObject _jumpers;
    [SerializeField] private GameObject _boosters;
    [SerializeField] private GameObject _bariers;
    [SerializeField] private GameObject _pickeds;
    [Space(10)]
    [SerializeField] private ParticleSystem _finishEffects;

    public void SetActiveInteractiveObjects(bool hasJumper, bool hasBooster, bool hasBarier, bool hasAirplanes, bool hasPicked, bool checkPoint)
    {
        _finish.SetActive(isFinishContanier);
        _airplanes.SetActive(hasAirplanes);
        _checkPoint.SetActive(checkPoint);
        _jumpers.SetActive(hasJumper);
        _boosters.SetActive(hasBooster);
        _bariers.SetActive(hasBarier);
        _pickeds.SetActive(hasPicked);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(ObjTags.player_tag)) return;

        onLevelEnter?.Invoke();

        if (isFinishContanier)
        {
            _finishEffects.Play();
            GameManager.Finish();
        }
    }
}
