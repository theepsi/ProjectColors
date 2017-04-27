using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(BoxCollider2D))]
public class ColorBlock : MonoBehaviour {

    public BlockType Type = BlockType.DEFAULT;

    private Renderer _renderer;
    private GameManager _gameManager;

    BoxCollider2D _collider;

    Bounds _storedBounds;

	// Use this for initialization
	void Awake () {

        _renderer = GetComponent<Renderer>();
        _gameManager = FindObjectOfType<GameManager>();
        _collider = GetComponent<BoxCollider2D>();

        _storedBounds = _collider.bounds;

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

    public bool CheckTargetInside(Vector2 target)
    {
        float offset = 0.5f;

        if (target.x - offset >= _storedBounds.max.x)
            return false;
        else if (target.x + offset <= _storedBounds.min.x)
            return false;
        else if (target.y >= _storedBounds.max.y)
            return false;
        else if (target.y <= _storedBounds.min.y)
            return false;
        else
            return true;
    }
}
