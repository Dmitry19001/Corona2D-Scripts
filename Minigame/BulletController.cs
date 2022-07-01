using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace testEnvironment
{
    public class BulletController : MonoBehaviour
    {
        private Rigidbody2D Bullet;
        private float ySpeed = 1;
        // Start is called before the first frame update
        void Start()
        {
            Bullet = gameObject.GetComponent<Rigidbody2D>();
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void FixedUpdate()
        {
            ySpeed = MinigameController.bullet_speed;

            Vector2 Dir = new Vector2(Bullet.position.x, ySpeed + Bullet.position.y);
            Bullet.MovePosition(Dir);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            print("TriggerEnter");
            if (other.CompareTag("Finish"))
                Destroy(gameObject);

            if (other.CompareTag("Enemy"))
            {
                Destroy(gameObject);
                Destroy(other.gameObject);
            }
        }

    }
}


