using UnityEngine;
using System.Collections;

public interface UpdateableWhileFrozen {
	void UpdateWhileFrozen();
}

public class InfActor : MonoBehaviour {

	public InfZone zone;
	public System.Action<InfZone> onFreezeAction;
	public System.Action<InfZone> onUnfreezeAction;
	int randomFrameCountOffset;
	System.Action onUpdateWhileFrozen;

	void Awake() {
		randomFrameCountOffset = Random.Range(0, 60);
		SetupOnUpdateWhileFrozen();
	}

	void SetupOnUpdateWhileFrozen () {
		var updateableComponents = GetComponents<UpdateableWhileFrozen>();
		foreach(var u in updateableComponents) {
			onUpdateWhileFrozen += u.UpdateWhileFrozen;
		}
	}

	public void UpdateWhileFrozen () {
		onUpdateWhileFrozen();
	}

	/// <summary>
	/// Alters the activity status of the actor's GameObject. 
	/// If the status is changed from it's previous value the approperiate callback is called (FixAfterFreeze / FixAfterUnfreeze).
	/// </summary>
	public void SetActive (bool newActiveStatus, InfZone zone) {
		if (!isActiveAndEnabled && newActiveStatus) {
			FixAfterUnfreeze (zone);
		} else if (isActiveAndEnabled && !newActiveStatus) {
			FixAfterFreeze (zone);
		}
		gameObject.SetActive (newActiveStatus);
	}

	/// <summary>
	/// Whether an actor is frozen or not depends solely on if its Zone is frozen. It can't be set from the actor itself.
	/// An actor always has exactly one zone that it is contained inside. 
	/// Zones might overlap geographicly though, and many zones can be active at the same time.
	/// </summary>
	public bool frozen {
		get {
			if (zone == null) {
				throw new UnityException (name + "'s zone == null");
			}
			return zone.frozen;
		}
	}

	/// <summary>
	/// The correct way to move an actor from its current Zone to another one.
	/// </summary>
	public void TransferToZone(InfZone newZone) {
		if(zone == newZone) {
			return; // Already in that zone
		}
		if(zone != null) {
			zone.RemoveActor (this);
		}
		newZone.AddActor (this);
		zone = newZone;
		SetActive (!zone.frozen, zone);
	}

	public void TransferToClosestZone(InfZone[] zones) {
		if (zones.Length == 0) {
			throw new UnityException ("No zones to choose from");
		}
		float minimum = float.MaxValue;
		InfZone closest = null;
		foreach (var z in zones) {
			float distance = Vector3.Distance (transform.position, z.transform.position);
			if (distance < minimum) {
				minimum = distance;
				closest = z;
			}
		}
		TransferToZone(closest);
	}

	public void OccasionallyCheckForClosestZone(InfZone[] zones, int everyNthFrame) {
		if((randomFrameCountOffset + Time.frameCount) % everyNthFrame == 0) {
			TransferToClosestZone(zones);
		}
	}

	public void FixAfterUnfreeze (InfZone zone) {
		onUnfreezeAction (zone);
	}

	public void FixAfterFreeze (InfZone zone) {
		onFreezeAction (zone);
	}
}
