using UnityEngine;

public class LevelContanier : MonoBehaviour
{
    [SerializeField] private GameObject _jumpers;
    [SerializeField] private GameObject _boosters;
    [SerializeField] private GameObject _bariers;
    [SerializeField] private GameObject _interactiveBariers;

    private void Start()
    {
        SetActiveInteractiveObjects();
    }
    public void SetActiveInteractiveObjects()
    {
        bool hasJumper = Random.Range(0, 2) == 1 ? true : false;
        bool hasBooster = Random.Range(0, 2) == 1 ? true : false;
        bool hasBarier = Random.Range(0, 2) == 1 ? true : false;
        bool interactiveBarier = Random.Range(0, 3) == 1 ? true : false;

        if (_jumpers != null)
            _jumpers.SetActive(hasJumper);

        if (_boosters != null)
            _boosters.SetActive(hasBooster);

        if (_bariers != null)
            _bariers.SetActive(hasBarier);

        if (_interactiveBariers != null)
            _interactiveBariers.SetActive(interactiveBarier);
    }
}
