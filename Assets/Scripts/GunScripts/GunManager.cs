using UnityEngine;
using System.Collections.Generic;

public class GunManager : MonoBehaviour
{
    public static GunManager Instance { get; private set; }

    [SerializeField] public GunData[] gunDataArray;
    private Dictionary<string, GunData> gunDataDictionary = new();

    private void Awake()
    {

        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        LoadGunData();
    }

    private void LoadGunData()
    {
        GameObject[] gunPrefabs = Resources.LoadAll<GameObject>("Prefabs/Guns");
        List<GunData> gunDataList = new List<GunData>();

        Debug.Log($"Found {gunPrefabs.Length} gun prefabs.");

        foreach (GameObject gunPrefab in gunPrefabs)
        {
            GameObject gunInstance = Instantiate(gunPrefab);

            //            if (gunPrefab.TryGetComponent<Gun>(out var gunComponent))
            if (gunInstance.TryGetComponent<Gun>(out var gunComponent))
            {
                GunData gunData = gunComponent.GetGunData();
                gunDataList.Add(gunData);
                gunDataDictionary[gunData.GunName] = gunData;
                Debug.Log($"GunPrefab {gunData.GunName} loaded.");
            }
            else Debug.Log("No Gun Component found in prefab");
//            else Debug.Log("No GunPrefabs found.");

            Destroy(gunInstance);
        }

        // Convert list to array so it appears in the Inspector
        gunDataArray = gunDataList.ToArray();

        Debug.Log($"GunData Dictionary Size: {gunDataDictionary.Count}");
        Debug.Log($"GunData Array Size: {gunDataArray.Length}");


    }

    public GunData GetGunData(string gunName)
    {
        //Get Gun and properties by gunName from GunData array
        return gunDataDictionary.TryGetValue(gunName, out GunData gunData) ? gunData : null;
    }
}

/**************************************************************************
 *                     Old code, partly not working
 *                     Use as reference if needed
 **************************************************************************                     
            // Create a new GunData instance
/*
            GunData gunData = new GunData
            {
                GunName = gunComponent.gunName,
                FireRate = gunComponent.fireRate,
                AmmoCount = gunComponent.ammoCount,
                Ammonition = gunComponent.ammonition,
                Speed = gunComponent.speed,
                DestroyTime = gunComponent.destroyTime
                
            };
*/

//            gunDataDictionary[gunData.GunName] = gunData;
//            gunDataList.Add(gunData);



/******************* //Debugging
        GameObject[] gunPrefabs = Resources.LoadAll<GameObject>("Prefabs/Guns");
        gunDataDictionary = new Dictionary<string, GunData>();

        Debug.Log($"Found {gunPrefabs.Length} gun prefabs.");

        foreach (GameObject gunPrefab in gunPrefabs)
        {
            if (gunPrefab == null)
            {
                Debug.LogError("Gun prefab is null!");
                continue;
            }

            Gun gunComponent = gunPrefab.GetComponent<Gun>();

            if (gunComponent == null)
            {
                Debug.LogError($"Gun component missing on prefab: {gunPrefab.name}");
                continue;
            }

            GunData gunData = gunComponent.GetGunData();

            if (gunData == null)
            {
                Debug.LogError($"GunData is null for {gunPrefab.name}");
                continue;
            }

            Debug.Log($"Loaded Gun: {gunData.GunName}");

            if (string.IsNullOrEmpty(gunData.GunName))
            {
                Debug.LogError($"GunName is null or empty for {gunPrefab.name}");
                continue;
            }

            if (!gunDataDictionary.ContainsKey(gunData.GunName))
            {
                gunDataDictionary[gunData.GunName] = gunData;
                Debug.Log($"Added gun: {gunData.GunName}");
            }
            else
            {
                Debug.LogWarning($"Duplicate gun name found: {gunData.GunName}");
            }



        }

        Debug.Log($"GunData Dictionary Size: {gunDataDictionary.Count}");
**************************/

/*********************
//Almost working solution...

GameObject[] gunPrefabs = Resources.LoadAll<GameObject>("Prefabs/Guns");
List<GunData> gunList = new List<GunData>();
Debug.Log($"Begin loading guns...");

foreach (GameObject gunPrefab in gunPrefabs)
{
    if (gunPrefab.TryGetComponent<Gun>(out var gunComponent))
    {
        GunData gunData = gunComponent.GetGunData();
        gunList.Add(gunData);
        gunDataDictionary[gunData.GunName] = gunData;
        Debug.Log($"GunPrefab {gunData.GunName} loaded.");
    }
    else Debug.Log("No GunPrefabs found.");
}

gunDataArray = gunList.ToArray();
Debug.Log("Loading GunPrefabs done.");
**********************/

//            Gun gunComponent = gunPrefab.GetComponent<Gun>();
/*
            if (gunComponent == null)
            {
                Debug.LogError($"Gun component missing on prefab: {gunPrefab.name}");
                continue;
            }

            GunData gunData = gunComponent.GetComponent<GunData>();
            gunDataList.Add(gunData);
            gunDataDictionary[gunData.GunName] = gunData;
*/
/*
            // Ensure GunName is valid
            if (string.IsNullOrEmpty(gunData.GunName))
            {
                Debug.LogError($"GunName is null or empty for {gunPrefab.name}!");
                continue;
            }

            Debug.Log($"Loaded Gun: {gunData.GunName}");

            gunDataDictionary[gunData.GunName] = gunData;
            gunDataList.Add(gunData);
*/
