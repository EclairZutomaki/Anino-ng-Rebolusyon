using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WaterSource
{
    Lake,
    River,
    Ocean
}

public class FishingSystem : MonoBehaviour
{

    public static FishingSystem Instance { get; set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

    }

    public List<FishData> lakeFishList; // Cod
    public List<FishData> riverFishList; // Salmon
    public List<FishData> oceanFishList; // Tuna

    public bool isThereABite;
    bool hasPulled;

    public static event Action OnFishingEnd;

    internal void StartFishing(WaterSource waterSource)
    {
        StartCoroutine(FishingCoroutine(waterSource));
    }

    IEnumerator FishingCoroutine(WaterSource waterSource)
    {
        yield return new WaitForSeconds(3f);

        FishData fish = CalculateBite(waterSource);

        if (fish.fishName == "NoBite")
        {
            Debug.LogWarning("No fish caught");
            EndFishing();
        }
        else
        {
            Debug.LogWarning(fish.fishName + " is biting");
            StartCoroutine(StartFishStruggle(fish));
        }
    }

    IEnumerator StartFishStruggle(FishData fish)
    {
        isThereABite = true;

        // wait until player pulls the rod
        while (!hasPulled)
        {
            yield return null;
        }

        Debug.LogWarning("Start MiniGame");

    }

    public void SetHasPulled ()
    {
        hasPulled = true;   
    }

    private void EndFishing()
    {
        isThereABite = false;   
        hasPulled = false;

        // Trigger end fishing event
        OnFishingEnd?.Invoke();


    }

    private FishData CalculateBite(WaterSource waterSource)
    {
        List<FishData> availableFish = GetAvailableFish(waterSource);

        // Calculate total probability
        float totalProbability = 0f;
        foreach (FishData fish in availableFish) // tuna 5% salmon 20% nobite 10% = 35%
        {
            totalProbability += fish.probability;
        }

        // Generate random number between 0 and total probability
        int randomValue = UnityEngine.Random.Range(0, Mathf.FloorToInt(totalProbability) + 1); // 0 - 35 // 17
        Debug.Log("Random value is " + randomValue);

        // Loop through the fish and check if the random number falls into their probability range
        float cumulativeProbability = 0f;
        foreach (FishData fish in availableFish)
        {
            cumulativeProbability += fish.probability;
            if (randomValue <= cumulativeProbability)
            {
                // This fish is biting
                return fish;
            }
        }

        // This should never happen - Random number out of bounds
        return null;

    }

    private List<FishData> GetAvailableFish(WaterSource waterSource)
    {
        switch (waterSource)
        {
            case WaterSource.Lake:
                return lakeFishList;
            case WaterSource.River:
                return riverFishList;
            case WaterSource.Ocean:
                return oceanFishList;
            default:
                return null;
        }
    }


}