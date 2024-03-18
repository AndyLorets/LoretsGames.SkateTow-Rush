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
    [SerializeField] private GameObject _interactiveBariers; 
    [Space(10)]
    [SerializeField] private ParticleSystem _finishEffects;

    public void SetActiveInteractiveObjects(bool hasJumper, bool hasBooster, bool hasBarier, bool interactiveBarier, bool hasAirplanes, bool hasPicked, bool checkPoint)
    {
        if (_finish != null)
            _finish.SetActive(isFinishContanier);

        if (_airplanes != null)
            _airplanes.SetActive(hasAirplanes);

        if (_checkPoint != null)
            _checkPoint.SetActive(checkPoint);

        if (_jumpers != null)
            _jumpers.SetActive(hasJumper);

        if (_boosters != null)
            _boosters.SetActive(hasBooster);

        if (_bariers != null)
            _bariers.SetActive(hasBarier);

        if (_interactiveBariers != null)
            _interactiveBariers.SetActive(interactiveBarier);

        if (_pickeds != null)
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
