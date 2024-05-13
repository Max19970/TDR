using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[ExecuteInEditMode]
public class RadialMenu : MonoBehaviour
{
    [Header("Technics")]
    [SerializeField] private Transform optionsGroup;

    [Space]

    [Min(0)]
    public float radius;
    public float actualRadius;

    [Space]

    [Range(0, 1)]
    public float complition;

    private void Awake()
    {
        actualRadius = radius;
    }

    private void Update()
    {
        AlignChildren();
        
    }

    private void AlignChildren() 
    {
        if (optionsGroup.childCount <= 0) return;

        float step = (360f / optionsGroup.childCount) * complition;

        for (int i = 0; i < optionsGroup.childCount; i++) 
        {
            Vector3 point = (optionsGroup.position + new Vector3(0, actualRadius, 0)) - optionsGroup.position;
            point = Quaternion.AngleAxis(-step * i, optionsGroup.forward) * point;

            optionsGroup.GetChild(i).position = point + optionsGroup.position;
        }
    }
}
