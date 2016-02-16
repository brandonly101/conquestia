using UnityEngine;
using System.Collections;

public class VillageHouse : MonoBehaviour {

    public GameObject villager;

	public GameObject spawnVillager(bool isPlayer) {
        Transform spawnPoint = this.transform.Find("SpawnPoint");
        if (villager == null) {
			if (isPlayer) {
				villager = Instantiate (Resources.Load ("VillagerP"), spawnPoint.position, spawnPoint.rotation) as GameObject;
			} else {
				villager = Instantiate (Resources.Load ("VillagerE"), spawnPoint.position, spawnPoint.rotation) as GameObject;
			}
            return villager;
        } else {
            return null;
        }
    }

    // Use this for initialization
    void Start () {
        villager = null;
    }
	
	// Update is called once per frame
	void Update () {
	    
	}
}
