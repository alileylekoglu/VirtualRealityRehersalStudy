using UnityEngine;
using Photon.Pun;

public class ColorChanger : MonoBehaviourPun // inherit from MonoBehaviourPun to have a photonView reference
{
    public Material[] materials; // Assign your 8 materials here in the Unity Inspector
    private Renderer rend;
    private int currentMaterialIndex = 0; // Default material index

    private void Start()
    {
        rend = GetComponent<Renderer>();
        // Initialize with the current material index if available
        if (photonView.InstantiationData != null && photonView.InstantiationData.Length > 0)
        {
            currentMaterialIndex = (int)photonView.InstantiationData[0];
        }
        ChangeMaterial(currentMaterialIndex); // Change the material locally
    }

    public void SetMaterialByIndex(int materialIndex)
    {
        // Change the material locally
        ChangeMaterial(materialIndex);

        // Only the master client should update the room properties to avoid conflicts
        if (PhotonNetwork.IsMasterClient)
        {
            ExitGames.Client.Photon.Hashtable props = new ExitGames.Client.Photon.Hashtable
            {
                { photonView.ViewID.ToString(), materialIndex }
            };
            PhotonNetwork.CurrentRoom.SetCustomProperties(props);
        }

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
    void RpcChangeMaterial(int materialIndex, PhotonMessageInfo info)
    {
        // Ensure that the RPC is only applied to the intended object
        if (info.photonView.ViewID == photonView.ViewID)
        {
            ChangeMaterial(materialIndex);
        }
    }
}
