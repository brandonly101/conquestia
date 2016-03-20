using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    // Singleton instance of the GameManager.
	public static GameManager instance = null;

    // References to the game states.
    public GameStateGather gather;
    public GameStateBuild build;
    public GameStateBattle battle;

    public GameObject DirLight;
	public GameObject ImageTargetGather;
	public GameObject ModelGather;
	public GameObject ImageTarget;
	public GameObject ModelBuildBattle;
    public GameObject ARCamera;
    public AudioClip AudioBuild;
    public AudioClip AudioBattle;

	// Main game GUI.
	public GameObject GUIMainMenu;
	public GameObject GUIHelpMM;

	// Public functions to be accessed by other classes.
    public void StartGameNew () {
		GUIMainMenu.transform.GetChild(1).gameObject.SetActive(false);
		GUIMainMenu.transform.GetChild(2).gameObject.SetActive(false);
		GUIMainMenu.transform.GetChild(3).gameObject.SetActive(false);
		GUIMainMenu.transform.GetChild(4).gameObject.SetActive(true);
    }

	public void StartGameNewConfirm () {
		SaveManager.GameNew();
		StartBuildMode();
		GUIMainMenu.SetActive(false);
	}

	public void StartGameNewCancel () {
		GUIMainMenu.transform.GetChild(1).gameObject.SetActive(true);
		GUIMainMenu.transform.GetChild(2).gameObject.SetActive(true);
		GUIMainMenu.transform.GetChild(3).gameObject.SetActive(true); 
		GUIMainMenu.transform.GetChild(4).gameObject.SetActive(false); 
	}

    public void StartGameContinue () {
        SaveManager.GameLoad();
        GUIMainMenu.SetActive(false);
        StartBuildMode();
    }

	public void StartGatherMode () {
		// Activate and deactivate the correct game states.
		gather.enabled = true;
		build.enabled = false;
		battle.enabled = false;
		ImageTargetGather.SetActive(true);
		ModelGather.SetActive(true);
		ImageTarget.SetActive(false);
		ModelBuildBattle.SetActive(false);
		DirLight.transform.eulerAngles = new Vector3(135.0f, 0.0f, 0.0f);
	}

	public void StartBuildMode () {
        // Activate and deactivate the correct game states.
		gather.enabled = false;
        build.enabled = true;
		battle.enabled = false;
		ImageTargetGather.SetActive(false);
		ModelGather.SetActive(false);
		ImageTarget.SetActive(true);
		ModelBuildBattle.SetActive(true);
		DirLight.transform.eulerAngles = new Vector3(45.0f, 0.0f, 0.0f);
		ARCamera.GetComponent<AudioSource>().clip = AudioBuild;
		ARCamera.GetComponent<AudioSource>().Play();
	}

	public void StartBattleMode () {
        // Activate and deactivate the correct game states.
		gather.enabled = false;
        build.enabled = false;
		battle.enabled = true;
		ImageTargetGather.SetActive(false);
		ModelGather.SetActive(false);
		ImageTarget.SetActive(true);
		ModelBuildBattle.SetActive(true);
		ARCamera.GetComponent<AudioSource>().clip = AudioBattle;
		ARCamera.GetComponent<AudioSource>().Play();
	}

	public void GUIHelpMainMenu () {
		GUIHelpMM.SetActive(true);
		GUIMainMenu.SetActive(false);
	}

	public void GUIHelpMMGoBack () {
		GUIMainMenu.SetActive(true);
		GUIHelpMM.SetActive(false);
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
		gather = GetComponent<GameStateGather>();
		build = GetComponent<GameStateBuild>();
        battle = GetComponent<GameStateBattle>();
		gather.enabled = false;
        build.enabled = false;
        battle.enabled = false;

		ImageTarget.SetActive(false);
		ImageTargetGather.SetActive(false);

        // Enable GUI Elements.
		GUIMainMenu.SetActive(true);
		GUIMainMenu.transform.GetChild(0).gameObject.SetActive(true);
		GUIMainMenu.transform.GetChild(1).gameObject.SetActive(true);
		GUIMainMenu.transform.GetChild(2).gameObject.SetActive(true);
		GUIMainMenu.transform.GetChild(3).gameObject.SetActive(true);
		GUIMainMenu.transform.GetChild(4).gameObject.SetActive(false);
    }
}
