using System;
using UnityEngine;

public static class SaferGameObject
{
	public static T FindObjectOfType<T>() where T : class {
		T o = GameObject.FindObjectOfType(typeof(T)) as T;
		if(o == null) {
			throw new UnityException(string.Format("No instance of {0} could be found in the scene", typeof(T).ToString()));
		}
		return o;
	}
}