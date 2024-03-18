using UnityEngine;

public class InteractiveBarier : MonoBehaviour
{
    [SerializeField] private UITweenAnimation _tweenAnimation; 

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(ObjTags.player_tag)) return;

        _tweenAnimation.PlayePosTween();
    }
}
