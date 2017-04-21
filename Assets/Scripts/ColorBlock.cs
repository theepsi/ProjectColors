using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ColorBlock : MonoBehaviour {

    public BlockType Type = BlockType.DEFAULT;

    private Renderer _renderer;
    private GameManager _gameManager;

	// Use this for initialization
	void Start () {

        _renderer = GetComponent<Renderer>();
        _gameManager = FindObjectOfType<GameManager>();

        UpdateColor();
    }
	
	// Update is called once per frame
	void Update () {
        UpdateColor();
    }

    void UpdateColor()
    {
        switch (Type)
        {
            case BlockType.DEFAULT:
                _renderer.material = _gameManager.Default;
                break;
            case BlockType.RED:
                _renderer.material = _gameManager.Red;
                break;
            case BlockType.BLUE:
                _renderer.material = _gameManager.Blue;
                break;
            case BlockType.GREEN:
                _renderer.material = _gameManager.Green;
                break;
        }
    }
}
