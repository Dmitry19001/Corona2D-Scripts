using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts 
{
    public class GameContoller : MonoBehaviour
    {
        [SerializeField] [Range(0f, 1f)] private float getSickOnSpawnChance_modifier = 0.01f; // 1.0 = 100%
        [SerializeField] [Range(0f, 1f)] private float getSickFromAnother_modifier = 0.25f; // 1.0 = 100%
        [SerializeField] [Range(0f, 1f)] private float healChance_modifier = 0.01f; // 1.0 = 100%
        [SerializeField] [Range(0f, 1f)] private float nearSick_modifier = 0.05f; // 1.0 = 100%
        [SerializeField] [Range(0f, 2f)] private float spreadRadius = 0.75f;
        [SerializeField] [Range(0f, 1f)] private float chanceHealthPerk = 0.075f;
        [SerializeField] [Range(0f, 1f)] private float chanceNeatPerk = 0.1f;
        [SerializeField] private float buildingRent = 100f;
        [SerializeField] private float profitToCitizenMultiplier = 50f;
        [SerializeField] private float citizenTax = 25f;
        [SerializeField] private float startMoney = 100000;
        [SerializeField] [Range(0f, 1f)] private float maskEffect = 0.10f;
        [SerializeField] [Range(0f, 1f)] private float desiEffect = 0.10f;
        [SerializeField] [Range(0f, 1f)] private float pillsEffect = 0.10f;
        [SerializeField] [Range(0f, 1f)] private float researchEffect = 0.10f;
        [SerializeField] [Range(0f, 1f)] private float vaccineEffect = 0.7f;
        [SerializeField] private float maskPrice = 50000;
        [SerializeField] private float desiPrice = 100000;
        [SerializeField] private float pillsPrice = 150000;
        [SerializeField] private float researchPrice = 200000;
        [SerializeField] private float vaccinePrice = 500000;
        //[SerializeField] [Range(0f, 1f)] private float maskNearEffect = 0.10f;
        //[SerializeField] private int buildingsOpen = 1;
        //[SerializeField] private GameObject buildingInfo;

        [SerializeField] private bool DebugMode = false;


        void Start()
        {
            StartCoroutine(CorrectRules());
            //Buildings
            //buildingsOpen = GameObject.Find("Buildings").transform.childCount;

            GameObject Buildings = GameObject.Find("Buildings");
            GetBuildings(Buildings);
            //GetBuildingsProfitList(Buildings);
            //SetBuildingsInfo();
            GlobalVars.g_budget = startMoney;
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void FixedUpdate()
        {

        }

        IEnumerator CorrectRules()
        {
            for(; ; )
            {
                GlobalVars.g_getSickOnSpawnChance_modifier = getSickOnSpawnChance_modifier;
                GlobalVars.g_getSickFromAnother_modifier = getSickFromAnother_modifier;
                GlobalVars.g_healChance_modifier = healChance_modifier;
                GlobalVars.g_nearSick_modifier = nearSick_modifier;
                GlobalVars.g_spreadRadius = spreadRadius;
                GlobalVars.g_profitMultiplier = profitToCitizenMultiplier;
                GlobalVars.g_buildingRent = buildingRent * GlobalVars.g_citizenCount;
                if (GlobalVars.g_citizenCount >= 85)
                {
                    GlobalVars.g_citizenTax = citizenTax;
                }
                else
                {
                    GlobalVars.g_citizenTax = citizenTax * 1.5f;
                }
                GlobalVars.g_chanceHealthPerk = chanceHealthPerk;
                GlobalVars.g_chanceNeatPerk = chanceNeatPerk;
                GlobalVars.g_debugMode = DebugMode;
                GlobalVars.g_maskEffect = maskEffect;
                GlobalVars.g_desiEffect = desiEffect;
                GlobalVars.g_pillsEffect = pillsEffect;
                GlobalVars.g_researchEffect = researchEffect;
                GlobalVars.g_vaccineEffect = vaccineEffect;
                GlobalVars.g_maskPrice = maskPrice;
                GlobalVars.g_desiPrice = desiPrice;
                GlobalVars.g_pillsPrice = pillsPrice;
                GlobalVars.g_resPrice = researchPrice;
                GlobalVars.g_vaccinePrice = vaccinePrice;

                CountCash();

                CountBounus();

                yield return new WaitForSeconds(1f);
            }
        }

        private void CountBounus()
        {
            GlobalVars.g_totalHealthEffect = 0;
            GlobalVars.g_totalHealEffect = 0;

            if (GlobalVars.g_masksObtained)
                GlobalVars.g_totalHealthEffect += GlobalVars.g_maskEffect;

            if (GlobalVars.g_desiObtained)
                GlobalVars.g_totalHealthEffect += GlobalVars.g_desiEffect;

            if (GlobalVars.g_pillsObtained)
                GlobalVars.g_totalHealEffect += GlobalVars.g_pillsEffect;

            if (GlobalVars.g_resObtained)
            {
                GlobalVars.g_totalHealEffect += GlobalVars.g_researchEffect;
                GlobalVars.g_totalHealthEffect += GlobalVars.g_researchEffect;
            }

            if (GlobalVars.g_vacObtained)
            {
                GlobalVars.g_totalHealEffect += GlobalVars.g_vaccineEffect;
                GlobalVars.g_totalHealthEffect += GlobalVars.g_vaccineEffect;
            }
        }

        private void CountCash()
        {
            GlobalVars.g_cashModifier = 0;

            //Debug.Log("Cash Counting: InList:" + GlobalVars.g_Buildings.Count.ToString());

            for (int x = 0; x < GlobalVars.g_Buildings.Count; x++)
            {
                //Debug.Log("Profit " + GlobalVars.g_Buildings[x].Profit.ToString());
                if (!GlobalVars.g_Buildings[x].Closed)
                    GlobalVars.g_cashModifier += (GlobalVars.g_Buildings[x].Profit - GlobalVars.g_buildingRent);
                else
                    GlobalVars.g_cashModifier -= GlobalVars.g_buildingRent;

                GlobalVars.g_Buildings[x].WasInsideCount = 0;
                GlobalVars.g_Buildings[x].CountProfit();
            }


            GlobalVars.g_cashModifier += GlobalVars.g_citizenCount * GlobalVars.g_citizenTax;

            //Debug.Log("Cash Counted: " + GlobalVars.g_citizenCount.ToString() + " * " + GlobalVars.g_citizenTax.ToString());

            GlobalVars.g_budget += GlobalVars.g_cashModifier;

            //Debug.Log("Cash Counted: Income " + GlobalVars.g_cashModifier.ToString());
        }


        private void GetBuildings(GameObject Buildings)
        {
            GlobalVars.g_Buildings.Clear();

            for (int i = 0; i < Buildings.transform.childCount; i++)
            {
                Building bd = new Building(i,Buildings.transform.GetChild(i).gameObject);

                GlobalVars.g_Buildings.Add(bd);                
            }
        }


        //private void SetBuildingsInfo()
        //{
        //    GameObject buildings = GameObject.Find("Buildings");
        //    for (int i=0; i < buildingsOpen; i++)
        //    {
        //        GameObject building = buildings.transform.GetChild(i).gameObject;

        //        var newbuildingInfo = Instantiate(buildingInfo, new Vector2(0, 0), Quaternion.identity);
        //        newbuildingInfo.transform.SetParent(building.transform);
        //        GameObject newbutton = newbuildingInfo.transform.GetChild(0).gameObject;
        //        newbutton.transform.position = new Vector2(-20,20);
        //        ////GameObject buildinginfo = building.transform.GetChild(building.transform.childCount - 1).gameObject;
        //        //GameObject button = buildinginfo.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject;
        //        //button.transform.position = building.transform.position;
        //        //Debug.Log("building: " + building.name);
        //        Debug.Log("newbuildingInfo: " + newbuildingInfo.name);
        //    }
        //}

    }
}


