using UnityEngine;
using System.Collections;

public class GunController : MonoBehaviour
{
    [Header("Settings")]
    public float fireSpeed;
    public GameObject bulletPrefab;
    public Transform bulletStart;
    public GameObject bullets;
    
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
            GameObject bullet = Instantiate(bulletPrefab, bulletStart.position, bulletStart.rotation);
            bullet.transform.SetParent(bullets.transform);
            _canShoot = false;
            _shootTimer += 1 / fireSpeed;
        }
    }

    private bool CanShoot()
    {
        if (!_canShoot && Time.timeScale > 0)
        {
            _shootTimer -= Time.deltaTime;
            if (_shootTimer <= 0)
                _canShoot = true;
        }
        return _canShoot;
    }
}