using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameStateBattle : MonoBehaviour {

    // Public variables.
	public GameObject GUIBattle;
    public GameObject GUIBattleWorld;

    // Private game variables.
    List<GameObject> VPlayer;
	List<GameObject> VEnemy;
	List<GameObject> VHousesPlayer;
	List<GameObject> VHousesEnemy;
	List<GameObject> BuildPlayer;
	List<GameObject> BuildEnemy;
	bool battle;
	int healthPlayer;
	int healthEnemy;

	int enemyNumArmory;
	int enemyNumFarm;

	// GUI variables.
	Slider HealthBarPlayer;
	Slider HealthBarEnemy;

	// Public functions.
	public void assignVillagerTarget (GameObject villager, bool isPlayer) {
		Villager villagerScript = villager.GetComponent<Villager>();
		if (isPlayer) {
			villagerScript.ObjectTarget = VEnemy[Random.Range(0, VEnemy.Count - 1)];
		} else {
			villagerScript.ObjectTarget = VPlayer[Random.Range(0, VPlayer.Count - 1)];
		}
	}

	public void addVillager (int index, GameObject villager, bool isPlayer) {
		if (isPlayer) {
			VPlayer[index] = villager;
		} else {
			VEnemy[index] = villager;
		}
	}

	public void removeVillager (int index, GameObject villager, bool isPlayer) {
		Villager villagerScript = villager.GetComponent<Villager>();
		if (isPlayer) {
			VPlayer[index] = null;
			healthPlayer--;
		} else {
            VEnemy[index] = null;
            healthEnemy--;
		}
		villagerScript.alive = false;
		villagerScript.Die();
	}

	// Use this for initialization
	void Awake () {
        VHousesPlayer = new List<GameObject>();
		VHousesEnemy = new List<GameObject>();
		BuildPlayer = new List<GameObject>();
		BuildEnemy = new List<GameObject>();
        battle = false;

        // Disable the GUI on awake, for now.
        GUIBattle.SetActive(false);
    }

    void OnEnable () {
        // Spawn all player buildings.
		for (int i = 0; i < SaveManager.GameDataSave.buildingPos.Count; i++) {
			string resourceName = SaveManager.GameDataSave.buildingName[i];
            GameObject building = (GameObject) Instantiate(
				Resources.Load(resourceName),
                new Vector3(
                    SaveManager.GameDataSave.buildingPos[i][0],
                    SaveManager.GameDataSave.buildingPos[i][1],
                    SaveManager.GameDataSave.buildingPos[i][2]
                ),
                new Quaternion(0.0f, 0.0f, 0.0f, 0.0f)
            );
			BuildPlayer.Add(building);
			if (resourceName == "VillagerHousePlayer") {
				VHousesPlayer.Add(building);
				VillageHouse buildingScript = building.GetComponent<VillageHouse>();
				buildingScript.gameStateBattle = this;
                buildingScript.villagerIndex = VHousesPlayer.Count - 1;
			}
        }
        VPlayer = new List<GameObject>();
        foreach (GameObject building in VHousesPlayer) {
            VPlayer.Add(null);
        }
		healthPlayer = SaveManager.GameDataSave.healthVillage + VHousesPlayer.Count / 5 + SaveManager.GameDataSave.numFarm * GameDataLevels.healthFarm;

		// Spawn enemies.
		BuildEnemy = GameDataLevels.initEnemyHouses(SaveManager.GameDataSave.GameLevel, ref VHousesEnemy, ref enemyNumArmory, ref enemyNumFarm);
		for (int i = 0; i < VHousesEnemy.Count; i++) {
            VillageHouse buildingScript = VHousesEnemy[i].GetComponent<VillageHouse>();
            buildingScript.gameStateBattle = this;
            buildingScript.villagerIndex = i;
        }
        VEnemy = new List<GameObject>();
        foreach (GameObject building in VHousesEnemy) {
            VEnemy.Add(null);
        }
        healthEnemy = GameDataLevels.healthEnemyVillage + VHousesEnemy.Count / 5 + enemyNumFarm * GameDataLevels.healthFarm;

        // Enable GUI Elements.
        GUIBattle.SetActive(true);
        GUIBattleWorld.SetActive(true);
        GUIBattle.transform.GetChild(0).gameObject.SetActive(true);
        GUIBattle.transform.GetChild(1).gameObject.SetActive(false);
		GUIBattle.transform.GetChild(2).gameObject.SetActive(false);
		GUIBattle.transform.GetChild(3).gameObject.SetActive(false);
		GUIBattle.transform.GetChild(4).gameObject.SetActive(false);

        HealthBarPlayer = GUIBattleWorld.transform.GetChild(0).gameObject.GetComponent<Slider>();
        HealthBarEnemy = GUIBattleWorld.transform.GetChild(1).gameObject.GetComponent<Slider>();
        HealthBarPlayer.gameObject.transform.forward = -HealthBarPlayer.gameObject.transform.forward;
        HealthBarEnemy.gameObject.transform.forward = -HealthBarEnemy.gameObject.transform.forward;
        HealthBarPlayer.maxValue = healthPlayer;
        HealthBarEnemy.maxValue = healthEnemy;

        if (GameManager.instance.ARCamera.activeInHierarchy) {
            GameManager.instance.ARCamera.GetComponent<AudioSource>().clip = GameManager.instance.AudioBattle;
            GameManager.instance.ARCamera.GetComponent<AudioSource>().Play();
        } else {
            GameManager.instance.DebugCamera.GetComponent<AudioSource>().clip = GameManager.instance.AudioBattle;
            GameManager.instance.DebugCamera.GetComponent<AudioSource>().Play();
        }

        // Spawn enemies and begin battle mode (after 3 seconds)!
        StartCoroutine(setBattleModeWrapper(true, 2.0f));
    }

    // Update is called once per frame
    void Update () {
        // Change health
        HealthBarPlayer.value = healthPlayer;
        HealthBarEnemy.value = healthEnemy;
        HealthBarPlayer.gameObject.transform.LookAt(GameManager.instance.ARCamera.transform);
        HealthBarEnemy.gameObject.transform.LookAt(GameManager.instance.ARCamera.transform);

        if ((healthPlayer <= 0.0f || healthEnemy <= 0.0f) && battle) {
			GUIBattle.transform.GetChild(2).gameObject.SetActive(true);
			setBattleMode(false);

			// Show the correct end battle message and disable health bars.
			if (healthPlayer >= healthEnemy) {
				GUIBattle.transform.GetChild(3).gameObject.SetActive(true);
                SaveManager.GameDataSave.GameLevel++;
			} else {
				GUIBattle.transform.GetChild(4).gameObject.SetActive(true);
//				SaveManager.GameDataSave.removeBuilding(Random.Range (0, SaveManager.GameDataSave.buildingPos.Count - 1));
			}
			GUIBattle.transform.GetChild(0).gameObject.SetActive(false);
			GUIBattle.transform.GetChild(1).gameObject.SetActive(false);

            // Destroy all villagers still present.
            for (int i = 0; i < VPlayer.Count; i++) {
                if (VPlayer[i]) {
                    VPlayer[i].GetComponent<Villager>().Die();
                }
            }
            for (int i = 0; i < VEnemy.Count; i++) {
                if (VEnemy[i]) {
                    VEnemy[i].GetComponent<Villager>().Die();
                }
            }

			SaveManager.GameSave();
		}
    }

    // Private functions.
    IEnumerator setBattleModeWrapper (bool mode, float time) {
        yield return new WaitForSeconds(time);
        setBattleMode(mode);
    }

    void setBattleMode (bool mode) {
        if (!battle) {
            GUIBattle.transform.GetChild(0).gameObject.SetActive(false);
            GUIBattle.transform.GetChild(1).gameObject.SetActive(true);
            StartCoroutine(GUIDisableOverTime(2.0f));
        }
        
        // Set whether or not the battle is going on currently.
        battle = mode;

		// Manage player houses.
		foreach (GameObject house in VHousesPlayer) {
			VillageHouse houseScript = house.GetComponent<VillageHouse>();
			houseScript.battleMode = mode;
		}

		// Manage enemy houses.
		foreach (GameObject house in VHousesEnemy) {
			VillageHouse houseScript = house.GetComponent<VillageHouse>();
			houseScript.battleMode = mode;
		}
	}

    IEnumerator GUIDisableOverTime(float time) {
        yield return new WaitForSeconds(time);
        GUIBattle.transform.GetChild(1).gameObject.SetActive(false);
    }

    // Do things when this MonoBehavior is disabled.
    void OnDisable () {
        // Remove the village houses.
		foreach (GameObject building in BuildPlayer) {
			Destroy(building);
        }
		BuildPlayer.Clear();
        VHousesPlayer.Clear();
		foreach (GameObject building in BuildEnemy) {
			Destroy(building);
		}
		BuildEnemy.Clear();
		VHousesEnemy.Clear();

        // Disable GUI Elements.
        if (GUIBattle) {
            GUIBattle.SetActive(false);
            GUIBattleWorld.SetActive(false);
        }

		// Enable correct audio to play.
        if (GameManager.instance.ARCamera.activeInHierarchy) {
            GameManager.instance.ARCamera.GetComponent<AudioSource>().clip = GameManager.instance.AudioBuild;
            GameManager.instance.ARCamera.GetComponent<AudioSource>().Play();
        } else {
            GameManager.instance.DebugCamera.GetComponent<AudioSource>().clip = GameManager.instance.AudioBuild;
            GameManager.instance.DebugCamera.GetComponent<AudioSource>().Play();
        }
    }
}
