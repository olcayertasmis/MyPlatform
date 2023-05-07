using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerFire : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float bulletSpeed = 20f;
    public float bulletLifetime = 5f;

    private float lastShotTime;
    private bool facingRight = true;

    public GameObject bulletSpawner;

    void Update()
    {
        float moveDirection = Input.GetAxisRaw("Horizontal");
        if (moveDirection > 0)
        {
            facingRight = true;
        }
        else if (moveDirection < 0)
        {
            facingRight = false;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            SoundManager.Instance.PlaySfx("Shot");
            // get fire direction based on player facing direction
            Vector2 fireDirection = facingRight ? firePoint.right : -firePoint.right;

            // create new bullet
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation, bulletSpawner.transform);

            // set bullet velocity based on fire direction
            bullet.GetComponent<Rigidbody2D>().velocity = fireDirection * bulletSpeed;

            // set bullet lifetime
            //Destroy(bullet, bulletLifetime);
        }
    }
}