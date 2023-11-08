using Photon.Pun;
using UnityEngine;

// This class is responsible for synchronizing hand tracking data across the network.
public class HandTrackingSynchronization : MonoBehaviour, IPunObservable
{
    // Reference to the PhotonView component.
    private PhotonView photonView;

    
    // Local representation of the hand data.
    // This should be replaced with your actual hand data variables.
    private Vector3 localHandPosition;
    private Quaternion localHandRotation;

    // Smoothing factor for the synchronization.
    public float positionLerpRate = 5f;
    public float rotationLerpRate = 5f;

    private void Start()
    {
        // Get the PhotonView component from the GameObject.
        photonView = GetComponent<PhotonView>();
    }

    private void Update()
    {
        // Check if the PhotonView is ours to control.
        if (photonView.IsMine)
        {
            // Here you would get the hand's position and rotation from the tracking system.
            // For example:
            // localHandPosition = handTrackingSystem.LocalHandPosition;
            // localHandRotation = handTrackingSystem.LocalHandRotation;
        }
        else
        {
            // Smoothly update the hand's position and rotation for remote players.
            transform.position = Vector3.Lerp(transform.position, localHandPosition, Time.deltaTime * positionLerpRate);
            transform.rotation = Quaternion.Lerp(transform.rotation, localHandRotation, Time.deltaTime * rotationLerpRate);
        }
    }

    // Called by Photon to send and receive the hand tracking data.
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // Send our local hand data to the other players.
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        else
        {
            // Receive the remote hand data.
            localHandPosition = (Vector3)stream.ReceiveNext();
            localHandRotation = (Quaternion)stream.ReceiveNext();
        }
    }
}
