using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bag : MonoBehaviour
{
    public int maxWeight;
    public List<Item> itemList;

    private int[,] memo;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        int n = itemList.Count;
        memo = new int[n + 1, maxWeight + 1];
        for (int i = 0; i <= n; i++)
        {
            for (int w = 0; w <= maxWeight; w++)
            {
                memo[i, w] = -1;
            }
        }

        int result = TotalValue(0, maxWeight);
        Debug.Log("Max value = " + result);
    }

    public int TotalValue(int i, int remainingWeight)
    {
        if (i == itemList.Count || remainingWeight == 0)
        {
            return 0;
        }

        if (memo[i, remainingWeight] != -1)
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
        
        //Debug.Log(itemList[i].name);
        return memo[i, remainingWeight];
        
    }
}