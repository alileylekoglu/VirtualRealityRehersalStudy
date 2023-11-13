using UnityEngine;
using Photon.Pun;
using System.Collections.Generic;
using TMPro;

public class ObjectSpawnManager : MonoBehaviourPunCallbacks
{
    public GameObject[] spawnablePrefabs;
    public TMP_Text[] objectCountTexts;
    public Vector3 spawnPositionOffset = new Vector3(0, 0, 2);
    private Dictionary<GameObject, int> spawnedObjectsCount = new Dictionary<GameObject, int>();
    private Dictionary<GameObject, Stack<GameObject>> spawnedInstances = new Dictionary<GameObject, Stack<GameObject>>();
    private const int MaxInstancesPerObject = 5;

    private void Start()
    {
        foreach (var prefab in spawnablePrefabs)
        {
            spawnedObjectsCount[prefab] = 0;
            spawnedInstances[prefab] = new Stack<GameObject>();
        }
        UpdateUI();
    }

    public void SpawnSpecificObject(int prefabIndex)
    {
        if (prefabIndex >= 0 && prefabIndex < spawnablePrefabs.Length)
        {
            GameObject prefabToSpawn = spawnablePrefabs[prefabIndex];
            if (prefabToSpawn != null && spawnedObjectsCount[prefabToSpawn] < MaxInstancesPerObject)
            {
                Vector3 spawnPosition = CalculateSpawnPosition();
                GameObject instance = PhotonNetwork.Instantiate(prefabToSpawn.name, spawnPosition, Quaternion.identity);
                spawnedObjectsCount[prefabToSpawn]++;
                spawnedInstances[prefabToSpawn].Push(instance);
                UpdateUI();
            }
            else
            {
                Debug.Log("Maximum instances of this object have been spawned or prefab is null.");
            }
        }
        else
        {
            Debug.LogError("The specified index is out of range of the spawnablePrefabs array.");
        }
    }

    public void DecreaseObjectCount(int prefabIndex)
    {
        if (prefabIndex >= 0 && prefabIndex < spawnablePrefabs.Length)
        {
            GameObject prefabToDecrease = spawnablePrefabs[prefabIndex];
            if (prefabToDecrease != null && spawnedInstances[prefabToDecrease].Count > 0)
            {
                GameObject instanceToDestroy = spawnedInstances[prefabToDecrease].Pop();
                PhotonNetwork.Destroy(instanceToDestroy);
                spawnedObjectsCount[prefabToDecrease]--;
                UpdateUI();
            }
            else
            {
                Debug.Log("No instances of this object exist or prefab is null.");
            }
        }
        else
        {
            Debug.LogError("The specified index is out of range of the spawnablePrefabs array.");
        }
    }

    private void UpdateUI()
    {
        for (int i = 0; i < spawnablePrefabs.Length; i++)
        {
            if (i < objectCountTexts.Length)
            {
                int remainingCount = MaxInstancesPerObject - spawnedObjectsCount[spawnablePrefabs[i]];
                objectCountTexts[i].text = $"{MaxInstancesPerObject}";
            }
        }
    }

    private Vector3 CalculateSpawnPosition()
    {
        Camera playerCamera = Camera.main;
        Vector3 spawnPosition = playerCamera.transform.position + playerCamera.transform.forward * spawnPositionOffset.z;
        return spawnPosition;
    }
}
