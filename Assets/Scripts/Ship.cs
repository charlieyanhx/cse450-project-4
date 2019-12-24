using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ship : MonoBehaviour
{
    //outlet
    public GameObject projectilePrefab;
    public Image imageHealthBar;
    public Text hullUpgradeText;
    public Text fireSpeedUpgradeText;
    public Text gameOverText;

    //state tracking
    public float firingDelay = 1f;
    public float healthMax = 100f;
    public float health = 100f; 

    void Start()
    {
        StartCoroutine("FiringTimer");
    }

    void Update()
    {
        if (health > 0)
        {
            transform.position = new Vector2(0, Mathf.Sin(GameController.instance.timeElapsed) * 3f);
        }
    }

    void FireProjectile()
    {
        Instantiate(projectilePrefab, transform.position, Quaternion.identity);
    }

    IEnumerator FiringTimer()
    {
        yield return new WaitForSeconds(firingDelay);

        FireProjectile();

        StartCoroutine("FiringTimer");
    }

    void TakeDamage(float damageAmount)
    {
        health -= damageAmount;
        if (health <= 0)
        {
            Die();
        }
        imageHealthBar.fillAmount = health / healthMax;
    }

    void Die()
    {
        StopCoroutine("FiringTimer");
        GetComponent<Rigidbody2D>().gravityScale = 0;
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        gameOverText.text = "Game Over";
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Asteroid>())
        {
            TakeDamage(10f);
        } 
    }

    public void RepairHull()
    {
        int cost = 100;
        if (health < healthMax && GameController.instance.money >= cost)
        {
            GameController.instance.money -= cost;

            health = healthMax;
            imageHealthBar.fillAmount = health / healthMax;
        }
    }

    public void UpgradeHull()
    {
        int cost = Mathf.RoundToInt(healthMax);
        if (GameController.instance.money >= cost)
        {
            GameController.instance.money -= cost;

            health += 100;
            healthMax += 100;
            imageHealthBar.fillAmount = health / healthMax;

            hullUpgradeText.text = "Hull Strength $" + Mathf.RoundToInt(healthMax).ToString();
        }
    }

    public void UpgradeFireSpeed()
    {
        int cost = 100 + Mathf.RoundToInt((1f - firingDelay) * 100f);

        if (GameController.instance.money >= cost)
        {
            GameController.instance.money -= cost;

            firingDelay -= 0.1f;

            int newCost = 100 + Mathf.RoundToInt((1f - firingDelay) * 100f);
            fireSpeedUpgradeText.text = "Fire Speed $" + newCost.ToString(); 
        }
    }
}

