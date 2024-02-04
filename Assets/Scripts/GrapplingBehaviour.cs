using UnityEngine;

public static class GrapplingBehaviour
{
    public static Transform GrapplePoint { get; private set; }
    public static bool isGrappling { get; private set; }

    public static void StartGrapple(Transform grapplePoint)
    {
        GrapplePoint = grapplePoint;
        isGrappling = true; 
    }

    public static void StopGrapple()
    {
        isGrappling = false;
    }
}
