using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float MoveForce = 4f;
    //public float MaxSpeed = 5f;
    public float JumpForce = 1000f;
    public Transform GroundCheck;
    public Transform WallCheck;
    public BoxCollider2D pickableRange;

    [HideInInspector] public bool Jump = false;
    [HideInInspector] public bool FacingRight = true;

    private bool _grounded = false;
    private bool _againstWall = false;
    private bool _canMove = true;
    private PickableObject _objectInRange = null;
    private PickableObject _pickedObject = null;

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

        _grounded = Physics2D.Linecast(transform.position, GroundCheck.position, 1 << LayerMask.NameToLayer("Ground"));

        Vector3 playerUpperTransform = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);

        _againstWall = Physics2D.Linecast(playerUpperTransform, WallCheck.position, 1 << LayerMask.NameToLayer("Ground"));

        if (Input.GetButtonDown("Jump") && _grounded)
        {
            Jump = true;
        }

        if (Input.GetButtonDown("Interact"))
        {
            if(_objectInRange != null)
                _pickedObject = _objectInRange;

            if(_pickedObject != null)
            {
                if (!_pickedObject.IsPicked())
                {
                    _pickedObject.SetPickedUp(true);
                    pickableRange.enabled = false;
                }
                else
                {
                    _pickedObject.SetPickedUp(false);
                    pickableRange.enabled = true;
                    _pickedObject = null;
                }
            }
        }
    }

    void FixedUpdate()
    {
        float horizontal = Input.GetAxis("Horizontal");

        //Anim set speed;

        if (_againstWall)
        {
            if ((FacingRight && horizontal > 0) || (!FacingRight && horizontal < 0))
                _canMove = false;
            else
                _canMove = true;
        }
        else
            _canMove = true;

        if (_canMove)
            _rigidbody2d.velocity = new Vector2(horizontal * MoveForce, _rigidbody2d.velocity.y);

        //if (horizontal * _rigidbody2d.velocity.x < MaxSpeed && _canMove)
        //    _rigidbody2d.AddForce(Vector2.right * horizontal * MoveForce);

            //if (Mathf.Abs(_rigidbody2d.velocity.x) > MaxSpeed)
            //    _rigidbody2d.velocity = new Vector2(Mathf.Sign(_rigidbody2d.velocity.x) * MaxSpeed, _rigidbody2d.velocity.y);

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

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Pickable"))
        {
            if(_pickedObject == null)
                _objectInRange = other.gameObject.GetComponent<PickableObject>();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Pickable"))
        {
            _objectInRange = null;
        }
    }

}
