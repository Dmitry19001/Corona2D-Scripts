using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace testEnvironment
{
    public class MinigameController : MonoBehaviour
    {
        public static bool Shooting = false;
        public static float shooting_interval = 0.5f;
        public static float bullet_speed = 1;
        public static GameObject bullet_prefab = null;
        

        [SerializeField] private GameObject BulletPrefab;
        [SerializeField] private float MoveSpeed = 2f; 
        /*[SerializeField] */private GameObject EnemyList;
        //[SerializeField] private GameObject MainCam;

        [SerializeField] private Text UIHUD_Text;

        [SerializeField] private bool IsShooting = false;
        
        [SerializeField] private int Health = 100;

        //[SerializeField] private float CameraYOffset = 2f;
        [SerializeField][Range(0f,2f)] private float ShootInterval = 0.5f;
        [SerializeField][Range(0,100)] private float BulletSpeed = 20;


        private Rigidbody2D Charact;

        private GameObject BorderLeft;
        private GameObject BorderRight;
        private GameObject BorderTop;
        private GameObject BorderBottom;

        private float WidthFactor = 0;
        private float HeightFactor = 0;

        // Start is called before the first frame update
        void Start()
        {
            EnemyList = GameObject.Find("EnemyList");

            bullet_prefab = BulletPrefab;

            Charact = transform.gameObject.GetComponent<Rigidbody2D>();
            StartCoroutine(UpdateUI());

            BorderLeft = GameObject.Find("BorderLeft");
            BorderRight = GameObject.Find("BorderRight");
            BorderTop = GameObject.Find("BorderTop");
            BorderBottom = GameObject.Find("BorderBottom");

            WidthFactor = gameObject.GetComponent<RectTransform>().rect.width;
            HeightFactor = gameObject.GetComponent<RectTransform>().rect.width;

            //UIHUD_Text = GameObject.Find("UIHUDText").GetComponent<Text>();
        }

        // Update is called once per frame
        void Update()
        {



        }

        private void FixedUpdate()
        {
            float x = Input.GetAxis("Horizontal");
            float y = Input.GetAxis("Vertical");
            float shoot = Input.GetAxis("Fire1");

            if (shoot == 1 && !IsShooting)
            {
                IsShooting = true;
                Shooting = IsShooting;  
            }
            else if (shoot == 0 && IsShooting)
            {
                IsShooting = false;
                Shooting = IsShooting;
            }

            if (IsShooting)
                shooting_interval = ShootInterval;

            var xpos = x * MoveSpeed + Charact.position.x;
            var ypos = y * MoveSpeed + Charact.position.y;

            Vector2 Dir = new Vector2(Charact.position.x, Charact.position.y);
            if (x != 0 || y != 0)
            {
                if (xpos - WidthFactor / 2 > BorderLeft.transform.position.x && xpos + WidthFactor / 2 < BorderRight.transform.position.x && ypos + HeightFactor / 2 < BorderTop.transform.position.y && ypos - HeightFactor / 2 > BorderBottom.transform.position.y)
                {
                    Dir = new Vector2(x * MoveSpeed + Charact.position.x, y * MoveSpeed + Charact.position.y);
                    Debug.Log("Moving");
                }
            }

                

            //UIHUD_Text.text = $"LeftBorderPos: {BorderLeft.transform.position.x}\nCurPos:{Charact.position.x}\nMovePos:{Dir}";

            //Charact.Move(Dir);
            Charact.MovePosition(Dir);

            //CamFollow();
        }

        //private void CamFollow()
        //{
        //    MainCam.transform.position = new Vector3(0, transform.position.y + CameraYOffset, -10);
        //}

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Enemy"))
            {
                Destroy(other.gameObject);
                Health -= 25;
            }
        }

        IEnumerator UpdateUI()
        {
            for (; ; )
            {
                bullet_speed = BulletSpeed;
                if (Health <= 0)
                {
                    Destroy(transform.gameObject);
                    

//#if UNITY_EDITOR
//                    if (EditorApplication.isPlaying)
//                    {
//                        EditorApplication.isPlaying = false;
//                    }
//#endif
//                }

//                if (EnemyList.transform.childCount == 0)
//                {
//#if UNITY_EDITOR
//                    if (EditorApplication.isPlaying)
//                    {
//                        EditorApplication.isPlaying = false;
//                    }
//#endif
                }

                //UIHUD_Text.text = string.Format("Health {0}\nEnemies left {1}", Health, EnemyList.transform.childCount);

                yield return new WaitForSeconds(0.25f);
            }
        }
    }

}




