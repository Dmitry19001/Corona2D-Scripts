using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using testEnvironment;
namespace testEnvironment
{
    public class GunScript : MonoBehaviour
    {
        
        [SerializeField] private float spawnOffsetX = 0;
        [SerializeField] private float spawnOffsetY = 0;
        // Start is called before the first frame update
        void Start()
        {
            StartCoroutine(Shoot());
        }

        // Update is called once per frame
        void Update()
        {

        }

        IEnumerator Shoot()
        {
            for (; ; )
            {
                if (MinigameController.Shooting)
                {
                    GameObject Bullet = Instantiate(MinigameController.bullet_prefab, Vector3.zero, Quaternion.identity, transform);
                    Bullet.transform.localPosition = new Vector2(spawnOffsetX, spawnOffsetY);
                }


                yield return new WaitForSeconds(MinigameController.shooting_interval);
            }
        }
    }
}


