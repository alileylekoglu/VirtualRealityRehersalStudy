using UnityEngine;
using Photon.Pun;
using System.Collections.Generic;
using TMPro; // Required for TextMeshPro elements

public class ObjectSpawnManager : MonoBehaviourPunCallbacks
{
    // Array of prefabs that can be spawned
    public GameObject[] spawnablePrefabs;

    // Array of TMP_Text elements corresponding to the UI display for each object count
    public TMP_Text[] objectCountTexts;

    // The spawn position relative to the player
    public Vector3 spawnPositionOffset = new Vector3(0, 0, 2);

    // Dictionary to keep track of spawned objects count for each type
    private Dictionary<GameObject, int> spawnedObjectsCount = new Dictionary<GameObject, int>();

    // Maximum number of instances for each object type that can be in the scene
    private const int MaxInstancesPerObject = 5;

    private void Start()
    {
        // Initialize the dictionary with the prefab counts set to 0
        foreach (var prefab in spawnablePrefabs)
        {
            spawnedObjectsCount[prefab] = 0;
        }
        UpdateUI();
    }

    public void SpawnSpecificObject(int prefabIndex)
    {
        if (prefabIndex >= 0 && prefabIndex < spawnablePrefabs.Length)
        {
            GameObject prefabToSpawn = spawnablePrefabs[prefabIndex];
            if (prefabToSpawn != null)
            {
                if (spawnedObjectsCount[prefabToSpawn] < MaxInstancesPerObject)
                {
                    Vector3 spawnPosition = CalculateSpawnPosition();
                    PhotonNetwork.Instantiate(prefabToSpawn.name, spawnPosition, Quaternion.identity);
                    spawnedObjectsCount[prefabToSpawn]++;
                    UpdateUI();
                }
                else
                {
                    Debug.Log("Maximum instances of this object have been spawned.");
                }
            }
            else
            {
                Debug.LogError("The specified prefab is not assigned in the spawnablePrefabs array.");
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
            if (prefabToDecrease != null && spawnedObjectsCount[prefabToDecrease] > 0)
            {
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

    // Call this method to decrease the count when an object is destroyed
    public void OnObjectDestroyed(GameObject destroyedObject)
    {
        // You'll need to find the prefab that this destroyed object corresponds to.
        // This can be done by comparing names, storing a reference upon instantiation, or other means.
        GameObject prefabType = FindPrefabType(destroyedObject);
        if (prefabType != null && spawnedObjectsCount.ContainsKey(prefabType))
        {
            spawnedObjectsCount[prefabType]--;
            UpdateUI();
        }
    }

    private GameObject FindPrefabType(GameObject destroyedObject)
    {
        // Here you need logic to find which prefab the destroyed object corresponds to.
        // This is placeholder logic; adjust it to match your project's needs.
        string nameToCheck = destroyedObject.name.Replace("(Clone)", "").Trim();
        foreach (var prefab in spawnablePrefabs)
        {
            if (prefab.name == nameToCheck)
            {
                return prefab;
            }
        }
        return null;
    }

    private void UpdateUI()
    {
        for (int i = 0; i < spawnablePrefabs.Length; i++)
        {
            if (i < objectCountTexts.Length)
            {
                int remainingCount = MaxInstancesPerObject - spawnedObjectsCount[spawnablePrefabs[i]];
                objectCountTexts[i].text = $"{remainingCount} / 5";
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
