using UnityEngine;

public class SetObjectInLane : MonoBehaviour 
{
    [SerializeField] private bool _random; 
    [SerializeField, Range(min_line_count, max_line_count)] private int _lanePos = 1;

    private const int min_line_count = 1;
    private const int max_line_count = 4;
    private const float start_line_posX = -5f;//-8;
    private const float dist_btwn_lane = 5f;//6.7f;

    private void Start()
    {
        transform.position = GetLanePosition();
    }
    private Vector3 GetLanePosition()
    {
        Vector3 pos = transform.position;
        int line = _random ? Random.Range(min_line_count, max_line_count) : _lanePos;
        float x = start_line_posX - dist_btwn_lane;
        x += dist_btwn_lane * line;
        pos = new Vector3(x, pos.y, pos.z);
        return pos;
    }
    private void OnValidate()
    {
        transform.position = GetLanePosition();
    }
}
