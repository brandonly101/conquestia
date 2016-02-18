using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameStateBattle : MonoBehaviour {

	// Reference to the singleton GameManager.
	public GameManager gameManager;

	public GameObject VPrefabPlayer;
	public GameObject VPrefabEnemy;
	public GameObject CanvasHealthPrefab;

	// Private variables.
	int healthPlayer;
	int healthEnemy;
	GameObject GUIHealth;
	Slider HealthBarPlayer;
	Slider HealthBarEnemy;
	List<GameObject> VPlayer = new List<GameObject>();
	List<GameObject> VEnemy = new List<GameObject>();
	List<GameObject> VHousesPlayer;
	List<GameObject> VHousesEnemy;

	// Use this for initialization
	void Start () {
		this.VHousesPlayer = gameManager.VHousesPlayer;
		this.VHousesEnemy = gameManager.VHousesEnemy;

		healthPlayer = 30;
		healthEnemy = 30;

		GUIHealth = Instantiate(Resources.Load("GUI/BattleHealthGUI"), transform.position, transform.rotation) as GameObject;
		HealthBarPlayer = GUIHealth.transform.GetChild(0).gameObject.GetComponent<Slider>();
		HealthBarEnemy = GUIHealth.transform.GetChild(1).gameObject.GetComponent<Slider>();
		HealthBarPlayer.maxValue = healthPlayer;
		HealthBarEnemy.maxValue = healthEnemy;

		gameManager.ImageTarget.transform.position = new Vector3(25.0f, 0.0f, -25.0f);
	}
	
	// Update is called once per frame
	void Update () {
		// Manage the village houses and to spawn more villagers when necessary.
		manageVillageHouses();

		// Manage the villagers as they attack and defend.
		manageVillagers();

		// Change health
		HealthBarPlayer.value = healthPlayer;
		HealthBarEnemy.value = healthEnemy;
	}

	// Private functions.

	// Manage the villagers.
	void manageVillagers () {
		// Manage each and every player villager.
		foreach (GameObject villager in VPlayer) {
			Villager villagerScript = villager.GetComponent<Villager>();

			// Check to see if player villager each have a target.
			if (villagerScript.ObjectTarget == null) {
				villagerScript.ObjectTarget = VEnemy[Random.Range(0, VEnemy.Count)];
			}

			// Check to see if enemy villager is dead.
			if (villagerScript.GetHealth() == 0 && villagerScript.GetAlive()) {
				villagerScript.SetDead();
				VPlayer.Remove(villager);
				villagerScript.Die();
				healthPlayer--;
			}
		}

		// Manage each and every enemy villager.
		foreach (GameObject villager in VEnemy) {
			Villager villagerScript = villager.GetComponent<Villager>();

			// Check to see if enemy villager each have a target.
			if (villagerScript.ObjectTarget == null) {
				villagerScript.ObjectTarget = VPlayer[Random.Range(0, VPlayer.Count)];
			}

			// Check to see if enemy villager is dead.
			if (villagerScript.GetHealth() == 0 && villagerScript.GetAlive()) {
				villagerScript.SetDead();
				VEnemy.Remove(villager);
				villagerScript.Die();
				healthEnemy--;
			}
		}
	}

	// Manage the houses and spawning in the village.
	void manageVillageHouses() {
		// Manage player houses.
		foreach (GameObject house in VHousesPlayer) {
			VillageHouse houseScript = house.GetComponent<VillageHouse>();
			if (houseScript.villager == null) {
				houseScript.spawnVillager(true);
				VPlayer.Add(houseScript.villager);
			}
		}

		// Manage enemy houses.
		foreach (GameObject house in VHousesEnemy) {
			VillageHouse houseScript = house.GetComponent<VillageHouse>();
			if (houseScript.villager == null) {
				houseScript.spawnVillager(false);
				VEnemy.Add(houseScript.villager);
			}
		}
	}
}
