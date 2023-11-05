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
        spawnButton.onClick.AddListener(SpawnObject);
        resetButton.onClick.AddListener(ResetObjects);

        UpdateCounterText();
    }

    private void SpawnObject()
    {
        if (!PhotonNetwork.IsConnected || !photonView.IsMine) return;  // Ensure the player is connected and owns this view

        if (currentSpawnCount < maxSpawnCount)
        {
            Vector3 spawnPosition = new Vector3(
                Random.Range(spawnAreaCenter.x - spawnAreaSize.x / 2, spawnAreaCenter.x + spawnAreaSize.x / 2),
                Random.Range(spawnAreaCenter.y - spawnAreaSize.y / 2, spawnAreaCenter.y + spawnAreaSize.y / 2),
                Random.Range(spawnAreaCenter.z - spawnAreaSize.z / 2, spawnAreaCenter.z + spawnAreaSize.z / 2)
            );

            GameObject spawned = PhotonNetwork.Instantiate(objectToSpawn.name, spawnPosition, Quaternion.identity);
            spawnedObjects.Add(spawned);

            currentSpawnCount++;

            UpdateCounterText();

            if (currentSpawnCount >= maxSpawnCount)
            {
                spawnButton.interactable = false;
            }
        }
    }

    [PunRPC]
    private void ResetObjects()
    {
        if (!PhotonNetwork.IsConnected || !photonView.IsMine) return;  // Ensure the player is connected and owns this view

        foreach (GameObject obj in spawnedObjects)
        {
            PhotonNetwork.Destroy(obj);
        }
        spawnedObjects.Clear();

        currentSpawnCount = 0;
        UpdateCounterText();

        spawnButton.interactable = true;
    }

    private void UpdateCounterText()
    {
        counterText.text = $"Spawned: {currentSpawnCount}/{maxSpawnCount}";
    }
}
