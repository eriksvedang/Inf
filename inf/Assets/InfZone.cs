using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;

public class InfZone : MonoBehaviour {

	public InfActor[] actors;

	[SerializeField, HideInInspector]
	bool _frozen = false;

	public bool frozen {
		get {
			return _frozen;
		}
	}

	void Awake() {
		if (frozen) {
			Freeze ();
		} else {
			Unfreeze ();
		}
	}

	void Update() {
		if (frozen) {
			foreach(var a in actors) {
				a.UpdateWhileFrozen();
			}
		}
	}

	public void AddActor(InfActor actor) {
		var actorsList = actors.ToList ();
		actorsList.Add (actor);
		actors = actorsList.ToArray ();
	}

	public void RemoveActor(InfActor actor) {
		var actorsList = actors.ToList ();
		actorsList.Remove (actor);
		actors = actorsList.ToArray ();
	}

	public void Freeze() {
		_frozen = true;
		SetActiveStatusOnActors (false);
	}

	public void Unfreeze() {
		_frozen = false;
		SetActiveStatusOnActors (true);
	}

	void SetActiveStatusOnActors(bool active) {
		foreach (var actor in actors) {
			actor.SetActive (active, this);
		}
	}

	public static InfZone ClosestZone (Vector3 position, InfZone[] zones)
	{
		if (zones.Length == 0) {
			throw new UnityException ("No zones to choose from");
		}
		float minimum = float.MaxValue;
		InfZone closest = null;
		foreach (var zone in zones) {
			float distance = Vector3.Distance (position, zone.transform.position);
			if (distance < minimum) {
				minimum = distance;
				closest = zone;
			}
		}
		return closest;
	}

	void OnDrawGizmos() {
		Gizmos.color = frozen ? Color.blue : Color.yellow;
		Gizmos.DrawSphere(transform.position, 1);
	}

	public override string ToString () {
		return string.Format ("{0} {1}: [{2}]", name, frozen ? " (frozen)" : "", string.Join(", ", actors.Select(a => a.name).ToArray()));
	}
}

[CustomEditor(typeof(InfZone))]
public class DrawWireArcEditor : Editor
{
	InfZone infZone {
		get {
			return target as InfZone;
		}
	}

	void OnSceneGUI() {
		Handles.Label(infZone.transform.position + Vector3.up * 2 + Vector3.forward * 2, infZone.ToString());
	}

	public override void OnInspectorGUI() {
		GUILayout.Label (infZone.frozen ? "FROZEN" : "Not frozen");

		if (infZone.frozen) {
			if (GUILayout.Button ("Unfreeze")) {
				infZone.Unfreeze ();
			}
		} else {
			if(GUILayout.Button("Freeze")) {
				infZone.Freeze ();
			}
		}

		DrawDefaultInspector ();
	}
}