using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using Photon.Pun;
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
    private List<GameObject> spawnedObjects = new List<GameObject>();

   private void Start()
{
    spawnButton.onClick.AddListener(() => TrySpawnObject());
    resetButton.onClick.AddListener(() => photonView.RPC("ResetObjects", RpcTarget.MasterClient));

    UpdateCounterText(spawnedObjects.Count); // Use spawnedObjects.Count directly
}

private void TrySpawnObject()
{
    if (PhotonNetwork.IsConnected && PhotonNetwork.IsMasterClient && spawnedObjects.Count < maxSpawnCount)
    {
        SpawnObject();
    }
}

    private void SpawnObject()
    {
        Vector3 spawnPosition = new Vector3(
            Random.Range(spawnAreaCenter.x - spawnAreaSize.x / 2, spawnAreaCenter.x + spawnAreaSize.x / 2),
            Random.Range(spawnAreaCenter.y - spawnAreaSize.y / 2, spawnAreaCenter.y + spawnAreaSize.y / 2),
            Random.Range(spawnAreaCenter.z - spawnAreaSize.z / 2, spawnAreaCenter.z + spawnAreaSize.z / 2)
        );

        GameObject spawned = PhotonNetwork.Instantiate(objectToSpawn.name, spawnPosition, Quaternion.identity);
        spawnedObjects.Add(spawned);

        UpdateCounterText(spawnedObjects.Count);

        // Update interactability of the spawn button
        spawnButton.interactable = spawnedObjects.Count < maxSpawnCount;
    }

    [PunRPC]
    private void ResetObjects()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            foreach (GameObject obj in spawnedObjects)
            {
                if (obj != null)
                {
                    PhotonNetwork.Destroy(obj);
                }
            }
            spawnedObjects.Clear();
            UpdateCounterText(0); // Reset the count
        }
    }

    private void UpdateCounterText(int count)
    {
        counterText.text = $"Spawned: {count}/{maxSpawnCount}";
        // Since this is also called at the start and after reset, update the button's interactability here
        spawnButton.interactable = count < maxSpawnCount;
    }
}
