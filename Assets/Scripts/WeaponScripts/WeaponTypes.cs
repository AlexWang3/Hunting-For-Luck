using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponType", menuName = "Metroidvania/Weapons", order = 1)]
public class WeaponTypes : ScriptableObject
{
    public GameObject projectile;
    public float projectileSpeed;
    public int amountToPool;
    public float lifeTime;
}

