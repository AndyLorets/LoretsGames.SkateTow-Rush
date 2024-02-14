using UnityEngine;

public class CarsManager : MonoBehaviour
{
    [SerializeField] private float _maxPosZ = 1200; 
    void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {   
            Car car = transform.GetChild(i).GetComponent<Car>();
            if (car == null) return;

            car.maxPosZ = _maxPosZ;
        }
    }

}
