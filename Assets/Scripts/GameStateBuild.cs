using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameStateBuild : MonoBehaviour {
    
    public GameObject house;
    
    List<GameObject> villageHouses;
    List<GameObject> enemyHouses;
    public Transform [] spawnPoints;
    public GameManager gameManager;
    
    public float spawnTime = 1.5f;
	// Use this for initialization
	void Start () {
        villageHouses = gameManager.VHousesPlayer;
        enemyHouses = gameManager.VHousesEnemy;
        
        if(villageHouses == null)
            villageHouses = new List<GameObject>();
        if(enemyHouses == null)
            enemyHouses = new List<GameObject>();
        
        Invoke("spawnHouses", spawnTime);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    
    void spawnHouses() {
        //int spawnIndex = Random.Range (0, spawnPoints.Length);
        for(int i = 0; i < spawnPoints.Length; i++){
        Instantiate(house, spawnPoints [i].position, spawnPoints [i].rotation);
        //yield return new WaitForSeconds(2);
        }
    }
}
