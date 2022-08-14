using UnityEngine;

public class BodyPositioner : MonoBehaviour
{

	[SerializeField] private Transform head;
	[SerializeField] private float headOffset = 0.4f;
	[SerializeField] private float rotationSpeed = 0.1f;

	void Update()
	{
		var headForward = head.forward;
		Vector3 clampedForward = new Vector3(headForward.x, 0, headForward.z);
		transform.SetPositionAndRotation((head.position - (head.up * headOffset)),
			(Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(clampedForward), rotationSpeed)));
	}
}