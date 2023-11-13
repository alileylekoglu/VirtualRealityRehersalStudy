using UnityEngine;

public class FollowObject : MonoBehaviour
{
    public Transform target; // Assign the target GameObject's Transform here in the Inspector
    public Vector3 offset;   // Define the offset from the target object

    // Update is called once per frame
    private void Update()
    {
        if (target != null)
        {
            transform.position = target.position + offset;
        }
    }
}