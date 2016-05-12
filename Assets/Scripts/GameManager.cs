using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	public GameObject enemy;
	public bool gameOver = false;
	public GameObject lightning;
	public AimerCanvasRecharger aimer;
	public Canvas mainCanvas;

	private float spawnDistance = 34.0f;
	private float respawnTime = 5f;
	private float playerHealth = 100;


	// Use this for initialization
	void Start () {
		StartCoroutine(BeginWaves());
	}
	
	// Update is called once per frame
	void Update () {
	}

	IEnumerator BeginWaves () {
		yield return new WaitForSeconds(1);
		while (!gameOver) {
			print ("Starting new wave");
			Vector3 spawnPoint = RandomSpawn();
			Vector3 rotation = Vector3.zero - spawnPoint;
			Quaternion lookRotation = Quaternion.LookRotation (rotation.normalized);
			GameObject newEnemy = Instantiate (enemy, spawnPoint, lookRotation) as GameObject;
			newEnemy.GetComponent<EnemyController> ().gameManager = this;

			if (respawnTime > 2) {
				respawnTime -= 0.25f;
			}

			yield return new WaitForSeconds(respawnTime);
		}
	}

	public void SetGameOver() {
		gameOver = true;
	}

	public void PlayerAttacked() {
		Debug.Log ("Player Attacked");
		// make the canvas flicker red
		Color redFlash = new Color(1.0f, 0f, 0f, 0.5f);
		Image canvasImage = mainCanvas.GetComponent<Image> ();
		mainCanvas.GetComponent<Image> ().color = Color.Lerp (canvasImage.color, redFlash, 0.25f);
		StartCoroutine (flashBack ());
	}

	IEnumerator flashBack () {
		yield return new WaitForSeconds (0.25f);
		Color resetFlash = new Color(0f, 0f, 0f, 0.0f);
		Image canvasImage = mainCanvas.GetComponent<Image> ();
		mainCanvas.GetComponent<Image> ().color = Color.clear;
	}

	public void ShotsFired(Vector3 location) {
		if (!aimer.isRecharging ()) {
			Debug.Log ("SHOTS FIRED AT: " + location);
			//fire away
			GameObject shot = Instantiate(lightning, location, Quaternion.identity) as GameObject;
			shot.GetComponent<ShellExplosion> ().FireLightning ();
			aimer.LightningShot ();
			//		shot.GetComponent<Rigidbody>().velocity = trajectory;
		}
	}

	private Vector3 RandomSpawn() {
		float ang = Random.value * 360;
		// so you always want spawnDistance

		Vector3 pos;
		pos.x = 0 + spawnDistance * Mathf.Cos(ang * Mathf.Deg2Rad);
		pos.y = -1;
		pos.z = 0 + spawnDistance * Mathf.Sin(ang * Mathf.Deg2Rad);
		return pos;
	}
}
