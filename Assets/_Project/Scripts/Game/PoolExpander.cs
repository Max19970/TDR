using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolExpander : MonoBehaviour
{
    [SerializeField] private int count;

    private void Awake()
    {
        GameObject child = transform.GetChild(0).gameObject;

        for (int i = 0; i < count - 1; i++) 
        {
            Instantiate(child, transform);
        }
    }
}
