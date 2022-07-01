using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts
{
    public class SickController : MonoBehaviour
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
        //private bool randomRes = false;
        private bool isSick = false;

        private int NearCount = 0;
        //private int Ticks = 0;
       // private int randomInt = 0;

        private int my_id = 0;

        private float healthBonus = 0;
        private float healingBonus = 0;

        private string MyNameIs = "What? WHO?";

        // Start is called before the first frame update
        void Start()
        {

            citizenList = GameObject.Find("CitizenList");
            healthyList = citizenList.transform.GetChild(0).gameObject;
            sickList = citizenList.transform.GetChild(1).gameObject;

            my_id = GetInstanceID();
            MyNameIs = GlobalVars.g_citizenNames[Random.Range(0, GlobalVars.g_citizenNames.Length - 1)] + " " + GlobalVars.g_citizenSurnames[Random.Range(0, GlobalVars.g_citizenSurnames.Length - 1)];
            transform.name = "Citizen" + my_id.ToString();//Setting name for gameObject for identification

            SelectIndicator = transform.GetChild(1).gameObject;

            DebugCircle = transform.GetChild(0).gameObject;
            DebugCircle.SetActive(false);

            GetPerks();
            randomSickness();

            StartCoroutine(CheckSickness());
        }

        private void randomSickness()
        {
            var gotSickness = Randomise(GlobalVars.g_getSickOnSpawnChance_modifier - healthBonus + (GlobalVars.g_zeroPatientExcists ? 0:1));
            if (gotSickness)
            {
                GetSick();
                if (!GlobalVars.g_zeroPatientExcists) 
                    GlobalVars.g_zeroPatientExcists = true;
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

            var health = GlobalVars.g_chanceHealthPerk;
            var healing = GlobalVars.g_chanceNeatPerk;

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
                    healthBonus += 0.15f;
                    healingBonus += 0.1f;
                    break;

                case HPPerk.Weak:
                    healthBonus += -0.20f;
                    healingBonus += -0.05f;
                    break;

                default: break;
            }


            switch (neatPerk) {
                case NeatnessPerk.Normal:
                    break;
                
                case NeatnessPerk.Neat:
                    healthBonus += 0.07f;
                    healingBonus += 0.05f;
                    break;

                case NeatnessPerk.Dirty:
                    healthBonus += -0.10f;
                    healingBonus += -0.05f;
                    break;

                default: break;
            }

            healingBonus += GlobalVars.g_totalHealEffect;
            healthBonus += GlobalVars.g_totalHealthEffect;
        }

        private void FixedUpdate()
        {
            CheckInBuilding();
            CheckNearAround();
            CheckGroup();
            //Ticks++;
            if (GlobalVars.g_debuggingID == my_id && GlobalVars.g_debugMode) //Debbuging variables
            {
                GlobalVars.g_debugLog = string.Format("DebuggingID:{0}" +
                    "\nIsNear:{1}" +
                    "\nNearCount:{2}" +
                    "\nCursorLocation:{3}" +
                    "\nPerks: {4}, {5}" +
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

                //DebugCircle.transform.localScale = new Vector3(GlobalVars.g_spreadRadius * 0.22f, GlobalVars.g_spreadRadius * 0.22f, 1);
                
                CheckBonuses();

                

                if (CompareTag("Sick"))
                {
                    var chance_to_heal = GlobalVars.g_healChance_modifier + healingBonus;
                    var to_heal = Randomise(chance_to_heal);
                    if (to_heal)
                        GetHealthy();

                    GlobalVars.normalBiggesHeal = chance_to_heal;
                }

                //CheckNearAround();
                var sickChance = GlobalVars.g_getSickFromAnother_modifier - healthBonus;

                if (CompareTag("Healthy+b"))
                    sickChance += GlobalVars.g_nearSick_modifier * NearCount * 2;
                else
                    sickChance += GlobalVars.g_nearSick_modifier * NearCount;

                if (sickChance < 0)
                    sickChance = 0.01f;
                else if (sickChance > 1)
                    sickChance = 1f;

                if (isNear && (CompareTag("Healthy") || CompareTag("Healthy+b")))
                {
                    
                    var gotSickness = Randomise(sickChance);

                    if (gotSickness)
                        GetSick();

                }

                
                GlobalVars.normalBiggestSick = sickChance;

                GlobalVars.g_sickCount = sickList.transform.childCount;

                yield return new WaitForSeconds(2f);
            }
        }

        private void CheckInBuilding()
        {
            var inBuilding = false;
            Collider2D[] others = Physics2D.OverlapPointAll(transform.position);

            foreach (var collider in others)
            {
                if (collider.gameObject.name != gameObject.name)
                {
                    if (collider.CompareTag("Building"))
                    {
                        inBuilding = true;
                        break; //no need in continuing searching if is found
                    }
                }
            }

            if (inBuilding && !tag.Contains("+b"))
                tag += "+b";
            else if (!inBuilding && tag.Contains("+b"))
                tag = tag.Substring(0, tag.Length - 2);
        }


        private void CheckNearAround()
        {
            NearCount = 0;

            Collider2D[] others = Physics2D.OverlapCircleAll(transform.position, GlobalVars.g_spreadRadius);
            
            foreach(var collider in others)
            {
                if (collider.gameObject.name != gameObject.name)
                {
                    if (CompareTag("Healthy") && collider.gameObject.CompareTag("Sick"))
                        NearCount++;
                    else if (CompareTag("Healthy+b") && collider.gameObject.CompareTag("Sick+b"))
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

            GlobalVars.g_gotSick.Add(gameObject);
            GlobalVars.g_gotSickNames.Add(MyNameIs);

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
            GlobalVars.g_hovering_Char = true;
            GlobalVars.g_infoChar = string.Format(
                "{0}" +
                "\n\nHas {1} health and can be characterized as {2}" +
                "\n\nCurrently: {3}",
                MyNameIs,
                healthPerk.ToString(),
                neatPerk.ToString(),
                isSick ? "Sick" : "Healthy"
                );
            //transform.GetComponent<SpriteRenderer>().color = Color.blue;
        }

        private void OnMouseExit()
        {
            GlobalVars.g_hovering_Char = false;

            if (tag.Contains("Sick"))
                transform.GetComponent<SpriteRenderer>().color = Color.red;
            else
                transform.GetComponent<SpriteRenderer>().color = Color.green;
        }

        private void OnMouseDrag()
        {
            if (GlobalVars.g_debugMode)
            {
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector3 mousePosOffset = new Vector3(mousePos.x, mousePos.y, mousePos.z * 0 - 4);

                transform.position = mousePosOffset;
            }
        }

        private void OnMouseDown()
        {
            if (GlobalVars.g_debugMode)
            {
                GlobalVars.g_debuggingID = my_id;
                DebugCircle.SetActive(true);

                SelectIndicator.transform.GetComponent<SpriteRenderer>().color = Color.blue;
                Selection.activeObject = transform.gameObject;
            }
        }
    }
}


