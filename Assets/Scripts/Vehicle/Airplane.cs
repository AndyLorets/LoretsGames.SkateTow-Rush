using System.Collections;
using UnityEngine;

public class Airplane : VehicleBase
{
    [SerializeField] protected float _speed = 5f;
    [SerializeField] private Transform _par;

    private const float propeller_rotate_speed = 300; 
    public float lifeMaxDuration { private get; set; }

    protected virtual void FixedUpdate()
    {
        transform.Translate(Vector3.forward * _speed * Time.deltaTime);
    }
    private void OnEnable()
    {
        StartCoroutine(OnAliveState()); 
    }
    private void OnDisable()
    {
        StopAllCoroutines();  
    }
    private IEnumerator OnAliveState()
    {
        float delay = lifeMaxDuration == 0 ? 7 : lifeMaxDuration;
        yield return new WaitForSeconds(delay);

        Restart();
    }
    protected virtual void Update()
    {
        _par.Rotate(new Vector3(0, 0, 5) * Time.deltaTime * propeller_rotate_speed);
    }
    protected override void Restart()
    {
        base.Restart();
        gameObject.SetActive(false); 
    }
}
