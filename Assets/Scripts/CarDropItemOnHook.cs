using UnityEngine;
using System.Collections.Generic;

public class CarDropItemOnHook : MonoBehaviour
{
    [SerializeField] private HookObject _hookObject;
    [SerializeField] private GameObject[] _dropSoursePrefab;

    private List<IDroppable> _droppablesList = new List<IDroppable>();   

    public System.Action onDroped; 

    private bool _isHooking;
    private float _dropSpawnTimer = drop_spawn_timer;

    private const float drop_spawn_timer = .15f;
    private const int drop_count = 5;

    private void OnEnable() => _hookObject.onHook += OnHook;
    private void OnDisable() => _hookObject.onHook -= OnHook;
    private void Start()
    {
        int r = Random.Range(0, 5); 
        if(r == 2)
            ConstructDroppables();
    }
    private void Update() => Run();
    private void ConstructDroppables()
    {
        int randomDrop = Random.Range(0, _dropSoursePrefab.Length);

        for (int i = 0; i < drop_count; i++)
        {
            bool dropActive = i == 0 ? true : false; 
            GameObject dropObject = Instantiate(_dropSoursePrefab[randomDrop], transform);
            IDroppable droppable = dropObject.GetComponent<IDroppable>(); 
            dropObject.transform.localPosition = new Vector3(0.0f, 5.0f, -2.0f);
            dropObject.transform.localRotation = Quaternion.identity;
            _droppablesList.Add(droppable);
            _droppablesList[i].Construct(dropActive); 
        }
    }
    private void Run()
    {
        if (!_isHooking) return;

        if (!GrapplingBehaviour.isGrappling && _isHooking)
        {
            _isHooking = false;
            _dropSpawnTimer = drop_spawn_timer;
        }

        _dropSpawnTimer -= Time.deltaTime;
        if (_dropSpawnTimer <= 0)
            GetDrop();
    }
    private void OnHook()
    {
        _dropSpawnTimer = drop_spawn_timer;
        _isHooking = true;
    }

    private void GetDrop()
    {
        if (_droppablesList.Count == 0)
        {
            enabled = false;
            return;
        }

        _droppablesList[0].MoveToPlayer();
        _droppablesList.RemoveAt(0);
        if (_droppablesList.Count != 0)
            _droppablesList[0].Active(); 

        _dropSpawnTimer = drop_spawn_timer;
        onDroped?.Invoke(); 
    }
}
