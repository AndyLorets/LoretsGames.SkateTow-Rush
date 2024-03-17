using UnityEngine;
using DG.Tweening; 

public class HookObject : MonoBehaviour
{
    //[SerializeField] private bool _prefabAfterHooking; 
    //[SerializeField] private GetPrefabAfterHooking _getPrefabAfterHooking;

    private Player _player;
    private PlayerHookController _playerHookController;

    private const float max_hook_distance = 200f;
    private bool CanHook => Vector3.Distance(transform.position, _player.transform.position) < max_hook_distance; 


    private void Start()
    {
        //Construct(); 
    }
    //private void Construct()
    //{
    //    _player = ServiceLocator.GetService<Player>();
    //    _playerHookController = ServiceLocator.GetService<PlayerHookController>();
    //    //if (_prefabAfterHooking)
    //    //    _getPrefabAfterHooking.ConstructLevelContaniers(this); 
    //}
    //private void OnMouseDown()
    //{
    //    if (CanHook)
    //    {
    //        //_playerHookController.SetHookObje(transform);
    //    }
    //}
    //private void OnMouseUp()
    //{
    //    _playerHookController.ClearHookObj();
    //}

}
//[System.Serializable]
//public class GetPrefabAfterHooking 
//{
//    [SerializeField] private GameObject _prefab;
//    [SerializeField] private Transform _prefabParent;

//    private HookObject _hookObject;

//    public void ConstructLevelContaniers(HookObject _hookObject)
//    {
//        _prefab = GameObject.Instantiate(_prefab, _prefabParent);
//        _prefab.transform.localPosition = Vector3.zero;
//        _prefab.transform.localRotation = Quaternion.identity; 
//        _prefab.SetActive(false);

//        _hookObject = _hookObject;
//        _hookObject.onHook += DropPrefab;
//    }
    
//    private void DropPrefab()
//    {
//        Vector3 dropPos = new Vector3(_prefabParent.position.x, -1, _prefabParent.position.z);
//        dropPos.z -= 10f;

//        _prefab.SetActive(true);
//        _prefab.transform.parent = null;
//        _prefab.transform.localScale = Vector3.one * 7f;
//        _prefab.transform.DOScale(new Vector3(1.5f, 1.5f, 1.5f), 1.5f)
//            .OnComplete(() => _prefab.SetActive(false));
//        _prefab.transform.DOJump(dropPos, 3, 1, .5f)
//            .SetEase(Ease.Linear)
//            .SetUpdate(UpdateType.Fixed);
//        _hookObject.onHook -= DropPrefab;
//    }
//}

