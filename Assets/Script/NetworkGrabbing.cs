using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;


public class NetworkGrabbing : MonoBehaviourPunCallbacks, IPunOwnershipCallbacks
{
    PhotonView m_photonView;

    Rigidbody rb;
    
    bool isBeingHeld = false;
    private void Awake()
    {
        m_photonView = GetComponent<PhotonView>();
    }
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (isBeingHeld)
        {
            rb.isKinematic = true;
            gameObject.layer = 8;
        }
        else
        {
            rb.isKinematic = false;
            gameObject.layer = 0;
        }
    }

    private void TrasferOwnership()
    {
       m_photonView.RequestOwnership();
    }

    public void OnSelectEntered()
    {
        Debug.Log("Grabbed");
        m_photonView.RPC("StartNetworkGrabbing", RpcTarget.AllBuffered);
        if (m_photonView.Owner == PhotonNetwork.LocalPlayer)
        {
            Debug.Log("We don't request ownership. It has already mine");
        }
        TrasferOwnership();
        
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
      Debug.Log("Ownership Transfered for " + targetView.name + " from " + previousOwner.NickName);
    }

    public void OnOwnershipTransferFailed(PhotonView targetView, Player senderOfFailedRequest)
    {
      
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
