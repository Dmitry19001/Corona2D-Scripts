using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts 
{
    public class GameControl : MonoBehaviour
    {

        [SerializeField] [Range(0f, 2f)] private float spreadRadius = 1f;
        [SerializeField] [Range(0f, 1f)] private float chanceHealthPerk = 0.075f;
        [SerializeField] [Range(0f, 1f)] private float chanceNeatPerk = 0.1f;

        [SerializeField] private float BaseImmunity = 100;
        [SerializeField] private float BaseImmunityDecrease = 5;
        [SerializeField] private float BaseImmunityIncrease = 2;
        [SerializeField] private float NearSickModifier = 1f;
        [SerializeField] private float InsideSicknessIncreaser = 1.5f;

        [SerializeField] private float StartMoney = 100000;
        [SerializeField] private float BuildingRent = 550f;
        [SerializeField] [Range(0,100)] [Tooltip("Percentage of Building's rent to income while the building is open")] private float BuildingPassiveIncome = 75;
        [SerializeField] private float profitToCitizenMultiplier = 50f;
        [SerializeField] private float citizenTax = 25f;

        [SerializeField] private float maskPrice = 120000;
        [SerializeField] private float desiPrice = 150000;
        [SerializeField] private float pillsPrice = 170000;
        [SerializeField] private float researchPrice = 200000;
        [SerializeField] private float vaccinePrice = 250000;

        [SerializeField] [Range(0f, 100f)] private float maskEffect = 5;
        [SerializeField] [Range(0f, 100f)] private float desiEffect = 7;
        [SerializeField] [Range(0f, 100f)] private float pillsEffect = 10;
        [SerializeField] [Range(0f, 100f)] private float researchEffect = 10;
        [SerializeField] [Range(0f, 100f)] private float vaccineEffect = 40;


        [SerializeField] private bool DebugMode = false;


        void Start()
        {
            StartCoroutine(CorrectRules());

            GameObject Buildings = GameObject.Find("Buildings");
            GetBuildings(Buildings);
            //GetBuildingsProfitList(Buildings);
            //SetBuildingsInfo();
            Globals.budget = StartMoney;
        }

        IEnumerator CorrectRules()
        {
            for(; ; )
            {
           
                Globals.profitMultiplier = profitToCitizenMultiplier;
                Globals.buildingRent = BuildingRent;
                Globals.citizenTax = citizenTax;

                Globals.chanceHealthPerk = chanceHealthPerk;
                Globals.chanceNeatPerk = chanceNeatPerk;
                
                Globals.debugMode = DebugMode;

                Globals.spreadRadius = spreadRadius;
                Globals.baseImmunity = BaseImmunity;
                Globals.BaseImmunityIncrease = BaseImmunityIncrease;
                Globals.BaseImmunityDecrease = BaseImmunityDecrease;
                Globals.NearSickModifier = NearSickModifier;
                Globals.InsideSicknessIncreaser = InsideSicknessIncreaser;

                Globals.maskEffect = maskEffect;
                Globals.desiEffect = desiEffect;
                Globals.pillsEffect = pillsEffect;
                Globals.researchEffect = researchEffect;
                Globals.vaccineEffect = vaccineEffect;

                Globals.maskPrice = maskPrice;
                Globals.desiPrice = desiPrice;
                Globals.pillsPrice = pillsPrice;
                Globals.resPrice = researchPrice;
                Globals.vaccinePrice = vaccinePrice;

                CountCash();

                CountBounus();

                yield return new WaitForSeconds(1f);
            }
        }

        private void CountBounus()
        {
            Globals.totalHealthEffect = 0;
            Globals.totalHealEffect = 0;

            if (Globals.masksObtained)
                Globals.totalHealthEffect += Globals.maskEffect;

            if (Globals.desiObtained)
                Globals.totalHealthEffect += Globals.desiEffect;

            if (Globals.pillsObtained)
                Globals.totalHealEffect += Globals.pillsEffect;

            if (Globals.resObtained)
            {
                Globals.totalHealEffect += Globals.researchEffect;
                Globals.totalHealthEffect += Globals.researchEffect;
            }

            if (Globals.vacObtained)
            {
                Globals.totalHealEffect += Globals.vaccineEffect;
                Globals.totalHealthEffect += Globals.vaccineEffect;
            }
        }

        private void CountCash()
        {
            Globals.cashModifier = 0;

            //Debug.Log("Cash Counting: InList:" + Globals.Buildings.Count.ToString());

            for (int x = 0; x < Globals.Buildings.Count; x++)
            {
                //Debug.Log("Profit " + Globals.Buildings[x].Profit.ToString());
                if (!Globals.Buildings[x].Closed)
                    Globals.cashModifier += (Globals.Buildings[x].Profit - Globals.buildingRent * (1 - BuildingPassiveIncome/100 ));
                else
                    Globals.cashModifier -= Globals.buildingRent;

                Globals.Buildings[x].CountProfit();
            }


            Globals.cashModifier += Globals.citizenCount * Globals.citizenTax;

            //Debug.Log("Cash Counted: " + Globals.citizenCount.ToString() + " * " + Globals.citizenTax.ToString());

            Globals.budget += Globals.cashModifier;

            //Debug.Log("Cash Counted: Income " + Globals.cashModifier.ToString());
        }


        private void GetBuildings(GameObject Buildings)
        {
            Globals.Buildings.Clear();

            for (int i = 0; i < Buildings.transform.childCount; i++)
            {
                Bldng bd = new Bldng(i,Buildings.transform.GetChild(i).gameObject);

                Globals.Buildings.Add(bd);                
            }
        }

    }
}


