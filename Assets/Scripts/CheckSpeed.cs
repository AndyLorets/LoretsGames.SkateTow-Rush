using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckSpeed : MonoBehaviour
{
    private Player _player;

    private const float min_scoreSpeed = 25f; 

    private void Start()
    {
        _player = ServiceLocator.GetService<Player>();   
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(ObjTags.player_tag)) return;

        if (_player.SpeedScore > min_scoreSpeed)
            ScoreManager.AddScore(_player.SpeedScore, transform.position);
    }
}
