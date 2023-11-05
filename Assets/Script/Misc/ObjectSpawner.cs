using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using System.Collections.Generic;
using TMPro;

public class ObjectSpawner : MonoBehaviourPunCallbacks
{
    public GameObject objectToSpawn;
    public Button spawnButton;
    public Button resetButton;
    public Vector3 spawnAreaCenter;
    public Vector3 spawnAreaSize;
    public TextMeshProUGUI counterText;

    private int maxSpawnCount = 10;
    private int currentSpawnCount = 0;
    private List<GameObject> spawnedObjects = new List<GameObject>();

    private void Start()
    {
        spawnButton.onClick.AddListener(RequestSpawnObject);
        resetButton.onClick.AddListener(() => photonView.RPC("ResetObjects", RpcTarget.All));

        UpdateCounterText();
    }

    public void RequestSpawnObject()
    {
        if (PhotonNetwork.IsConnected && PhotonNetwork.IsMasterClient && currentSpawnCount < maxSpawnCount)
        {
            photonView.RPC("SpawnObject", RpcTarget.MasterClient);
        }
    }

    [PunRPC]
    private void SpawnObject()
    {
        Vector3 spawnPosition = new Vector3(
            Random.Range(spawnAreaCenter.x - spawnAreaSize.x / 2, spawnAreaCenter.x + spawnAreaSize.x / 2),
            Random.Range(spawnAreaCenter.y - spawnAreaSize.y / 2, spawnAreaCenter.y + spawnAreaSize.y / 2),
            Random.Range(spawnAreaCenter.z - spawnAreaSize.z / 2, spawnAreaCenter.z + spawnAreaSize.z / 2)
        );

        GameObject spawned = PhotonNetwork.Instantiate(objectToSpawn.name, spawnPosition, Quaternion.identity);
        spawnedObjects.Add(spawned);

        currentSpawnCount++;

        // Call an RPC to update counter for all players
        photonView.RPC("UpdateSpawnCount", RpcTarget.All, currentSpawnCount);
    }

    [PunRPC]
    private void UpdateSpawnCount(int count)
    {
        currentSpawnCount = count;
        UpdateCounterText();

        spawnButton.interactable = currentSpawnCount < maxSpawnCount;
    }

    [PunRPC]
    private void ResetObjects()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            foreach (GameObject obj in spawnedObjects)
            {
                PhotonNetwork.Destroy(obj);
            }
        }

        spawnedObjects.Clear();
        currentSpawnCount = 0; // Reset the count
        photonView.RPC("UpdateSpawnCount", RpcTarget.All, currentSpawnCount); // Update the count across all clients
    }

    private void UpdateCounterText()
    {
        counterText.text = $"Spawned: {currentSpawnCount}/{maxSpawnCount}";
    }
}
