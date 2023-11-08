using UnityEngine;
using Photon.Pun;

public class ColorChanger : MonoBehaviourPun
{
    public Material[] materials; // Assign your 8 materials here in the Unity Inspector
    private Renderer rend;

    private void Start()
    {
        rend = GetComponent<Renderer>();
    }

    public void SetMaterialByIndex(int materialIndex)
    {
        // Change the material locally and inform other clients
        ChangeMaterial(materialIndex);
        
        // Call the RPC to inform other clients of the material change
        photonView.RPC("RpcChangeMaterial", RpcTarget.OthersBuffered, materialIndex);
    }

    void ChangeMaterial(int materialIndex)
    {
        // Ensure the material index is valid
        if(materialIndex >= 0 && materialIndex < materials.Length)
        {
            rend.material = materials[materialIndex];
        }
    }

    [PunRPC]
    void RpcChangeMaterial(int materialIndex)
    {
        ChangeMaterial(materialIndex);
    }
}