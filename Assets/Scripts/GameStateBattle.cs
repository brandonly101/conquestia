using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameStateBattle : MonoBehaviour {

    // Public variables.
    public GameObject GUIBattle;

    // Private game variables.
    List<GameObject> VPlayer;
	List<GameObject> VEnemy;
	List<GameObject> VHousesPlayer;
	List<GameObject> VHousesEnemy;
	bool battle;
	int healthPlayer;
	int healthEnemy;

	// GUI variables.
	Slider HealthBarPlayer;
	Slider HealthBarEnemy;

	// Public functions.
	public void assignVillagerTarget (GameObject villager, bool isPlayer) {
		Villager villagerScript = villager.GetComponent<Villager>();
		if (isPlayer) {
			villagerScript.ObjectTarget = VEnemy[Random.Range(0, VEnemy.Count)];
		} else {
			villagerScript.ObjectTarget = VPlayer[Random.Range(0, VPlayer.Count)];
		}
	}

	public void addVillager (GameObject villager, bool isPlayer) {
		if (isPlayer) {
			VPlayer.Add(villager);
		} else {
			VEnemy.Add(villager);
		}
	}

	public void removeVillager (GameObject villager, bool isPlayer) {
		Villager villagerScript = villager.GetComponent<Villager>();
		if (isPlayer) {
			VPlayer.Remove(villager);
			healthPlayer--;
		} else {
			VEnemy.Remove(villager);
			healthEnemy--;
		}
		villagerScript.alive = false;
		villagerScript.Die();
	}

	// Use this for initialization
	void Awake () {
        VHousesPlayer = new List<GameObject>();
		VHousesEnemy = new List<GameObject>();

        // Disable the GUI on awake, for now.
        GUIBattle.SetActive(false);
    }

    void OnEnable () {
        // Spawn all player buildings.
        for (int i = 0; i < SaveManager.GameDataSave.buildingNum; i++) {
            GameObject house = (GameObject) Instantiate(
                Resources.Load(SaveManager.GameDataSave.buildingName[i]),
                new Vector3(
                    SaveManager.GameDataSave.buildingPos[i][0],
                    SaveManager.GameDataSave.buildingPos[i][1],
                    SaveManager.GameDataSave.buildingPos[i][2]
                ),
                new Quaternion(0.0f, 180.0f, 0.0f, 0.0f)
            );
            VHousesPlayer.Add(house);
        }
        foreach (GameObject house in VHousesPlayer) {
            VillageHouse houseScript = house.GetComponent<VillageHouse>();
            houseScript.gameStateBattle = this;
        }

        // Set game variables.
        VPlayer = new List<GameObject>();
        VEnemy = new List<GameObject>();
        healthPlayer = 7;
        healthEnemy = 7;

        // Enable GUI Elements.
        GUIBattle.SetActive(true);
        GUIBattle.transform.GetChild(0).gameObject.SetActive(true);
        GUIBattle.transform.GetChild(1).gameObject.SetActive(true);
		GUIBattle.transform.GetChild(2).gameObject.SetActive(false);
		GUIBattle.transform.GetChild(3).gameObject.SetActive(false);
		GUIBattle.transform.GetChild(4).gameObject.SetActive(false);

        HealthBarPlayer = GUIBattle.transform.GetChild(0).gameObject.GetComponent<Slider>();
        HealthBarEnemy = GUIBattle.transform.GetChild(1).gameObject.GetComponent<Slider>();
        HealthBarPlayer.maxValue = healthPlayer;
        HealthBarEnemy.maxValue = healthEnemy;

        GameManager.GameManagerInstance.MainCamera.transform.position = new Vector3(-25.0f, 20.0f, -30.0f);

        // Test for now...
        spawnEnemies();

        setBattleMode(true);
    }

    // Update is called once per frame
    void Update () {
        // Change health
        HealthBarPlayer.value = healthPlayer;
        HealthBarEnemy.value = healthEnemy;

		if ((healthPlayer <= 0.0f || healthEnemy <= 0.0f) && battle) {
			GUIBattle.transform.GetChild(2).gameObject.SetActive(true);
			setBattleMode(false);

			// Show the correct end battle message and disable health bars.
			if (healthPlayer >= healthEnemy) {
				GUIBattle.transform.GetChild(3).gameObject.SetActive(true);
			} else {
				GUIBattle.transform.GetChild(4).gameObject.SetActive(true);
			}
			GUIBattle.transform.GetChild(0).gameObject.SetActive(false);
			GUIBattle.transform.GetChild(1).gameObject.SetActive(false);

			// Destroy all villagers still present.
			foreach (GameObject villager in VPlayer) {
				villager.GetComponent<Villager>().Die();
			}
			foreach (GameObject villager in VEnemy) {
				villager.GetComponent<Villager>().Die();
			}
		}
    }

    // Private functions.
    void spawnEnemies () {
		for (int i = 0; i < 5; i++) {
			GameObject villageHouse = (GameObject) Instantiate(
				Resources.Load("VillagerHouseEnemy"),
				new Vector3(-50.0f, 0.0f, ((float) i) * 10.0f + 2.5f),
				new Quaternion(0.0f, 0.0f, 0.0f, 0.0f)
			);
			VHousesEnemy.Add(villageHouse);
			villageHouse.GetComponent<VillageHouse>().gameStateBattle = this;
		}
	}

	void setBattleMode (bool mode) {
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

	// Do things when this MonoBehavior is disabled.
	void OnDisable () {
        // Remove the village houses.
        foreach (GameObject house in VHousesPlayer) {
            Destroy(house);
        }
        VHousesPlayer.Clear();
        foreach (GameObject house in VHousesEnemy) {
			Destroy(house);
		}
		VHousesEnemy.Clear();

        // Disable GUI Elements.
        if (GUIBattle) {
            GUIBattle.SetActive(false);
        }
    }
}
