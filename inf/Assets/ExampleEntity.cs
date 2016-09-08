using UnityEngine;
using System.Collections;

public class ExampleEntity : MonoBehaviour, Freezable {
	
	public float distanceTravelledWhileFrozen;
	public Vector3 freezePosition;
	public Vector3 target;

	void Awake () {
		target = Random.onUnitSphere * Random.Range (10f, 30f);
		target.y = 0f;
	}
		
	void Update () {
		if (Vector3.Distance (transform.position, target) > 1f) {
			transform.LookAt (target);
			transform.Translate (Vector3.forward * Time.deltaTime * 2f);
		}
	}

	public void UpdateWhileFrozen(InfZone zone) {
		distanceTravelledWhileFrozen += Time.deltaTime * 2f;
	}

	public void Freeze(InfZone zone) {
		distanceTravelledWhileFrozen = 0;
		freezePosition = transform.position;
	}

	public void Unfreeze(InfZone zone) {
		float t = distanceTravelledWhileFrozen / Vector3.Distance (freezePosition, target);
		transform.position = Vector3.Lerp(freezePosition, target, t);
	}
}
