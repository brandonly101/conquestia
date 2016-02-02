using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameStateBattle : MonoBehaviour {

	GameObject VPrefabPlayer;
	GameObject VPrefabEnemy;

	List<GameObject> VPlayer = new List<GameObject>();
	List<GameObject> VEnemy = new List<GameObject>();

	List<GameObject> VHousesPlayer;
	List<GameObject> VHousesEnemy;

	int healthPlayer;
	int healthEnemy;

	// Reference to the singleton GameManager.
	public GameManager gameManager;

	// Use this for initialization
	void Start () {
		this.VPrefabPlayer = gameManager.VPrefabPlayer;
		this.VPrefabEnemy = gameManager.VPrefabEnemy;
		this.VHousesPlayer = gameManager.VHousesPlayer;
		this.VHousesEnemy = gameManager.VHousesEnemy;
	}
	
	// Update is called once per frame
	void Update () {
		// Manage the village houses and to spawn more villagers when necessary.
		manageVillageHouses();

		// Manage the villagers as they attack and defend.
		manageVillagers();
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
			}
		}
	}

	// Manage the houses and spawning in the village.
	void manageVillageHouses() {
		foreach (GameObject house in VHousesPlayer) {
			VillageHouse houseScript = house.GetComponent<VillageHouse>();
			if (houseScript.villager == null) {
				houseScript.spawnVillager(VPrefabPlayer);
				VPlayer.Add(houseScript.villager);
			}
		}
		foreach (GameObject house in VHousesEnemy) {
			VillageHouse houseScript = house.GetComponent<VillageHouse>();
			if (houseScript.villager == null) {
				houseScript.spawnVillager(VPrefabEnemy);
				VEnemy.Add(houseScript.villager);
			}
		}
	}
}
