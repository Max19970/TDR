using NaughtyAttributes;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Gun_Data", menuName = "ScriptableObjects/Gun Data")]
public class GunData : ScriptableObject 
{
    [Header("Gun Settings")]
    public new string name;
    public float aimRadius = 2f;
    [Space]
    public float shootCooldown = 1f;
    public float bulletDamage;
    public float bulletSpeed;
    [Space]
    public bool aim = true;
    public float rotationSpeed = 5f;
    public float shootingFOV = 20f;
    [Space]
    public Sprite gunSprite;
    public Sprite shopSprite;
    public Vector3 gunPointLocalPosition;
    [Space]
    public List<GunData> menuOptions;
    [Space]
    public int shopCost;
    public int totalCost;
}