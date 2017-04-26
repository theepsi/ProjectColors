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
	void LateUpdate () {
        if (Following)
        {
            //Vector3 point = GetComponent<Camera>().WorldToViewportPoint(new Vector3(Player.transform.position.x + 4, 0, -5));
            //Vector3 delta = new Vector3(Player.transform.position.x + 4, 0, -5) - GetComponent<Camera>().ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z));
            //Vector3 destination = transform.position + delta;
            //transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, DampTime);

            transform.position = Vector3.Lerp(transform.position, new Vector3(Player.transform.position.x + 4, 0, -5), Time.deltaTime * 1.5f);

            //transform.position = new Vector3(Player.transform.position.x + 4, 0, -5);
        }

    }
}
