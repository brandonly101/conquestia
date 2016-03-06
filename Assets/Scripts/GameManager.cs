using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

    // Singleton instance of the GameManager.
	public static GameManager instance = null;

    // References to the game states.
    public GameStateBuild build;
    public GameStateBattle battle;
	public GameObject MainCamera;
	public GameObject ImageTarget;

	// Main game GUI.
	public GameObject GUIMainMenu;
	public GameObject GUIGather;
	public GameObject GUIBuild;
	public GameObject GUIBattle;

    const int PLAYER = 0;
	const int ENEMY = 1;

	// List that keeps track of the village houses and villagers.
	public List<GameObject> VHousesPlayer;
	public List<GameObject> VHousesEnemy;

	// Public functions to be accessed by other classes.
	public void StartGame() {
		GUIMainMenu.SetActive(false);
		StartBuildMode();
	}

	public void StartBuildMode() {
        // Activate and deactivate the correct game states.
        build.enabled = true;
        battle.enabled = false;

        // Activate and deactivate the correct GUI elements.
        GUIBuild.SetActive(true);
		GUIBattle.SetActive(false);
	}

	public void StartBattleMode() {
        // Activate and deactivate the correct game states.
        build.enabled = false;
        battle.enabled = true;

        // Activate and deactivate the correct GUI elements.
        GUIBuild.SetActive(false);
		GUIBattle.SetActive(true);
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
		// Reference and object initializations.
		VHousesPlayer = new List<GameObject>();
		VHousesEnemy = new List<GameObject>();
        build = GetComponent<GameStateBuild>();
        battle = GetComponent<GameStateBattle>();
        build.enabled = false;
        battle.enabled = false;
        foreach (GameObject house in VHousesPlayer) {
            VillageHouse houseScript = house.GetComponent<VillageHouse>();
            houseScript.gameStateBuild = build;
            houseScript.gameStateBattle = battle;
        }
        foreach (GameObject house in VHousesEnemy) {
            VillageHouse houseScript = house.GetComponent<VillageHouse>();
            houseScript.gameStateBuild = build;
            houseScript.gameStateBattle = battle;
        }

        ImageTarget.transform.position = new Vector3(25.0f, 0, 25.0f);
        MainCamera.transform.position = new Vector3(25.0f, 20.0f, -30.0f);

		GUIMainMenu.SetActive(true);
        GUIGather.SetActive(false);
		GUIBuild.SetActive(false);
		GUIBattle.SetActive(false);
    }

    // Update is called once per frame
	void Update () {
		
	}
}
