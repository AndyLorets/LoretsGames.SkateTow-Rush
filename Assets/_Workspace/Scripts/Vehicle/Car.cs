using UnityEngine;

public class Car : VehicleBase
{
    [SerializeField] private Transform _skinsParent;
    [SerializeField] private AudioSource _audio;

    private GameObject[] _skins;
    private BoxCollider _collder;
    public float speed { get; set; } = 20f;
    public float maxPosZ { private get; set; }

    private void Awake()
    {
        AudioManager.OnChangeState += ChangeSoundState; 
    }
    private void OnDestroy()
    {
        AudioManager.OnChangeState -= ChangeSoundState;
    }
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
        _audio.pitch = Random.Range(1.1f, 1.4f);
        _audio.mute = AudioManager.Mute; 
    }
    private void ChangeSoundState()
    {
        _audio.mute = AudioManager.Mute;
    }
    protected virtual void FixedUpdate()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
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
