using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (Player))]
public class PlayerInput : MonoBehaviour {

    Player _player;
	// Use this for initialization
	void Start () {
        _player = GetComponent<Player>();
	}
	
	// Update is called once per frame
	void Update () {
		Vector2 directionalInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        _player.SetDirectionalInput(directionalInput);

        if (Input.GetButtonDown("Jump"))
        {
            _player.OnJumpInput();
        }

        if (Input.GetButtonDown("Interact"))
        {
            _player.OnInteract();
        }
    }
}
