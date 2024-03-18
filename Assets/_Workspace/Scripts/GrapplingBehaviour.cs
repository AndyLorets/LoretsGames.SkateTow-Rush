using UnityEngine;

public static class GrapplingBehaviour
{
    private static Transform _grappleTransform;
    private static Vector3 _grappleOffset; 
    public static bool isGrappling { get; private set; }

    public static void StartGrapple(Transform grappleTransform, Vector3 offset)
    {
        _grappleTransform = grappleTransform;
        _grappleOffset = offset;
        isGrappling = true;

        AudioManager.PlayOneShot(AudioManager.SoundType.Grapple); 
    }
    public static Vector3 GetDirection()
    {
        return _grappleTransform.position + _grappleOffset; 
    }
    public static void StopGrapple()
    {
        isGrappling = false;
    }
}
