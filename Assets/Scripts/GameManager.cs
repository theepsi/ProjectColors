using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public GameObject ActivePower;

    private BlockType _currentColor;

    public Material White;
    public Material Red;
    public Material Green;
    public Material Blue;

    public Material Default;

    private ColorBlock[] _colorBlocks;
    private List<GameObject> _blocks;

    // Use this for initialization
    void Start () {
        _blocks = new List<GameObject>();
        SetActivePower(BlockType.WHITE);
        _colorBlocks = FindObjectsOfType<ColorBlock>();

        foreach (ColorBlock block in _colorBlocks)
            _blocks.Add(block.gameObject);
    }
	
	// Update is called once per frame
	void Update () {
        
        // TODO: Add some cooldown to this.
        if (Input.GetButtonDown("White"))
            SetActivePower(BlockType.WHITE);
        if (Input.GetButtonDown("Red"))
            SetActivePower(BlockType.RED);
        if (Input.GetButtonDown("Green"))
            SetActivePower(BlockType.GREEN);
        if (Input.GetButtonDown("Blue"))
            SetActivePower(BlockType.BLUE);
    }

    void SetActivePower(BlockType color)
    {
        switch (color)
        {
            case BlockType.WHITE:
                ActivePower.GetComponent<Renderer>().material = White;
                break;
            case BlockType.RED:
                ActivePower.GetComponent<Renderer>().material = Red;
                break;
            case BlockType.BLUE:
                ActivePower.GetComponent<Renderer>().material = Blue;
                break;
            case BlockType.GREEN:
                ActivePower.GetComponent<Renderer>().material = Green;
                break;
        }
        _currentColor = color;
        CheckColorBlocks();
    }

    void CheckColorBlocks()
    {
        foreach (GameObject block in _blocks)
        {
            if (_currentColor == block.GetComponent<ColorBlock>().Type)
                block.SetActive(false);
            else
                block.SetActive(true);
        }
    }
}
