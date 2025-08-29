using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bag : MonoBehaviour
{
    public int maxWeight;
    public List<Item> itemList;

    private int[,] dp;
    
    private List<Item> selectedItems = new List<Item>();
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        int result = TotalValue();
        Debug.Log("Max value = " + result);
        
        
        
        GetSelectedItems();

        foreach (var item in selectedItems)
        {
            StartCoroutine(MoveToBag(item.transform, transform.position));
            Debug.Log(item.name);
        }
    }

    public int TotalValue()
    {
        int n = itemList.Count;
        
        dp = new int[n + 1, maxWeight + 1];

        for (int i = 1; i <= n; i++)
        {
            for (int j = 0; j <= maxWeight; j++)
            {
                if (itemList[i - 1].weight > j)
                {
                    dp[i, j] = dp[i - 1, j];
                }
                else
                {
                    int takeI = itemList[i - 1].value + dp[i - 1, j - itemList[i - 1].weight];
                    
                    int nottakeI = dp[i - 1, j];

                    dp[i, j] = Mathf.Max(takeI, nottakeI);
                }
            }
        }
        return dp[n, maxWeight];
    }
    
    private void GetSelectedItems()
    {
        selectedItems.Clear();

        int i = itemList.Count;
        int j = maxWeight;

        while (i > 0 && j > 0)
        {
            if (dp[i, j] != dp[i - 1, j])
            {
                Item selected = itemList[i - 1];
                selectedItems.Add(selected);
                j -= selected.weight;
            }
            i--;
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
    }
}