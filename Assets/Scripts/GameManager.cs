using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

	public static GameManager instance = null;
	public GameObject VillagerPrefab;
	public GameObject VillagerEnemyPrefab;
    public GameObject[] VillagerHousesPlayer;
    public GameObject[] VillagerHousesEnemy;

    const int PLAYER = 0;
	const int ENEMY = 1;

	List<GameObject> VillagersPlayer = new List<GameObject>();
	List<GameObject> VillagersEnemy = new List<GameObject>();

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
        foreach (GameObject house in VillagerHousesPlayer) {
            VillagersPlayer.Add(house.GetComponent<VillageHouse>().spawnVillager(VillagerPrefab));
        }
        foreach (GameObject house in VillagerHousesEnemy) {
            VillagersEnemy.Add(house.GetComponent<VillageHouse>().spawnVillager(VillagerEnemyPrefab));
        }
    }

    // Update is called once per frame
    void Update () {
		// Manage the villagers as they attack and defend.
        manageVillagers();

		// Manage the village houses and to spawn more villagers when necessary.
        manageVillageHouses();
	}

    // Private functions.
    void manageVillagers () {
        // Manage each and every player villager.
        foreach (GameObject villager in VillagersPlayer) {
            Villager villagerScript = villager.GetComponent<Villager>();

            // Check to see if player villager each have a target.
            if (villagerScript.ObjectTarget == null) {
                villagerScript.ObjectTarget = VillagersEnemy[Random.Range(0, VillagersEnemy.Count)];
            }

            // Check to see if enemy villager is dead.
            if (villagerScript.GetHealth() == 0 && villagerScript.GetAlive()) {
                villagerScript.SetDead();
                VillagersPlayer.Remove(villager);
                villagerScript.Die();
            }
        }

        // Manage each and every enemy villager.
        foreach (GameObject villager in VillagersEnemy) {
            Villager villagerScript = villager.GetComponent<Villager>();

            // Check to see if enemy villager each have a target.
            if (villagerScript.ObjectTarget == null) {
                villagerScript.ObjectTarget = VillagersPlayer[Random.Range(0, VillagersPlayer.Count)];
            }

            // Check to see if enemy villager is dead.
            if (villagerScript.GetHealth() == 0 && villagerScript.GetAlive()) {
                villagerScript.SetDead();
                VillagersEnemy.Remove(villager);
                villagerScript.Die();
            }
        }
    }

    void manageVillageHouses() {
        foreach (GameObject house in VillagerHousesPlayer) {
            VillageHouse houseScript = house.GetComponent<VillageHouse>();
            if (houseScript.villager == null) {
                houseScript.spawnVillager(VillagerPrefab);
                VillagersPlayer.Add(houseScript.villager);
            }
        }
        foreach (GameObject house in VillagerHousesEnemy) {
            VillageHouse houseScript = house.GetComponent<VillageHouse>();
            if (houseScript.villager == null) {
                houseScript.spawnVillager(VillagerEnemyPrefab);
                VillagersEnemy.Add(houseScript.villager);
            }
        }
    }
}
