using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

	public static GameManager instance = null;

	public Camera cameraObj;
	public GameObject canvas;
	public GameObject ImageTarget;

    const int PLAYER = 0;
	const int ENEMY = 1;

	// List that keeps track of the village houses and villagers.
	public List<GameObject> VHousesPlayer;
	public List<GameObject> VHousesEnemy;

	// Public functions to be accessed by other classes.
	public void StartGame() {
		canvas.SetActive(false);
		StartBuildMode();
//		gameObject.AddComponent<GameStateBuild>().gameManager = this;
	}

	public void StartBuildMode() {
		gameObject.AddComponent<GameStateBuild>().gameManager = this;
	}

	public void StartBattleMode() {
		gameObject.AddComponent<GameStateBattle>().gameManager = this;
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
    }

    // Update is called once per frame
	void Update () {
		
	}
}
