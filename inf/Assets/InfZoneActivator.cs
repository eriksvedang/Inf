using UnityEngine;
using System.Collections.Generic;

public enum ActivationStrategy {
	OnlyClosestZone,
	AllZonesWithinDistance,
}

public class InfZoneActivator : MonoBehaviour {

	public int everyNthFrame = 20;
	public ActivationStrategy activationStrategy = ActivationStrategy.AllZonesWithinDistance;
	public float distanceThreshold = 20f; // used by AllZonesWithinDistance

	InfZone[] zones;
	InfZone closest; // used by OnlyClosestZone

    void Start () {
		zones = GameObject.FindObjectsOfType<InfZone>();
		foreach(var zone in zones) {
			zone.Freeze();
		}
    }

    void Update () {
		if(Time.frameCount % everyNthFrame == 0) {
			Refresh();
		}
    }

	void Refresh() {
		if(activationStrategy == ActivationStrategy.OnlyClosestZone) {
			var newClosest = InfZone.ClosestZone(transform.position, zones);
			if(newClosest != closest) {
				if(closest) {
					closest.Freeze();
				}
				newClosest.Unfreeze();
				closest = newClosest;
			}
		}
		else if(activationStrategy == ActivationStrategy.AllZonesWithinDistance) {
			foreach(var zone in zones) {
				float distance = Vector3.Distance(transform.position, zone.transform.position);
				if(distance < distanceThreshold) {
					zone.Unfreeze();
				} else {
					zone.Freeze();
				}
			}
		}
	}
}
