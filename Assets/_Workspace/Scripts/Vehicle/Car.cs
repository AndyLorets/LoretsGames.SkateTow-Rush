using UnityEngine;

public class Car : VehicleBase
{
    [SerializeField] protected float _speed = 5f;
    [SerializeField] private Transform _skinsParent;

    private GameObject[] _skins;
    private BoxCollider _collder; 
    public float maxPosZ { private get; set; }

    protected override void Construct()
    {
        base.Construct();

        int skinCount = _skinsParent.childCount;
        _skins = new GameObject[skinCount];
        for (int i = 0; i < skinCount; i++)
        {
            _skins[i] = _skinsParent.GetChild(i).gameObject;
            _skins[i].SetActive(false);
        }

        int r = Random.Range(0, _skins.Length);
        _skins[r].SetActive(true);

        _collder = GetComponent<BoxCollider>();
    }

    protected virtual void FixedUpdate()
    {
        transform.Translate(Vector3.forward * _speed * Time.deltaTime);
    }
    protected virtual void Update()
    {
        if (transform.position.z > maxPosZ)
            Restart(); 
    }
    protected override void Restart()
    {
        base.Restart();

        _collder.enabled = true;
        for (int i = 0; i < transform.childCount; i++)
            transform.GetChild(i).gameObject.SetActive(true);
    }
}
