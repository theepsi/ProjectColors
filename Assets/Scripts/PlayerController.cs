using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float MoveForce = 365f;
    public float MaxSpeed = 5f;
    public float JumpForce = 1000f;
    public Transform GroundCheck;

    [HideInInspector] public bool Jump = false;
    [HideInInspector] public bool FacingRight = true;

    private bool grounded = false;

    private Rigidbody2D _rigidbody2d;
    private float inverseMoveTime;
    private GameManager _gameManager;

    // Use this for initialization
    void Awake () {
        _gameManager = FindObjectOfType<GameManager>();
        _rigidbody2d = GetComponent<Rigidbody2D>();
        Jump = false;
    }
	
	// Update is called once per frame
	void Update () {

        grounded = Physics2D.Linecast(transform.position, GroundCheck.position, 1 << LayerMask.NameToLayer("Ground"));

        if (Input.GetButtonDown("Jump") && grounded)
        {
            Jump = true;
        }
        
    }

    void FixedUpdate()
    {
        float horizontal = Input.GetAxis("Horizontal");

        //Anim set speed;

        if (horizontal * _rigidbody2d.velocity.x < MaxSpeed)
            _rigidbody2d.AddForce(Vector2.right * horizontal * MoveForce);

        if (Mathf.Abs(_rigidbody2d.velocity.x) > MaxSpeed)
            _rigidbody2d.velocity = new Vector2(Mathf.Sign(_rigidbody2d.velocity.x) * MaxSpeed, _rigidbody2d.velocity.y);

        if (horizontal > 0 && !FacingRight)
            Flip();
        else if (horizontal < 0 && FacingRight)
            Flip();

        if (Jump)
        {
            //anim set jump
            _rigidbody2d.AddForce(new Vector2(0f, JumpForce));
            Jump = false;
        }
    }

    void Flip()
    {
        FacingRight = !FacingRight;
        Vector3 vScale = transform.localScale;
        vScale.x *= -1;
        transform.localScale = vScale;
    }

    public void KillPlayer()
    {
        _gameManager.RestartLevel();
    }
}
