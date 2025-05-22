using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bag : MonoBehaviour
{
    public int maxWeight;
    public List<Item> itemList;

    private int[,] memo;
    
    private List<Item> selectedItems = new List<Item>();
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        int n = itemList.Count;
        memo = new int[n + 1, maxWeight + 1];
        for (int i = 0; i <= n; i++)
        {
            for (int w = 0; w <= maxWeight; w++)
            {
                memo[i, w] = 0;
            }
        }

        int result = TotalValue(0, maxWeight);
        Debug.Log("Max value = " + result);
        
        GetSelectedItems(0, maxWeight);

        foreach (var item in selectedItems)
        {
            StartCoroutine(MoveToBag(item.transform, transform.position));
            Debug.Log(item.name);
        }
    }

    public int TotalValue(int i, int remainingWeight)
    {
        if (i == itemList.Count || remainingWeight == 0)
        {
            return 0;
        }

        if (memo[i, remainingWeight] != 0)
        {
            return memo[i, remainingWeight];
        }
        
        int skip = TotalValue(i +1, remainingWeight);

        int take = 0;
        if (itemList[i].weight <= remainingWeight)
        {
            take = itemList[i].value + TotalValue(i + 1, remainingWeight - itemList[i].weight);
        }
        
        memo[i, remainingWeight] = Mathf.Max(take, skip);
        return memo[i, remainingWeight];
        
    }
    
    private void GetSelectedItems(int i, int remainingWeight)
    {
        while (i < itemList.Count && remainingWeight > 0)
        {
            if (itemList[i].weight <= remainingWeight)
            {
                int take = itemList[i].value + memo[i + 1, remainingWeight - itemList[i].weight];
                int skip = memo[i + 1, remainingWeight];

                if (take >= skip)
                {
                    selectedItems.Add(itemList[i]);
                    remainingWeight -= itemList[i].weight;
                    i++;
                    continue;
                }
            }
            i++;
        }
    }

    IEnumerator MoveToBag(Transform itemTransform, Vector3 bagPosition)
    {
        float duration = 1f;
        float elapsed = 0f;
        Vector3 startPos = itemTransform.position;

        while (elapsed < duration)
        {
            itemTransform.position = Vector3.Lerp(startPos, bagPosition, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        itemTransform.position = bagPosition;
    }
}