using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy", menuName = "Enemy")]
public class Enemy : ScriptableObject
{
    public GameObject _enemyPrefab;
    public string _name;

    public int _level;
    public int _health;
    public int _damage;
    public float _enemySpeed;

    public void EnemySpawn() 
    {
        Instantiate(_enemyPrefab);
    }
}
