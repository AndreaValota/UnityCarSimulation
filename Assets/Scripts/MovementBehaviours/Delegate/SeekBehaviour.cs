using UnityEngine;

public class SeekBehaviour : MovementBehaviour {

	public float gas = 20f;
	public float steer = 30f;
	public float brake = 20f;

	public float brakeAtAngle = 20f;
	public float stopAt = 0.01f;

	public override Vector3 GetAcceleration (MovementStatus status, Vector3 destination) {

		if (destination != null) {
			//Vertical adjustment
			Vector3 verticalAdj = new Vector3 (destination.x, transform.position.y, destination.z);
			Vector3 toDestination = (verticalAdj - transform.position);

			if (toDestination.magnitude > stopAt)
			{
				Vector3 tangentComponent = Vector3.Project(toDestination.normalized, status.movementDirection);
				Vector3 normalComponent = (toDestination.normalized - tangentComponent);

				//If the destination is at BreakAngle degree from the movement direction it means we are doing a sharp turn and we need to break
				bool brakeCheck = (Vector3.Angle(status.movementDirection, toDestination) < brakeAtAngle);

				return (tangentComponent * (brakeCheck ? gas : -brake)) + (normalComponent * steer);
			}
			else {
				return Vector3.zero;
			}

		} else {
			return Vector3.zero;
		}
	}

}
