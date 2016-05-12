using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

[AddComponentMenu ("Cardboard/UI/CardboardReticle")]
[RequireComponent (typeof(Renderer))]
[RequireComponent(typeof(Camera))]
public class Bombard : MonoBehaviour, ICardboardGazePointer  {

	public CardboardHead cardboardHead;
	public GameManager gameManager;
	public Transform floor;
	public GameObject aimer;
	private Vector3 aimerPosition;
	private bool aiming = false;

	void Update ()
	{
		CalculateTargetPosition ();
	}

	public void UpdateAimerPosition(Vector3 hitPoint) {
		aimerPosition = hitPoint;
		aiming = true;
		Vector3 aimerUIPosition = new Vector3 (hitPoint.x, 0f, hitPoint.z);
		aimer.transform.position = aimerUIPosition;

		Vector3 rotation = Vector3.zero - aimer.transform.position;
		Quaternion aimerRotation = Quaternion.LookRotation (- rotation.normalized);
//		aimerRotation.y += 90f;
		aimer.transform.rotation = aimerRotation;
	}

	public void CalculateTargetPosition() {
		RaycastHit hit;
		Ray gaze = cardboardHead.Gaze;
		//TODO: Create aimer where gaze is
		if (Physics.Raycast (gaze, out hit, Mathf.Infinity)) {
			if (hit.collider.tag == "Floor") {
				UpdateAimerPosition (hit.point);
				return;
			}
		}
		aiming = false;
	}



	#region delegate functions

	void OnEnable ()
	{
		GazeInputModule.cardboardPointer = this;
	}

	void OnDisable ()
	{
		if (GazeInputModule.cardboardPointer == this) {
			GazeInputModule.cardboardPointer = null;
		}
	}
		
	public void OnGazeEnabled ()
	{
	}
		
	public void OnGazeDisabled ()
	{
	}

	public void GetPointerRadius(out float innerRadius, out float outerRadius) {
		innerRadius = 0;
		outerRadius = 0;
	}
		
	public void OnGazeStart (Camera camera, GameObject targetObject, Vector3 intersectionPosition,
		bool isInteractive)
	{
	}
		
	public void OnGazeStay (Camera camera, GameObject targetObject, Vector3 intersectionPosition,
		bool isInteractive)
	{
	}
		
	public void OnGazeExit (Camera camera, GameObject targetObject)
	{
	}
		
	public void OnGazeTriggerStart (Camera camera)
	{
		//play a sound or something, or charge up an arrow from the players position
	}
		
	public void OnGazeTriggerEnd (Camera camera)
	{
		if (aiming)
			gameManager.ShotsFired (aimerPosition);
	}

	private void SetGazeTarget (Vector3 target, Transform targetObject)
	{
	}


	public void ToggleActive(bool active) {
		gameObject.GetComponent<Renderer> ().enabled = active;
	}

	#endregion
}
