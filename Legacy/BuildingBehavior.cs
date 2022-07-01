using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class BuildingBehavior : MonoBehaviour
    {
        [SerializeField]private int insideCount = 0;
        [SerializeField]private int wasInside = 0;
        private int myID = 0;

        //private bool checkInside = true;

        // Start is called before the first frame update
        void Start()
        {
            myID = transform.GetSiblingIndex();
            StartCoroutine(checkInside());
        }

        // Update is called once per frame
        void Update()
        {
            //Debug.DrawRay(transform.position - new Vector3(3,2, 0), new Vector3(6, 4, -5));
        }

        private void FixedUpdate()
        {
            
        }

        private void CheckGuests()
        {
            insideCount = 0;

            Collider2D[] others = Physics2D.OverlapBoxAll(transform.position - new Vector3(3, 2), new Vector2(6, 4) , 0);


            foreach (var collider in others)
            {
                if (collider.gameObject.name != gameObject.name)
                {
                    if (collider.tag.Contains("Healthy") || collider.gameObject.tag.Contains("Sick"))
                        insideCount++;
                }

            }

            GlobalVars.g_Buildings[myID].NowInsideCount = insideCount;              

            if (GlobalVars.g_Buildings[myID].WasInsideCount < insideCount)
            {
                GlobalVars.g_Buildings[myID].WasInsideCount = insideCount;
                GlobalVars.g_Buildings[myID].CountProfit();
            }

            wasInside = insideCount;
        }

        IEnumerator checkInside()
        {
            for (; ; )
            {
                CheckGuests();
                yield return new WaitForSeconds(0.5f);
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {

        }
    }
}