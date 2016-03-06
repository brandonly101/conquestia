using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class VillageHouse : MonoBehaviour {

	public GameManager gameManager;
	public GameStateBuild gameStateBuild;
	public GameStateBattle gameStateBattle;

	public bool battleMode;
	public bool isPlayer;

	GameObject villager;

    // Use this for initialization
    void Start () {
		
    }
	
	// Update is called once per frame
	void Update () {
		if (battleMode && villager == null) {
			// Spawn a villager and add it to the gameStateBattle.
			spawnVillager();
			gameStateBattle.addVillager(villager, isPlayer);
		} else if (battleMode && villager) {
			Villager villagerScript = villager.GetComponent<Villager>();

			// Check to see if villager has a target.
			if (villagerScript.ObjectTarget == null) {
				gameStateBattle.assignVillagerTarget(villager, isPlayer);
			}

			// Check to see if villager is dead.
			if (villagerScript.health == 0 && villagerScript.alive)  {
				gameStateBattle.removeVillager(villager, isPlayer);
			}
		}
	}

	GameObject spawnVillager () {
		Transform spawnPoint = this.transform.Find("SpawnPoint");
		if (isPlayer) {
			villager = Instantiate (Resources.Load ("VillagerP"), spawnPoint.position, spawnPoint.rotation) as GameObject;
		} else {
			villager = Instantiate (Resources.Load ("VillagerE"), spawnPoint.position, spawnPoint.rotation) as GameObject;
		}
		return villager;
	}
}
