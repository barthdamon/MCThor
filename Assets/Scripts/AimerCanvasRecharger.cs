using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AimerCanvasRecharger : MonoBehaviour {

	public RawImage hammer;
	public Light chargedLight;

	private bool recharging = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public bool isRecharging() {
		return recharging;
	}

	public void LightningShot() {
//		hammer.enabled = false;
		chargedLight.enabled = false;
		recharging = true;
		StartCoroutine (RechargeHammer ());
	}

	IEnumerator RechargeHammer() {
		yield return new WaitForSeconds (1);
		recharging = false;
//		hammer.enabled = true;
		chargedLight.enabled = true;
	}
}
