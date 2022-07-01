using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts
{

    public class SickSpreadControl : MonoBehaviour
    {
        enum HPPerk
        { 
            Strong, 
            Normal,
            Weak 
        }

        enum NeatnessPerk
        {
            Neat,
            Normal,
            Dirty
        }

        //[SerializeField] [Range(0f, 1f)] private float nearSickModifier = 0.05f;

        private GameObject citizenList;    
        private GameObject sickList;
        private GameObject healthyList;

        [SerializeField] private HPPerk healthPerk = HPPerk.Normal;
        [SerializeField] private NeatnessPerk neatPerk = NeatnessPerk.Normal;

        private GameObject DebugCircle;
        private GameObject SelectIndicator;

        private bool isNear = false;
        private bool isSick = false;

        private int NearCount = 0;
        private int my_id = 0;
        private int lastVisitIndex = 0;

        private float Immunity = 100;

        private float healthBonus = 0;
        private float healingBonus = 0;

        private string myNameIs = "What? WHO?";

        // Start is called before the first frame update
        void Start()
        {

            citizenList = GameObject.Find("CitizenList");
            healthyList = citizenList.transform.GetChild(0).gameObject;
            sickList = citizenList.transform.GetChild(1).gameObject;

            my_id = GetInstanceID();
            myNameIs = Globals.citizenNames[Random.Range(0, Globals.citizenNames.Length - 1)] + " " + Globals.citizenSurnames[Random.Range(0, Globals.citizenSurnames.Length - 1)];
            transform.name = "Citizen" + my_id.ToString();//Setting name for gameObject for identification

            SelectIndicator = transform.GetChild(1).gameObject;

            DebugCircle = transform.GetChild(0).gameObject;
            DebugCircle.SetActive(false);

            GetPerks();

            Immunity = Globals.baseImmunity + healthBonus;

            randomSickness();

            StartCoroutine(CheckSickness());
        }

        private void randomSickness()
        {
            var gotSickness = Randomise(Globals.getSickOnSpawnChance_modifier - healthBonus + (Globals.zeroPatientExcists ? 0:1));
            if (gotSickness)
            {
                Immunity = 0;

                GetSick();
                if (!Globals.zeroPatientExcists) 
                    Globals.zeroPatientExcists = true;
            }
                
            else
                GetHealthy();
        }

        private void GetPerks() //Simple perks
        {
            //Has two categories
            //////////////////////
            //I category
            ////Strong Health -- healthBonus:15% healingBonus:10%
            ////Weak Health -- healthBonus:-20% healingBonus:-5%
            ////Normal -- no buffs
            //////////////////////
            //II category
            ////Neat -- healthBonus:7% healingBonus:5%
            ////Dirty -- healthBonus:-10% healingBonus:-5%
            ////Normal -- no buffs
            //////////////////////

            //Strong Health chance 7%
            //Weak Health chance 7%
            //Normal Health chance 86%

            //Neat chance 10%
            //Dirty chance 10%
            //Normal chance 80%

            var health = Globals.chanceHealthPerk;
            var healing = Globals.chanceNeatPerk;

            var healtChance = Randomise(health);
            if (healtChance)
                healthPerk = HPPerk.Strong;
            else
            {
                healtChance = Randomise(health);
                if (healtChance)
                    healthPerk = HPPerk.Weak;
                else
                    healthPerk = HPPerk.Normal;
            }


            var neatChance = Randomise(healing);
            if (neatChance)
                neatPerk = NeatnessPerk.Neat;
            else
            {
                neatChance = Randomise(healing);
                if (neatChance)
                    neatPerk = NeatnessPerk.Dirty;
                else
                    neatPerk = NeatnessPerk.Normal;
            }

            CheckBonuses();
        }

        private void CheckBonuses()
        {

            healingBonus = 0;
            healthBonus = 0;

            switch (healthPerk) {
                case HPPerk.Normal: 
                    break;

                case HPPerk.Strong: 
                    healthBonus += 15f;
                    healingBonus += 2.5f;
                    break;

                case HPPerk.Weak:
                    healthBonus += -20f;
                    healingBonus += -5f;
                    break;

                default: break;
            }


            switch (neatPerk) {
                case NeatnessPerk.Normal:
                    break;
                
                case NeatnessPerk.Neat:
                    healthBonus += 7f;
                    healingBonus += 5f;
                    break;

                case NeatnessPerk.Dirty:
                    healthBonus += -10f;
                    healingBonus += -7f;
                    break;

                default: break;
            }

            healingBonus += Globals.totalHealEffect;
            healthBonus += Globals.totalHealthEffect;
        }

        private void FixedUpdate()
        {
            CheckInBuilding();
            CheckNearAround();
            CheckGroup();
            //Ticks++;
            if (Globals.debuggingID == my_id && Globals.debugMode) //Debbuging variables
            {
                Globals.debugLog = string.Format("DebuggingID:{0}" +
                    "\nIsNear:{1}" +
                    "\nNearCount:{2}" +
                    "\nCursorLocation:{3}" +
                    "\nPerks: {4}, {5}" +
                    "\nImmunity: {6}" +
                    "\nSickProgress: {7}",
                    GetInstanceID(),
                    isNear ? "True" : "False",
                    NearCount,
                    Camera.main.ScreenToWorldPoint(Input.mousePosition) + new Vector3(0, 0, 10),
                    healthPerk.ToString(),
                    neatPerk.ToString()
                    ) ;

            }
            else
            {
                if (DebugCircle.activeSelf)
                    DebugCircle.SetActive(false);

                if (SelectIndicator.transform.GetComponent<SpriteRenderer>().color == Color.blue)
                    SelectIndicator.transform.GetComponent<SpriteRenderer>().color = Color.white;
            }
        }

        IEnumerator CheckSickness()
        {
            for (; ; )
            {

                if (Globals.hoveringChar == my_id)
                {
                    Globals.infoChar = string.Format(
                        "{0}" +
                        "\n\nHas {1} health and can be characterized as {2}" +
                        "\n\nCurrently: {3}",
                        myNameIs,
                        healthPerk.ToString(),
                        neatPerk.ToString(),
                        isSick ? "Sick" : "Healthy"
                    );

                    Globals.sicknessProgress = Immunity / (Globals.baseImmunity + healthBonus);
                }
                    

                //DebugCircle.transform.localScale = new Vector3(Globals.spreadRadius * 0.22f, Globals.spreadRadius * 0.22f, 1);
                
                CheckBonuses();

                var gotSickness = Randomise(0.5f);

                if (gotSickness && Globals.debuggingID == my_id)
                    Debug.Log("Got Sick");
                else if (Globals.debuggingID == my_id)
                    Debug.Log("Didn't get Sick");

                CheckNearAround();

                if (tag.Contains("Sick"))
                {
                    if (Immunity < Globals.baseImmunity + healthBonus)
                    {
                        Immunity += (Globals.BaseImmunityIncrease + healingBonus) - (NearCount * Globals.NearSickModifier);
                    }
                    else if (Immunity >= Globals.baseImmunity + healthBonus)
                    {
                        Immunity = Globals.baseImmunity + healthBonus;
                        GetHealthy();
                    }
                }

                if (isNear && tag.Contains("Healthy"))
                {
                    if (isNear)
                    {
                        if (Immunity > 0)
                        {
                            if (CompareTag("Healthy+b"))
                                Immunity += (-Globals.BaseImmunityDecrease + healingBonus) - (NearCount * Globals.NearSickModifier) * Globals.InsideSicknessIncreaser;
                            else
                                Immunity += (-Globals.BaseImmunityDecrease + healingBonus) - (NearCount * Globals.NearSickModifier);
                        }   
                        else
                        {
                            Immunity = 0;
                            GetSick();
                        }
                    }
                    else
                    {
                        if (Immunity < Globals.baseImmunity + healthBonus)
                           Immunity += (Globals.BaseImmunityIncrease + healingBonus);
                        else
                        {
                            Immunity = Globals.baseImmunity + healthBonus;
                        }
                    }
                }

                //Globals.sickCount = sickList.transform.childCount;

                yield return new WaitForSeconds(2f);
            }
        }

        private void CheckInBuilding()
        {
            var inBuilding = false;
            GameObject curBuilding = null;
            Collider2D[] others = Physics2D.OverlapPointAll(transform.position);

            foreach (var collider in others)
            {
                if (collider.gameObject.name != gameObject.name)
                {
                    if (collider.CompareTag("Building"))
                    {
                        curBuilding = collider.gameObject;
                        inBuilding = true;
                        break; //no need in continuing searching if is found
                    }
                }
            }

            if (inBuilding && !tag.Contains("+b"))
            {
                tag += "+b";

                if (curBuilding != null)
                {
                    for (int x = 0; x < Globals.Buildings.Count; x++)
                    {
                        if (Globals.Buildings[x].building == curBuilding)
                        {
                            Globals.Buildings[x].NowInsideCount++;

                            lastVisitIndex = x;
                        }
                    }
                }


            }          
            else if (!inBuilding && tag.Contains("+b"))
            {
                tag = tag.Substring(0, tag.Length - 2);
                Globals.Buildings[lastVisitIndex].NowInsideCount--;
            }

        }


        private void CheckNearAround()
        {
            NearCount = 0;

            Collider2D[] others = Physics2D.OverlapCircleAll(transform.position, Globals.spreadRadius);
            
            foreach(var collider in others)
            {
                if (collider.gameObject.name != gameObject.name)
                {
                    if (!tag.Contains("+b") && collider.gameObject.CompareTag("Sick"))
                        NearCount++;
                    else if (tag.Contains("+b") && collider.gameObject.CompareTag("Sick+b"))
                        NearCount++;
                }
            }

            if (NearCount > 0)
                isNear = true;
            else
                isNear = false;
        }

        private void CheckGroup()
        {
            if ((CompareTag("Sick") || CompareTag("Sick+b")) && transform.parent != sickList.transform)
            {
                transform.parent = sickList.transform;
            }
            else if ((CompareTag("Healthy") || CompareTag("Healthy+b")) && transform.parent != healthyList.transform)
            {
                transform.parent = healthyList.transform;
            }
        }
        
        private void GetSick()
        {
            transform.parent = null;
            transform.GetComponent<SpriteRenderer>().color = Color.red;
            tag = "Sick"; 
            transform.parent = sickList.transform;

            Globals.gotSick.Add(gameObject);
            Globals.gotSickNames.Add(myNameIs);

            isSick = true;
        }

        private void GetHealthy()
        {
            transform.parent = null;
            transform.GetComponent<SpriteRenderer>().color = Color.green;
            tag = "Healthy";
            transform.parent = healthyList.transform;

            isSick = false;
        }

        private bool Randomise(float chance)
        {
            bool result = false;

            int from = 0;
            int fromto = 10000;

            var rawseed = System.DateTime.Now.Second.ToString() + System.DateTime.Now.Minute.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString();

            if (Globals.debuggingID == my_id)
                Debug.Log("Time: " + rawseed);

            //Random.InitState(int.Parse(rawseed)); 

            int rand = Random.Range(from, fromto);
            if (rand < fromto * chance)
                result = true;
            else
                result = false;

            //Debbuging
            //randomInt = rand;
            //randomRes = result;
            //End Debugging

            return result;
        }

        //Mouse iteraction things for Debugging
        private void OnMouseEnter()
        {
            Globals.hoveringChar = my_id;
            Globals.infoChar = string.Format(
                "{0}" +
                "\n\nHas {1} health and can be characterized as {2}" +
                "\n\nCurrently: {3}",
                myNameIs,
                healthPerk.ToString(),
                neatPerk.ToString(),
                isSick ? "Sick" : "Healthy"
                );
            Globals.sicknessProgress = Immunity / (Globals.baseImmunity + healthBonus);
            //transform.GetComponent<SpriteRenderer>().color = Color.blue;
        }

        private void OnMouseExit()
        {
            Globals.hoveringChar = 0;

            if (tag.Contains("Sick"))
                transform.GetComponent<SpriteRenderer>().color = Color.red;
            else
                transform.GetComponent<SpriteRenderer>().color = Color.green;
        }

        private void OnMouseDrag()
        {
            if (Globals.debugMode)
            {
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector3 mousePosOffset = new Vector3(mousePos.x, mousePos.y, mousePos.z * 0 - 4);

                transform.position = mousePosOffset;
            }
        }

        private void OnMouseDown()
        {
            if (Globals.debugMode)
            {
                Globals.debuggingID = my_id;
                DebugCircle.SetActive(true);

                SelectIndicator.transform.GetComponent<SpriteRenderer>().color = Color.blue;
                Selection.activeObject = transform.gameObject;
            }
        }
    }
}


