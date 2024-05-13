using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Gun_Data", menuName = "ScriptableObjects/Gun Data")]
public class GunData : ScriptableObject 
{
    public float aimRadius = 2f;
    [Dropdown("aimingModes")] public string aimingMode;
    [Dropdown("shootModes")] public string shootMode;
    [Space]
    public float shootCooldown = 1f;
    public float bulletDamage;
    public float bulletSpeed;
    [Space]
    public float rotationSpeed = 5f;
    public float shootingFOV = 20f;
    [Space]
    public GameObject body;

    private List<string> aimingModes = new List<string>() { "Closest", "First in group", "Last in group", "None" };
    private List<string> shootModes = new List<string>() { "Physical Bullets", "Hitscan Shots", "None" };
}