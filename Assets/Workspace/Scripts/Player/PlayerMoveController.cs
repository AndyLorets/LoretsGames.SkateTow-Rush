using UnityEngine;

public class PlayerMoveController : MonoBehaviour
{
    [SerializeField, Range(.2f, 1f)] private float _movementSpeed = 0.25f;
    [SerializeField, Range(7f, 10)] private float _maxJumpForce = 7.0f;

    [SerializeField] private float _currentJumpForce; 

    private Rigidbody _rb;
    private const float clamp_velocity_x = 10;

    private Player _player; 
    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }
    private void Start()
    {
        _player = ServiceLocator.GetService<Player>();  
    }
    private void Update()
    {
        MoveHorizonta(Input.GetAxis("Horizontal"));
        if (Input.GetKey(KeyCode.Space))
            PreJump();
        if (Input.GetKeyUp(KeyCode.Space))
            Jump();
    }
    private void MoveHorizonta(float horizontalInput)
    {
        Vector3 movement = new Vector3(horizontalInput * _movementSpeed, 0f, 0f);
        _rb.AddForce(movement, ForceMode.VelocityChange);
        ClampVelocity(); 
    }
    private void PreJump()
    {
        if (!_player.onGround || _currentJumpForce >= _maxJumpForce) return;

        _currentJumpForce += _maxJumpForce * Time.deltaTime;
    }
    private void Jump()
    {
        if (!_player.onGround) return;

        _rb.AddForce(Vector3.up * _currentJumpForce, ForceMode.Impulse);
        _currentJumpForce = 0; 
    }
    private void ClampVelocity()
    {
        float clampX = _rb.velocity.x;
        clampX = Mathf.Clamp(clampX, -clamp_velocity_x, clamp_velocity_x);
        _rb.velocity = new Vector3(clampX, _rb.velocity.y, _rb.velocity.z);
    }
}
