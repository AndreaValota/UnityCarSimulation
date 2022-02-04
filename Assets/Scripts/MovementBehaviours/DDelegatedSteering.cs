using System.Collections.Generic;
using UnityEngine;


[RequireComponent (typeof (Rigidbody))]
public class DDelegatedSteering : MonoBehaviour {

	public float minLinearSpeed = 5f;
	public float maxLinearSpeed = 40f;
	public float maxAngularSpeed = 100f;

	private MovementStatus status;

	private void Start () {
		status = new MovementStatus ();
	}

	void FixedUpdate () {

		status.movementDirection = transform.forward;

		// Get the destination point based on the behaviour attached to the object
		PathFollowing pathStrategy = GetComponent<PathFollowing>();
		Vector3 destination = pathStrategy.GetDestination(status);

		// Contact al behaviours and build a list of directions
		List<Vector3> components = new List<Vector3> ();
		foreach (MovementBehaviour mb in GetComponents<MovementBehaviour> ())
			components.Add (mb.GetAcceleration (status, destination));

		// Blend the list to obtain a single acceleration to apply
		Vector3 blendedAcceleration = Blender.Blend (components);

		// if we have an acceleration, apply it
		if (blendedAcceleration.magnitude != 0f) {
			Driver.Steer (GetComponent<Rigidbody> (), status, blendedAcceleration,
				          minLinearSpeed, maxLinearSpeed, maxAngularSpeed);
		}
	}

	private void OnDrawGizmos () {
		if (status != null) {
			UnityEditor.Handles.Label (transform.position + 2f * transform.up, status.linearSpeed.ToString () + "\n" + status.angularSpeed.ToString());
		}
	}

}
