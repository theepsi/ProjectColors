using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour {

    public Transform Player;
    public int CameraSize = 5;
    public bool Following = true;

	// Use this for initialization
	void Start () {
        transform.position = new Vector3(
            Player.transform.position.x + 4,
            0,
            -5);
        GetComponent<Camera>().orthographicSize = CameraSize;
	}
	
	// Update is called once per frame
	void Update () {
        if (Following)
            transform.position = new Vector3(
                Player.transform.position.x + 4,
                0,
                -5);
    }
}
