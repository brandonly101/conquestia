using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameStateBuild : MonoBehaviour {

	const int FIELD_SIZE = 5;

	public GameObject house;

	List<GameObject> villageHouses;
	List<GameObject> enemyHouses;
	Vector3[,] spawnPoints;
	bool[,] spawnPointsMark;
	public GameManager gameManager;

	// Use this for initialization
	void Start () {
		villageHouses = gameManager.VHousesPlayer;
		enemyHouses = gameManager.VHousesEnemy;

		gameManager.ImageTarget.transform.position = new Vector3(25.0f, 0, 25.0f);

		spawnPoints = new Vector3[FIELD_SIZE, FIELD_SIZE];
		spawnPointsMark = new bool[FIELD_SIZE, FIELD_SIZE];

		// Instantiate array of spawn points!
		for (int row = 0; row < FIELD_SIZE; row++) {
			for (int col = 0; col < FIELD_SIZE; col++) {
				spawnPoints [row, col] = new Vector3 (col * 10.0f, 0.0f, row * 10.0f);
				spawnPointsMark [row, col] = false;
			}
		}

		if (villageHouses == null)
			villageHouses = new List<GameObject>();
		if (enemyHouses == null)
			enemyHouses = new List<GameObject>();

		//gameManager.StartBattleMode ();

//		spawnHouses();
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown(0)) {
			Vector3 mousePos = Input.mousePosition;
//			mousePos.z = 2.0f;
			Ray ray = Camera.main.ScreenPointToRay (mousePos);
			RaycastHit hit;
			Physics.Raycast (ray, out hit, 1000f);
			Vector3 objPos = hit.point;
//			Vector3 objPos = Camera.main.ScreenPointToRay (mousePos);
			spawnHouse (objPos);
		}
	}

	void spawnHouse(Vector3 position) {
		GameObject villager = (GameObject) Instantiate(Resources.Load("VillagerHouse"), position, new Quaternion(0.0f, 180.0f, 0.0f, 0.0f));
		villageHouses.Add(villager);
	}

	void spawnHouses() {
		for (int row = 0; row < FIELD_SIZE; row++) {
			for (int col = 0; col < FIELD_SIZE; col++) {
				spawnHouse (spawnPoints [row, col]);
				spawnPointsMark [row, col] = true;
			}
		}
	}
}
