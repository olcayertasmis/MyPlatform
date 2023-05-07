using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int currentHealth = 3;

    [SerializeField] private GameObject health1, health2, health3;

    private void Awake()
    {
        //throw new NotImplementedException();
    }

    private void Start()
    {
        StartCoroutine(IEAddHealth());
    }

    private void FixedUpdate()
    {
        UpdateHealthSprites();
    }

    private void UpdateHealthSprites()
    {
        switch (currentHealth)
        {
            case 0:
                health1.SetActive(false);
                health2.SetActive(false);
                health3.SetActive(false);
                break;
            case 1:
                health1.SetActive(true);
                health2.SetActive(false);
                health3.SetActive(false);
                break;
            case 2:
                health1.SetActive(true);
                health2.SetActive(true);
                health3.SetActive(false);
                break;
            case 3:
                health1.SetActive(true);
                health2.SetActive(true);
                health3.SetActive(true);
                break;
        }
    }

    IEnumerator IEAddHealth()
    {
        bool control = true;
        while (control)
        {
            if (currentHealth == 0)
            {
                control = false;
            }
            if (currentHealth < 3 && control == true && currentHealth !=0)
            {
                yield return new WaitForSeconds(2f);
                currentHealth++;
            }

            else
            {
                yield return null;
            }
        }
    }

    public void TakeDamage()
    {
        currentHealth--;
    }
}