using UnityEngine;
using System.Collections;

public class VillageHouse : MonoBehaviour {

    public GameObject villager;
    public GameObject villagerPrefab;

    public GameObject spawnVillager(GameObject prefab) {
        Transform spawnPoint = this.transform.Find("SpawnPoint");
        if (villager == null) {
            villager = Instantiate(prefab, spawnPoint.position, spawnPoint.rotation) as GameObject;
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
