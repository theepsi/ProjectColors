using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public GameObject ActivePower;

    private BlockType _currentColor;

    public Material White;
    public Material Red;
    public Material Green;
    public Material Blue;

    public Material Default;

    private ColorBlock[] _colorBlocks;
    private Player _player;

    // Use this for initialization
    void Start () {
        _colorBlocks = FindObjectsOfType<ColorBlock>();
        _player = FindObjectOfType<Player>();
        SetActivePower(BlockType.WHITE);
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
        foreach (ColorBlock block in _colorBlocks)
        {
            if (_currentColor == block.GetComponent<ColorBlock>().Type)
                block.gameObject.SetActive(false);
            else
                block.gameObject.SetActive(true);
        }
    }

    public void RestartLevel()
    {
        _player.gameObject.SetActive(false);
        StartCoroutine(WaitPlayerKill(1.5f));
    }

    public IEnumerator WaitPlayerKill(float waitseconds)
    {
        yield return new WaitForSeconds(waitseconds);

        // TODO: restart current level, this must call death menu in the future, letting the player to choose RESET or MAIN MENU
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
