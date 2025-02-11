using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class VillageHouse : MonoBehaviour {

	public GameStateBattle gameStateBattle;

	public bool battleMode;
	public bool isPlayer;

    public int villagerIndex { get; set; }
	GameObject villager;
	
	// Update is called once per frame
	void Update () {
		if (battleMode && villager == null) {
			// Spawn a villager and add it to the gameStateBattle.
			spawnVillager();
			gameStateBattle.addVillager(villagerIndex, villager, isPlayer);
		} else if (battleMode && villager) {
			Villager villagerScript = villager.GetComponent<Villager>();

			// Check to see if villager has a target.
			if (villagerScript.ObjectTarget == null) {
				gameStateBattle.assignVillagerTarget(villager, isPlayer);
			}

			// Check to see if villager is dead.
			if (villagerScript.health == 0 && villagerScript.alive)  {
				gameStateBattle.removeVillager(villagerIndex, villager, isPlayer);
			}
		}
	}

	// Private members.
	GameObject spawnVillager () {
		Transform spawnPoint = this.transform.Find("SpawnPoint");
		if (isPlayer) {
			villager = Instantiate (Resources.Load ("VillagerPGuy"), spawnPoint.position, spawnPoint.rotation) as GameObject;
		} else {
			villager = Instantiate (Resources.Load ("VillagerEGuy"), spawnPoint.position, spawnPoint.rotation) as GameObject;
		}
		return villager;
	}
}
