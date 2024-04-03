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

    private Player _player;
    private bool _pointChecked; 

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
    public void Construct()
    {
        _player = ServiceLocator.GetService<Player>();  

        if (isFinishContanier)
            ServiceLocator.GetService<LevelManager>().FinishTransform = _finish.transform;
    }
    private void Update()
    {
        if (_player.transform.position.z >= _finish.transform.position.z && !_pointChecked)
        {
            onLevelEnter?.Invoke();

            if (isFinishContanier)
            {
                _finishEffects.Play();
                GameManager.Finish();
            }

            _pointChecked = true;

            if (GameManager.DEBBUG_LOG)
                Debug.Log($"OnLevelEnter <color=#10FFFF>LevelContanier</color>\"");
        }
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (!other.CompareTag(ObjTags.player_tag)) return;

    //}
}
