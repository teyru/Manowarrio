using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAtacking : MonoBehaviour
{
    [SerializeField] private Transform _weaponTransform;

    [SerializeField] private GameObject _weaponPrefab;

    private void PlayerAttacking()
    {
        GameObject bullet = Instantiate(_weaponPrefab, _weaponTransform);
        bullet.transform.parent = null;
    }
}
