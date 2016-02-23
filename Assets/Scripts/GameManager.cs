using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

	public static GameManager instance = null;

	public GameObject titleCanvas;
	public GameObject buildCanvas;
	public GameObject MainCamera;
	public GameObject ImageTarget;
	public GameObject BuildButton;
	public GameObject BattleButton;

	//public class BuildState;
    const int PLAYER = 0;
	const int ENEMY = 1;

	// List that keeps track of the village houses and villagers.
	public List<GameObject> VHousesPlayer;
	public List<GameObject> VHousesEnemy;

	// Public functions to be accessed by other classes.
	public void StartGame() {
		titleCanvas.SetActive(false);
		StartBuildMode();
	}

	public void StartBuildMode() {
		gameObject.AddComponent<GameStateBuild>().gameManager = this;
		if (GetComponent<GameStateBattle>()) {
			Destroy (GetComponent<GameStateBattle>());
		}
		BattleButton.SetActive(true);
		BuildButton.SetActive(false);
		buildCanvas.SetActive(true);
	}

	public void StartBattleMode() {
		gameObject.AddComponent<GameStateBattle>().gameManager = this;
		if (GetComponent<GameStateBuild>()) {
			Destroy (GetComponent<GameStateBuild>());
		}
		BattleButton.SetActive(false);
		BuildButton.SetActive(true);

	}

	// Use this for initialization
	void Awake () {
		// Check if instance already exists.
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy (gameObject);
		}

		// Sets this to not be destroyed when reloading scene.
		DontDestroyOnLoad(gameObject);

		// Call the InitGame function to initialize the first level.
		InitGame();
	}

	void InitGame () {
		VHousesPlayer = new List<GameObject>();
//		VHousesEnemy = new List<GameObject>();
		titleCanvas = GameObject.Find("Canvas");
		buildCanvas = GameObject.Find("BuildCanvas");
		BattleButton = GameObject.Find ("Battle!");
		BuildButton = GameObject.Find ("Build!");
		buildCanvas.SetActive(false);
    }

    // Update is called once per frame
	void Update () {
		
	}
}
