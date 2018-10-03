using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerControl : PhysicalObject
{
    public float moveSpeed = 5F;
    public float jumpStrength = 5F;

    private Rigidbody2D _rigidbody;
    private bool _isJumping;
    private float _oldJumpingVelocity;

    public bool controlIsPressed(Control control)
    {
        return ControlManager.getInstance().controlIsPressed(control);
    }

    private void checkMovementInput()
    {
        if (controlIsPressed(Control.MoveLeft))
            _rigidbody.velocity = new Vector3(moveSpeed * -1, _rigidbody.velocity.y);
        else if (controlIsPressed(Control.MoveRight))
            _rigidbody.velocity = new Vector3(moveSpeed, _rigidbody.velocity.y);
        else
            _rigidbody.velocity = new Vector3(0, _rigidbody.velocity.y);
    }

    private void checkJumpingInput()
    {
        if (controlIsPressed(Control.Jump))
        {
            if (!_isJumping && isGrounded())
            {
                _isJumping = true;
                _rigidbody.velocity = new Vector3(_rigidbody.velocity.x, jumpStrength, 0);
            }
            if (_isJumping)
                _rigidbody.AddForce(Vector2.up * jumpStrength * Time.deltaTime * 50);
        }
        else if (_rigidbody.velocity.y <= 0)
            _isJumping = false;
    }

    private void checkInputs()
    {
        checkMovementInput();
        checkJumpingInput();
        if(transform.position.y < 0)
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        checkInputs();
        isGrounded();
    }

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        _rigidbody = gameObject.GetComponent<Rigidbody2D>();
        _rigidbody.freezeRotation = true;
    }
}
