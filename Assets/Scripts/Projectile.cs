using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    //outlets
    Rigidbody2D rigidbody;

    //transform target;
    Transform target;

    //methods
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        //some dynamic projectile attribute
        float acceleration = GameController.instance.missileSpeed/2f;
        float maxSpeed = GameController.instance.missileSpeed;

        //home in on target
        ChooseNearestTarget();
        if(target!=null)
        {
            //rotate towards taget
            Vector2 directionToTarget = target.position - transform.position;
            float angle = Mathf.Atan2(directionToTarget.y, directionToTarget.x)*Mathf.Rad2Deg;

            rigidbody.MoveRotation(angle);
        }

        //Acceleration forward
        rigidbody.AddForce(transform.right * acceleration);

        //Cap max speed
        rigidbody.velocity = Vector2.ClampMagnitude(rigidbody.velocity, maxSpeed);
    }

    void ChooseNearestTarget()
    {
        float closestDistance = 9999f; //pick a really high number as default
        Asteroid[] asteroids = FindObjectsOfType<Asteroid>();
        for(int i=0; i<asteroids.Length; i++)
        {
            Asteroid asteroid = asteroids[i];

            //asteroid must be to our right
            if (asteroid.transform.position.x > transform.position.x)
            {
                Vector2 directionToTarget = asteroid.transform.position - transform.position;

                if (directionToTarget.sqrMagnitude < closestDistance)
                {
                    //update closest distance for future comparisons
                    closestDistance = directionToTarget.sqrMagnitude;

                    //Store reference to closest target we've seen so far
                    target = asteroid.transform;
                }
            }
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        //only explode on asteroids
        if (other.gameObject.GetComponent<Asteroid>())
        {
            Destroy(other.gameObject);
            Destroy(gameObject);

            //create an explosion and destoy it soon after
            GameObject explosion = Instantiate(GameController.instance.explosionPrefab, transform.position, Quaternion.identity);
            Destroy(explosion, 0.25f);

            GameController.instance.EarnPoints(10);
        }
    }
}
