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

    // Private functions.
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
