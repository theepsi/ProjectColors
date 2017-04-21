using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public GameObject ActivePower;

    public Material White;
    public Material Red;
    public Material Green;
    public Material Blue;

    // Use this for initialization
    void Start () {
        SetActivePower(White);
    }
	
	// Update is called once per frame
	void Update () {
        
        // TODO: Add some cooldown to this.
        if (Input.GetButtonDown("White"))
            SetActivePower(White);
        if (Input.GetButtonDown("Red"))
            SetActivePower(Red);
        if (Input.GetButtonDown("Green"))
            SetActivePower(Green);
        if (Input.GetButtonDown("Blue"))
            SetActivePower(Blue);
    }

    void SetActivePower(Material newColor)
    {
        ActivePower.GetComponent<Renderer>().material = newColor;
    }
}
