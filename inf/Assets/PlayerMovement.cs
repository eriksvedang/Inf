using UnityEngine;
using System.Collections.Generic;

public class PlayerMovement : MonoBehaviour {
	
    void Update () {
		transform.Translate(new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")));
    }

}
