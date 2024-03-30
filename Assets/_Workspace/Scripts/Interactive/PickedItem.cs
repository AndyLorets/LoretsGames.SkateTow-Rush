using UnityEngine;

public class PickedItem : MonoBehaviour
{
    [SerializeField] private GameObject _coins;
    [SerializeField] private GameObject _gems;
    [SerializeField] private GameObject _keys;
    void Start()
    {
        _coins.gameObject.SetActive(false);
        _gems.gameObject.SetActive(false);
        _keys.gameObject.SetActive(false);

        Construct();
    }

    private void Construct()
    {
        int r;
        int randomNumber = Random.Range(0, 10);

        if (randomNumber < 2)
            r = 2;
        else if (randomNumber < 6)
            r = 1;
        else
            r = 0;

        switch (r)
        {
            case 0:
                _coins.gameObject.SetActive(true);
                break;
            case 1:
                _gems.gameObject.SetActive(true);
                break;
            case 2:
                if (!GameManager.showKeyPicked)
                {
                    _keys.gameObject.SetActive(true);
                    GameManager.showKeyPicked = true; 
                }
                else
                    Construct(); 
                break;
        }
    }

}
