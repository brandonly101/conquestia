using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

    // Singleton instance of the GameManager.
	public static GameManager GameManagerInstance = null;

    // References to the game states.
    public GameStateBuild build;
    public GameStateBattle battle;
	public GameObject MainCamera;
	public GameObject ImageTarget;

	// Main game GUI.
	public GameObject GUIMainMenu;

	// Public functions to be accessed by other classes.
    public void StartGameNew () {
        SaveManager.GameNew();
        GUIMainMenu.SetActive(false);
        StartBuildMode();
    }

    public void StartGameContinue () {
        SaveManager.GameLoad();
        GUIMainMenu.SetActive(false);
        StartBuildMode();
    }

	public void StartBuildMode () {
        // Activate and deactivate the correct game states.
        build.enabled = true;
        battle.enabled = false;
	}

	public void StartBattleMode () {
        // Activate and deactivate the correct game states.
        build.enabled = false;
        battle.enabled = true;
}

	// Use this for initialization
	void Awake () {
		// Check if instance already exists.
		if (GameManagerInstance == null) {
            GameManagerInstance = this;
		} else if (GameManagerInstance != this) {
			Destroy (gameObject);
		}

		// Sets this to not be destroyed when reloading scene.
		DontDestroyOnLoad(gameObject);

		// Call the InitGame function to initialize the first level.
		InitGame();
	}

	void InitGame () {
		// Reference and object initializations.
        build = GetComponent<GameStateBuild>();
        battle = GetComponent<GameStateBattle>();
        build.enabled = false;
        battle.enabled = false;

        ImageTarget.transform.position = new Vector3(25.0f, 0, 25.0f);
        MainCamera.transform.position = new Vector3(25.0f, 20.0f, -30.0f);

        // Enable GUI Elements.
		GUIMainMenu.SetActive(true);
    }
}
