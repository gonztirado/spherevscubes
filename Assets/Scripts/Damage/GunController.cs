using UnityEngine;
using System.Collections;

public class GunController : MonoBehaviour
{
    [Header("Settings")]
    public float fireSpeed;
    public GameObject bulletPrefab;
    public Transform bulletStart;
    
    [Header("Control")]
    public string fireAxis;
    
    private bool _canShoot = true;
    private float _shootTimer;

    void Update()
    {
        CheckFire();
    }

    private void CheckFire()
    {
        float fireAxisPressed = Input.GetAxisRaw(fireAxis);
        if (CanShoot() && fireAxisPressed != 0)
        {
            Instantiate(bulletPrefab, bulletStart.position, bulletStart.rotation);
            _canShoot = false;
            _shootTimer += 1 / fireSpeed;
        }
    }

    private bool CanShoot()
    {
        if (!_canShoot)
        {
            _shootTimer -= Time.deltaTime;
            if (_shootTimer <= 0)
                _canShoot = true;
        }
        return _canShoot;
    }
}