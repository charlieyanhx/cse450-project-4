using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    //outlets
    Rigidbody2D rigidbody;

    //methods
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        //always move left
        rigidbody.velocity = Vector2.left * Random.Range(0.5f,3f);
    }

    void OnBecameInvisible()
    {
        //destroy when offscreen
        Destroy(gameObject);
    }
}
