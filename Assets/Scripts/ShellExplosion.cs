using UnityEngine;
using System.Collections;

public class ShellExplosion : MonoBehaviour {
	public LayerMask enemyMask;
	public ParticleSystem explosionParticles;    
	public GameObject lightning;
	public AudioSource explosionAudio; 
	public float maxDamage = 200f;                  
	public float explosionForce = 10f;            
	public float maxLifeTime;                  
	public float explosionRadius = 10f;              


	private void Start()
	{
		maxLifeTime = explosionParticles.duration;
	}


	public void FireLightning()
	{
		Collider[] colliders = Physics.OverlapSphere (transform.position, explosionRadius, enemyMask);
		Debug.Log ("COLLIDERS FOUND: " + colliders.Length);
		for (int i = 0; i < colliders.Length; i++) {
			EnemyController targetController = colliders[i].GetComponent<EnemyController> ();
			if (!targetController)
				continue;
			float damage = CalculateDamage (targetController.transform.position);
			targetController.TakeDamage (damage);
		}
		Quaternion lightningRotation = Quaternion.Euler(0, 0, 90);
		GameObject strike = Instantiate(lightning, transform.position, lightningRotation) as GameObject;
		explosionParticles.Play ();
		explosionAudio.Play ();
		Destroy (strike, 0.25f);
		Destroy (gameObject, maxLifeTime);
	}


	private float CalculateDamage(Vector3 targetPosition)
	{
		Vector3 explosionToTarget = targetPosition - transform.position;
		float explosionDistance = explosionToTarget.magnitude;
		float relativeDistance = (explosionRadius - explosionDistance) / explosionRadius;
		float damage = maxDamage * relativeDistance;
		damage = Mathf.Max (0f, damage);
		return damage;
	}
}