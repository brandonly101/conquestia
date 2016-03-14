using UnityEngine;
using System.Collections;

public class Villager : MonoBehaviour {

	public GameObject ObjectTarget;
	public GameObject ExplodePrefab;
	public bool isPlayer;
	public bool alive;
	public int health;

	public Animator anim;
	float timer;
	float speed;

    // Function that kills the villager.
    public void Die () {
		foreach (SkinnedMeshRenderer mesh in GetComponentsInChildren<SkinnedMeshRenderer>()) {
            mesh.enabled = false;
        }
		GetComponent<AudioSource>().Play();
        GameObject exp = Instantiate(ExplodePrefab, transform.position, transform.rotation) as GameObject;
        Destroy(exp, 1.0f);
        Destroy(this.gameObject, 1.0f);
    }

	public void SetObjectTarget (GameObject target) {
		ObjectTarget = target;
	}

    // Damage function.
	public void TakeDamage () {
		if (timer > 1.0f) {
			health--;
			timer = 0.0f;
		}
	}

	// Use this for initialization
	void Start () {
		anim = GetComponentInChildren<Animator>();
		timer = 0.0f;
		alive = true;
		speed = 0.07f;
		anim.SetFloat("Speed", 1.0f);

		// Set villager health.
		if (isPlayer) {
			health = SaveManager.GameDataSave.healthVillager + SaveManager.GameDataSave.numArmory * GameDataLevels.healthArmory;
		} else {
			health = 5;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (alive) {
			timer += Time.deltaTime;
            if (ObjectTarget) {
                if (Vector3.Distance(transform.position, ObjectTarget.transform.position) > 1.0f) {
					anim.SetBool("Attack", false);
                    MoveTo(ObjectTarget.transform.position, speed);
					transform.forward = Vector3.Normalize(ObjectTarget.transform.position - transform.position);
                } else {
                    anim.SetBool("Attack", true);
                    Attack();
                }
            } else {
                anim.SetBool("Attack", false);
            }
		}
	}

	// Private Functions.
	void Attack () {
		ObjectTarget.GetComponent<Villager>().TakeDamage();
	}

	void MoveTo (Vector3 target, float speed) {
		transform.position = Vector3.MoveTowards(transform.position, target, speed);
	}
}
