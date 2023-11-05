using Photon.Pun;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class NetworkedGrabInteractable : XRGrabInteractable, IPunObservable
{
    private PhotonView photonView;
    private Rigidbody rb;
    private Vector3 correctNetworkedPosition;
    private Quaternion correctNetworkedRotation;
    private float lerpRate = 15;

    protected override void Awake()
    {
        base.Awake();
        photonView = GetComponent<PhotonView>();
        rb = GetComponent<Rigidbody>();
        correctNetworkedPosition = transform.position;
        correctNetworkedRotation = transform.rotation;
    }

    public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
    {
        base.ProcessInteractable(updatePhase);

        if (updatePhase == XRInteractionUpdateOrder.UpdatePhase.Fixed)
        {
            if (isSelected && !photonView.IsMine)
            {
                photonView.RequestOwnership();
            }
        }

        if (!photonView.IsMine)
        {
            transform.position = Vector3.Lerp(transform.position, correctNetworkedPosition, Time.deltaTime * lerpRate);
            transform.rotation = Quaternion.Lerp(transform.rotation, correctNetworkedRotation, Time.deltaTime * lerpRate);
        }
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);
        rb.useGravity = false;
        rb.isKinematic = true;
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);
        rb.useGravity = true;
        rb.isKinematic = false;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        else
        {
            correctNetworkedPosition = (Vector3)stream.ReceiveNext();
            correctNetworkedRotation = (Quaternion)stream.ReceiveNext();

            float lag = Mathf.Abs((float)(PhotonNetwork.Time - info.SentServerTime));
            correctNetworkedPosition += (rb.velocity * lag);
        }
    }
}
