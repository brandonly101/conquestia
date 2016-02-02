using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

	public static GameManager instance = null;

	public GameObject VPrefabPlayer;
	public GameObject VPrefabEnemy;

    const int PLAYER = 0;
	const int ENEMY = 1;

	// List that keeps track of the village houses and villagers.
	public List<GameObject> VHousesPlayer;
	public List<GameObject> VHousesEnemy;
	List<GameObject> VPlayer;
	List<GameObject> VEnemy;

	// Game States
	GameStateBattle gsBattle;

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
//		VHousesPlayer = new List<GameObject>();
//		VHousesEnemy = new List<GameObject>();
		VPlayer = new List<GameObject>();
		VEnemy = new List<GameObject>();
		gameObject.AddComponent<GameStateBattle>().gameManager = this;
    }

    // Update is called once per frame
	void Update () {
		
	}
}
