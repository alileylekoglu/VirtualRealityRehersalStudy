using UnityEngine;
using Photon.Pun;

public class ColorChanger : MonoBehaviourPunCallbacks // inherit from MonoBehaviourPunCallbacks to use Photon's callbacks
{
    public Material[] materials; // Assign your 8 materials here in the Unity Inspector
    private Renderer rend;
    private int currentMaterialIndex = 0; // Default material index

    private void Start()
    {
        rend = GetComponent<Renderer>();

        // Get the current material index from custom properties if available
        if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("materialIndex"))
        {
            currentMaterialIndex = (int)PhotonNetwork.CurrentRoom.CustomProperties["materialIndex"];
            ChangeMaterial(currentMaterialIndex);
        }
    }

    public override void OnJoinedRoom()
    {
        // Synchronize the material when joining the room
        ChangeMaterial(currentMaterialIndex);
    }

    public void SetMaterialByIndex(int materialIndex)
    {
        // Change the material locally and inform other clients
        ChangeMaterial(materialIndex);

        // Set the custom property for the material index
        ExitGames.Client.Photon.Hashtable props = new ExitGames.Client.Photon.Hashtable
        {
            { "materialIndex", materialIndex }
        };
        PhotonNetwork.CurrentRoom.SetCustomProperties(props);

        // Call the RPC to inform other clients of the material change
        photonView.RPC("RpcChangeMaterial", RpcTarget.OthersBuffered, materialIndex);
    }

    void ChangeMaterial(int materialIndex)
    {
        // Ensure the material index is valid
        if(materialIndex >= 0 && materialIndex < materials.Length)
        {
            rend.material = materials[materialIndex];
            currentMaterialIndex = materialIndex; // Update the current material index
        }
    }

    [PunRPC]
    void RpcChangeMaterial(int materialIndex)
    {
        ChangeMaterial(materialIndex);
    }
}