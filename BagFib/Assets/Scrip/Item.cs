using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public int weight;
    public int value;

    public Item(int weight, int value)
    {
        this.weight = weight;
        this.value = value;
    }
}

