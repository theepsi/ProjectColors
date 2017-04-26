using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (Controller2D))]
public class Player : MonoBehaviour {


    public float JumpHeight = 4;
    public float TimeToJumpApex = .4f;

    float _accelerationTimeAirborne = .2f;
    float _accelerationTimeGrounded = .1f;

    float _moveSpeed = 6;
    float _velocityXSmoothing;

    float _jumpVelocity;
    float _gravity;
    Vector3 _velocity;

    Controller2D _controller;

	void Start () {
        _controller = GetComponent<Controller2D>();

        _gravity = -(2 * JumpHeight) / Mathf.Pow(TimeToJumpApex, 2);
        _jumpVelocity = Mathf.Abs(_gravity) * TimeToJumpApex;
        print("Gravity: " + _gravity + " Jump Velocity: " + _jumpVelocity);
	}

    void Update()
    {
        if (_controller.Collisions.Above || _controller.Collisions.Below)
            _velocity.y = 0;

        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (Input.GetButtonDown("Jump") && _controller.Collisions.Below)
        {
            _velocity.y = _jumpVelocity;
        }

        float targetVelocityX = input.x * _moveSpeed;

        _velocity.x = Mathf.SmoothDamp(_velocity.x, targetVelocityX, ref _velocityXSmoothing, (_controller.Collisions.Below) ? _accelerationTimeGrounded : _accelerationTimeAirborne);
        _velocity.y += _gravity * Time.deltaTime;
        _controller.Move(_velocity * Time.deltaTime);
    }
}
