using Photon.Pun;
using UnityEngine;
using UnityEngine.XR;

public class HandTrackingNetworkSync : MonoBehaviourPun, IPunObservable
{
    public XRNode handNode; // Assign this to XRNode.LeftHand or XRNode.RightHand accordingly

    private Vector3 localPosition;
    private Quaternion localRotation;

    // The Start method is called on the frame when a script is enabled just before any of the Update methods are called the first time.
    void Start()
    {
        if (photonView.IsMine)
        {
            // Initialize with the current hand position and rotation if this is our local player's hand
            UpdateLocalHandTracking();
        }
    }

    // The Update method is called every frame, if the MonoBehaviour is enabled.
    void Update()
    {
        if (photonView.IsMine)
        {
            UpdateLocalHandTracking();
        }
        else
        {
            // Update the hand representation for remote players
            transform.localPosition = localPosition;
            transform.localRotation = localRotation;
        }
    }

    private void UpdateLocalHandTracking()
    {
        // Update the local hand tracking
        InputDevice device = InputDevices.GetDeviceAtXRNode(handNode);
        device.TryGetFeatureValue(CommonUsages.devicePosition, out localPosition);
        device.TryGetFeatureValue(CommonUsages.deviceRotation, out localRotation);

        // Apply the position and rotation to the transform
        transform.localPosition = localPosition;
        transform.localRotation = localRotation;
    }

    // This method is called by PUN with a PhotonStream and a PhotonMessageInfo to write or read data for synchronization.
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // We own this player: send the others our data
            stream.SendNext(transform.localPosition);
            stream.SendNext(transform.localRotation);
        }
        else
        {
            // Network player, receive data
            localPosition = (Vector3)stream.ReceiveNext();
            localRotation = (Quaternion)stream.ReceiveNext();
        }
    }
}
