using UnityEngine;

public class DropItemOnTrigger : MonoBehaviour
{
    [SerializeField] private GameObject[] _dropSoursePrefab;

    public System.Action onDroped; 

    private void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
            transform.GetChild(i).gameObject.SetActive(false);

        int r = Random.Range(0, 5); 
        if(r == 2)
            ConstructDroppables();
    }
    private void ConstructDroppables()
    {
        int randomDrop = Random.Range(0, _dropSoursePrefab.Length);
        int childCount = transform.childCount; 
        for (int i = 0; i < childCount; i++)
        {
            GameObject dropObject = Instantiate(_dropSoursePrefab[randomDrop], transform);
            dropObject.transform.localPosition = transform.GetChild(i).localPosition; 
            dropObject.transform.localRotation = Quaternion.identity;
        }
    }


}
