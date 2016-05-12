using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour {

	public float speed = 0.03f;
	public AudioSource enemyDeathAudio;
	public Animation trollAnimator;
	public GameManager gameManager;
	public ParticleSystem snowSplash;

	private bool hitting = false;
	private bool moving = false;
	private Vector3 playerPosition;
	private Vector3 nRoute;
	private float health = 100f;
	private float timePassed = 0f;
	private float timeToMove = 10f;

	// Use this for initialization
	void Start () {
		timeToMove = trollAnimator.GetComponent<Animation> ().clip.length;
	}

	// Update is called once per frame
	void Update () {
		if (!moving) {
			if (timePassed >= timeToMove) {
				moving = true;
				trollAnimator.CrossFade ("Run");
				snowSplash.Stop ();
			} else {
				timePassed += Time.deltaTime;
			}
		} else {
			Vector3 route = playerPosition - transform.position;
			if (route.magnitude < 3) {
				BeginHitting ();
			} else {
				nRoute = route.normalized;
			}
		}
	}

	void FixedUpdate() {
		if (!hitting && health > 0 && moving) {
			Vector3 movement = nRoute * speed * Time.deltaTime;
			transform.position =  transform.position + movement;
		}
	}

	void BeginHitting() {
		if (!hitting) {
			hitting = true;
			trollAnimator.CrossFade ("Attack_01");
			StartCoroutine (KeepHitting ());
		}
	}

	IEnumerator KeepHitting() {
		float wait = trollAnimator.GetComponent<Animation> ().clip.length;
		yield return new WaitForSeconds (wait - (wait / 2f));
		while (health > 0) {
			gameManager.PlayerAttacked ();
			yield return new WaitForSeconds (wait);
		}
	}

	public void TakeDamage(float damage) {
		health -= damage;
//		if (health < 0) {
			enemyDeathAudio.Play ();
			trollAnimator.CrossFade ("Die");
//			gameObject.GetComponent<Renderer> ().enabled = false;
			Destroy (gameObject, trollAnimator.GetComponent<Animation> ().clip.length);
//		}
	}
}
