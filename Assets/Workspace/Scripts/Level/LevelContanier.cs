using System;
using UnityEngine;

public class LevelContanier : MonoBehaviour
{
    public bool isFinishContanier { get; set; } 
    public Action onLevelEnter;

    [SerializeField] private GameObject _finish;
    [SerializeField] private GameObject _jumper;
    [SerializeField] private GameObject _booster;
    [SerializeField] private GameObject _barier;
    [SerializeField] private GameObject _airplanes;
    [SerializeField] private GameObject _coins; 
    [Space(5)]
    [SerializeField] private ParticleSystem _finishEffects;

    public void SetActiveInteractiveObjects(bool hasJumper, bool hasBooster, bool hasBarier, bool hasAirplanes, bool hasCoins)
    {
        _finish.SetActive(isFinishContanier);
        _jumper.SetActive(hasJumper);
        _booster.SetActive(hasBooster);
        _barier.SetActive(hasBarier);
        _airplanes.SetActive(hasAirplanes);
        _coins.SetActive(hasCoins);
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
