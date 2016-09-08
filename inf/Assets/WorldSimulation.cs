using UnityEngine;
using System.Collections;

public class WorldSimulation : MonoBehaviour {

	ExampleEntity[] entities;
	InfZone[] zones;
	InfActor[] actors;

	void Start () {
		entities = GameObject.FindObjectsOfType<ExampleEntity> ();
		zones = GameObject.FindObjectsOfType<InfZone> ();
		actors = GameObject.FindObjectsOfType<InfActor> ();
		foreach (var a in actors) {
			a.TransferToClosestZone (zones);
		}
	}

	void Update () {
		foreach (var e in entities) {
			e.UpdateWhenFrozen ();
		}
		foreach (var a in actors) {
			a.OccasionallyCheckForClosestZone(zones, 30);
		}
	}
}
