using UnityEngine;
using System.Collections;

public class WorldSimulation : MonoBehaviour {

	ExampleEntity[] entities;

	void Start () {
		entities = GameObject.FindObjectsOfType<ExampleEntity> ();
		var zones = GameObject.FindObjectsOfType<InfZone> ();
		var actors = GameObject.FindObjectsOfType<InfActor> ();
		foreach (var a in actors) {
			a.TransferToClosestZone (zones);
		}
	}

	void Update () {
		foreach (var e in entities) {
			e.UpdateWhenFrozen ();
		}
	}
}
