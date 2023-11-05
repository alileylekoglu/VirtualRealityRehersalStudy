using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class NetworkGrabbing : MonoBehaviourPunCallbacks, IPunOwnershipCallbacks
{
    private PhotonView m_photonView;
    private Rigidbody rb;
    private bool isBeingHeld = false;

    private void Awake()
    {
        m_photonView = GetComponent<PhotonView>();
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        rb.isKinematic = isBeingHeld;
        gameObject.layer = isBeingHeld ? 8 : 0;
    }

    private void TransferOwnership()
    {
        if (!m_photonView.IsMine)
        {
            m_photonView.RequestOwnership();
        }
    }

    public void OnSelectEntered()
    {
        Debug.Log("Grabbed");
        m_photonView.RPC("StartNetworkGrabbing", RpcTarget.AllBuffered);
        TransferOwnership();
    }
    
    public void OnSelectExited()
    {
        Debug.Log("Released");
        m_photonView.RPC("StopNetworkGrabbing", RpcTarget.AllBuffered);
    }

    public void OnOwnershipRequest(PhotonView targetView, Player requestingPlayer)
    {
        if (targetView != m_photonView)
        {
            return;
        }
        
        Debug.Log("Ownership Requested for " + targetView.name + " from " + requestingPlayer.NickName);
        m_photonView.TransferOwnership(requestingPlayer);
    }

    public void OnOwnershipTransfered(PhotonView targetView, Player previousOwner)
    {
        Debug.Log("Ownership Transferred for " + targetView.name + " from " + previousOwner.NickName);
    }

    public void OnOwnershipTransferFailed(PhotonView targetView, Player senderOfFailedRequest)
    {
        Debug.LogWarning("Ownership Transfer Failed for " + targetView.name);
    }
    
    [PunRPC]
    public void StartNetworkGrabbing()
    {
        isBeingHeld = true;
    }
    
    [PunRPC]
    public void StopNetworkGrabbing()
    {
        isBeingHeld = false;
    } 
}