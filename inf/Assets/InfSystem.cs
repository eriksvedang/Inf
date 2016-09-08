using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class InfSystem : MonoBehaviour {

	public InfZone[] zones;
	public InfActor[] actors;
	public int zoneTransferEveryNthFrame = 60;

	static InfSystem _instance;

    void Start () {
		zones = GameObject.FindObjectsOfType<InfZone>();
		actors = GameObject.FindObjectsOfType<InfActor>();
		foreach(var a in actors) {
			a.TransferToClosestZone(zones);
		}
    }

    void Update () {
		foreach(var a in actors) {
			a.OccasionallyCheckForClosestZone(zones, zoneTransferEveryNthFrame);
		}
    }

	static InfSystem instance {
		get {
			if(_instance == null) {
				_instance = GameObject.FindObjectOfType<InfSystem>();
				if(_instance == null) {
					throw new UnityException("No instance of InfSystem could be found in the scene");
				}
			}
			return _instance;
		}
	}
}
