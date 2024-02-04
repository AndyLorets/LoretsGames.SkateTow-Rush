using UnityEngine;

public class Jumper : MonoBehaviour
{
    [SerializeField] private ParticleSystem _effect;

    public void ActiveJumper()
    {
        _effect.Play();
    }
}
