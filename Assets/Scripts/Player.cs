using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (Controller2D))]
public class Player : MonoBehaviour {


    public float JumpHeight = 4;
    public float TimeToJumpApex = .4f;

    bool _carryingObject = false;
    PickableObject _objectCarried;

    float _accelerationTimeAirborne = .2f;
    float _accelerationTimeGrounded = .1f;

    float _moveSpeed = 6;
    float _velocityXSmoothing;

    float _jumpVelocity;
    float _gravity;
    Vector3 _velocity;

    Controller2D _controller;

    Vector2 _directionalInput;
    GameManager _gameManager;

    [HideInInspector] public bool FacingRight = true;

    void Start () {
        _gameManager = FindObjectOfType<GameManager>();
        _controller = GetComponent<Controller2D>();

        _gravity = -(2 * JumpHeight) / Mathf.Pow(TimeToJumpApex, 2);
        _jumpVelocity = Mathf.Abs(_gravity) * TimeToJumpApex;
        print("Gravity: " + _gravity + " Jump Velocity: " + _jumpVelocity);
	}

    void Update()
    {
        CalculateVelocity();

        _controller.Move(_velocity * Time.deltaTime, _directionalInput);

        if (_controller.Collisions.Above || _controller.Collisions.Below)
            _velocity.y = 0;

        if (_directionalInput.x > 0 && !FacingRight)
            Flip();
        else if (_directionalInput.x < 0 && FacingRight)
            Flip();
    }

    public void SetDirectionalInput(Vector2 input)
    {
        _directionalInput = input;
    }

    public void OnJumpInput()
    {
        if (_controller.Collisions.Below)
        {
            _velocity.y = _jumpVelocity;
        }
    }

    public void OnInteract()
    {
        if (!_carryingObject)
        {
            // Check if can be next object
            Vector3 pickupPosOrigin = new Vector3(transform.position.x, transform.position.y - .5f, transform.position.z);
            RaycastHit2D hit = Physics2D.Raycast(pickupPosOrigin, Vector2.right * transform.localScale.x, 2f, 1 << LayerMask.NameToLayer("PickableObject"));
            Debug.DrawRay(pickupPosOrigin, Vector2.right * transform.localScale.x, Color.green);

            if (hit)
            {
                _objectCarried = hit.transform.GetComponent<PickableObject>();
                _objectCarried.SetPickedUp(true);
                _carryingObject = true;
            }
        }
        else
        {
            _carryingObject = false;
            _objectCarried.SetPickedUp(false);
            _objectCarried = null;
        }
    }

    void CalculateVelocity()
    {
        float targetVelocityX = _directionalInput.x * _moveSpeed;
        _velocity.x = Mathf.SmoothDamp(_velocity.x, targetVelocityX, ref _velocityXSmoothing, (_controller.Collisions.Below) ? _accelerationTimeGrounded : _accelerationTimeAirborne);
        _velocity.y += _gravity * Time.deltaTime;
    }

    public void KillPlayer()
    {
        _gameManager.RestartLevel();
    }

    void Flip()
    {
        FacingRight = !FacingRight;
        Vector3 vScale = transform.localScale;
        vScale.x *= -1;
        transform.localScale = vScale;
    }
}
