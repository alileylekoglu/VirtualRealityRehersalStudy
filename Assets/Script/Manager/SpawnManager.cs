using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpawnManager : MonoBehaviourPunCallbacks
{
    [SerializeField]
    GameObject GenericVRPlayerPrefab;
    
    public Vector3 spawnPosition; 
    void Start () 
    {
        Debug.Log("Started, creating Generic Vr Prefab if connection is right " + PhotonNetwork.NetworkClientState);
        if (PhotonNetwork.IsConnectedAndReady)
        {
            PhotonNetwork.Instantiate(GenericVRPlayerPrefab.name, spawnPosition, Quaternion.identity);
        }   
    }
 
    public override void OnJoinedRoom()
    {      
        Debug.Log("Joined room, creating Generic Vr Prefab");
        if (PhotonNetwork.IsConnectedAndReady)
        {
            PhotonNetwork.Instantiate(GenericVRPlayerPrefab.name, spawnPosition, Quaternion.identity);
        }        
}
}