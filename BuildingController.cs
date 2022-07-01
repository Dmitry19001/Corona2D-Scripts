using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts
{
    public class BuildingController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        int enterd = 0;
        public void OnPointerEnter(PointerEventData eventData)
        {
            enterd++;
            Debug.Log($"MouseEnter {enterd}");
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            Debug.Log("MouseExit");
        }

        //private bool checkInside = true;

        // Start is called before the first frame update
        void Start()
        {
            //Deprecating BuildingController
            //All The functionality comes to SickSpreadControl and GameControl
            //StartCoroutine(checkInside());
        }

        // Update is called once per frame
        void Update()
        {
            //Debug.DrawRay(transform.position - new Vector3(3,2, 0), new Vector3(6, 4, -5));
        }
        
        
    }
}