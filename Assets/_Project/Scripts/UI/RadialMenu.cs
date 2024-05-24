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

    public List<Transform> activeChildren;

    private void Awake()
    {
        actualRadius = radius;
    }

    private void Update()
    {
        UpdateActiveChildren();
        AlignChildren();
    }

    private void UpdateActiveChildren() 
    {
        activeChildren.Clear();
        foreach (Transform child in optionsGroup) 
        {
            if (child.gameObject.activeSelf) activeChildren.Add(child);
        }
    }

    private void AlignChildren() 
    {
        if (activeChildren.Count <= 0) return;

        float step = (360f / activeChildren.Count) * complition;

        for (int i = 0; i < activeChildren.Count; i++) 
        {
            Vector3 point = new Vector3(0, actualRadius, 0);
            point = Quaternion.AngleAxis(-step * i, optionsGroup.forward) * point;

            activeChildren[i].position = point + optionsGroup.position;
        }
    }
}
