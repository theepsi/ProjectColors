using UnityEngine;

public class PickableObject : MonoBehaviour {

    public bool CanBePicked = true;
    public Transform PickedLocation;

    private bool _pickedUp = false;
    private Rigidbody2D _rigidbody2d;

	// Use this for initialization
	void Start () {
        _rigidbody2d = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update() {
        if (_pickedUp && CanBePicked)
        {
            //_rigidbody2d.MovePosition(PickedLocation.position);
            transform.position = PickedLocation.position;
        }
	}

    public void SetPickedUp(bool p)
    {
        _pickedUp = p;
        _rigidbody2d.freezeRotation = p;
    }

    public bool IsPicked()
    {
        return _pickedUp;
    }
}
