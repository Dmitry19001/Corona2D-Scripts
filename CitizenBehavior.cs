using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts;

public class CitizenBehavior : MonoBehaviour
{
    private Rigidbody2D rb;
    //[SerializeField]
    private float speedMin = 0.8f;
    //[SerializeField]
    private float speedMax = 1.5f;
    //[SerializeField]
    //private float ChangeSpeedInTime = 1.0f;
    //private bool walkingSteady = false;

    private GameObject Buildings;
    private int BuildingCount;
    private GameObject Building;
    private int buildingNum;

    [SerializeField]
    private int maxBuildingVisits = 5;
    private int BuildingsVisited = 0;
    [SerializeField]
    private int maxSpotVisits = 2;
    private int spotsVisited = 0;

    private GameObject CitizenSpawners;
   // private GameObject CitizenList;
    [SerializeField] private bool preSpawned = false;
    [SerializeField] private bool builgingMaskAtStart = false;

    private GameObject Target;

    [SerializeField] private float speed = 1.0f;

    //private int siblingCount = 0;
    [SerializeField] private bool entering = true;
    private bool exitingcity = false;


    //Wait for next target
    private bool waiting = false;

    //Get BuildingBehavior for current building
    private BuildingBehavior buildingBehavior;

    //Direction change counter, to stop if citizien is looping
    private int directionChange;


    //Debug tool
    private int debugCounter = 0;

    void Awake()
    {
        Buildings = GameObject.Find("Buildings");
        CitizenSpawners = GameObject.Find("CitizenSpawners");
        rb = GetComponent<Rigidbody2D>();
        //CitizenList = GameObject.Find("CitizenList");
    }
    // Start is called before the first frame update
    void Start()
    {   //Pre-spawned citizens
        if (preSpawned==true) { 
            Invoke("StartSettings", 0.5f); //gameObject.transform.parent = CitizenList.transform.GetChild(0);
            if (builgingMaskAtStart)
            {
                gameObject.transform.GetChild(2).gameObject.SetActive(true);
            }
        }
        else
        {StartSettings(); }

        //DEBUG
        //StartCoroutine(Debuginfo());
    }

    // Update is called once per frame
    void Update()
    {
    }
    IEnumerator Debuginfo()
    {
        for (; ; )
        {
            //Debug.Log("Park INfo:" + Globals.Buildings[12].building);
            //Debug.Log("Park: closed:" + Globals.Buildings[12].Closed + " ,Limit:" + Globals.Buildings[12].CitizenLimit + " ,inside:" + Globals.Buildings[12].NowInsideCount);
                
            yield return new WaitForSeconds(1.0f);
         
        }
    }
    void StartSettings()
    {
        ChooseBuilding();
        speed = Random.Range(speedMin, speedMax); //0.7 ... 1.2 toimi
        maxBuildingVisits = Random.Range(1, 4);
        BuildingsVisited = 0;
        //after spawn, direction towards center
        rb.velocity = new Vector2(0 - gameObject.transform.position.x, 0 - gameObject.transform.position.y).normalized;
        if (!builgingMaskAtStart)
        {
            gameObject.transform.GetChild(2).gameObject.SetActive(false); //unEnable see-through mask
        }

    }

    private void ChooseBuilding()
    {
        int loopCount = 0; //to avoid too long loops
        if (BuildingsVisited < maxBuildingVisits)
        {
            //Debug.Log("at ChooseBuilding");
            BuildingCount = Buildings.transform.childCount;
            int num = Random.Range(0, BuildingCount);
            //Target: Random building's Entrance-object
            Target = GameObject.Find(Buildings.transform.GetChild(num).name);
            //Choose again if Target is the same as previous Or if Building is closed
            while (((Target == Building) || (Globals.Buildings[num].Closed == true)) && loopCount < 30)
            {
                loopCount += 1;
                num = Random.Range(0, BuildingCount);
                Target = GameObject.Find(Buildings.transform.GetChild(num).name);
            }

            if (loopCount >= 30) { ChooseExit(); }// Debug.Log("Too many loops in ChooseBuilding"); }
            else
            {
                Building = Target; //Entrance will become target next, so saving the info of building
                buildingNum = num; //Save index
                Target = Target.gameObject.transform.GetChild(0).gameObject;

                //Target is choosen: Can continue with functions (later down a coroutine waits for this)
                waiting = false;

                //Debug.Log("id " + num);
                //Debug.Log(Globals.Buildings[num].Closed);
                //Debug.Log(Globals.Buildings[num].CitizenLimit);
            }
        }
        else { ChooseExit(); }

    }

    private void OnTriggerEnter2D(Collider2D obj)
    {
        //Cross section
        if (obj.tag == "CrossSection")
        {
            float lateturn = (0.5f / speed);
            Invoke("TurningMoving", lateturn);
        }

        //Spot - Find new //Target should be now 'Building'-gameobject
        else if (entering == false && exitingcity == false)
        {
            //Debug.Log("Spot - Find new");

            if (obj.tag == "BuildingInsideSpot")                            //Choose spot
            {
                Building = obj.transform.parent.gameObject;
                buildingNum = Building.transform.GetSiblingIndex();
                ChooseSpot(obj.gameObject);
                float standidle = (Random.Range(1.0f, 4.5f));               //Idle time
                Invoke("MoveToSpot", standidle);                            //Move to spot
            }

            else if (obj.tag == "BuildingEntrance" && spotsVisited > maxSpotVisits)//Exit building
            {
                //Debug.Log("ExitBuilding 1");
                ExitBuilding();
            }
            //buildingBehavior.insideCount++;
            //Debug.Log("BuildingBehavior Inside Count: " + buildingBehavior.insideCount);
        }
        //Entrance - entering
        else if (obj.gameObject == Target.gameObject && entering == true)
        {
            //Debug.Log("Entering");
            entering = false;
            int spotCount = Building.transform.childCount;
            Target = Target.transform.parent.gameObject; //New target=Building: to aim at center of inside
            float lateturn = (0.1f / speed);


            //Debug
            if (Target.name == "BuildingPark")
            {
                Debug.Log("building Num" + buildingNum);
                Debug.Log("CLOSED?" + "inside" + Globals.Buildings[buildingNum].NowInsideCount + " , limit:" + Globals.Buildings[buildingNum].CitizenLimit);

            }

            //Check building restricitions
            if (Globals.Buildings[buildingNum].Closed == true || (Globals.Buildings[buildingNum].NowInsideCount >= Globals.Buildings[buildingNum].CitizenLimit && Globals.Buildings[buildingNum].CitizenLimit != 0) )
            {
                if (Target.name != "BuildingPark")
                {
                    rb.velocity *= 0;
                }
                else { if (rb.velocity.y < 0) { rb.velocity *= 1.2f; }
                    }
                //rb.velocity *= -1;

                Invoke("ExitBuilding", lateturn*2);
            }

            else if (Target.name != "BuildingPark")
            {
                Invoke("TurningMoving", lateturn); //Turns at center of Building
            }
            else { rb.velocity = new Vector2(0.0f, -speed); } //Turns downwards (at park entrance)

            gameObject.transform.GetChild(2).gameObject.SetActive(true); //Enable see-through mask while inside

            //Set amount of spots to visit
            maxSpotVisits = Random.Range(5, 10);
            //Zero directioncheck
            directionChange = 0;
        }
        //OuterLimit - exit city
        if (obj.CompareTag("OuterLimit"))// && exitingcity == true)
        {
            //Debug.Log("Exited City");
            //Destroy(gameObject);
            rb.velocity = new Vector2(0, 0);
            gameObject.GetComponent<SickSpreadControl>().enabled = false;
            exitingcity = false;
            Invoke("BackToCity", Random.Range(8,12));
        }
    }

    //Choose spot (inside building)
    private void ChooseSpot(GameObject obj)
    {
        rb.velocity = new Vector2(0, 0);                //movement stopped
        spotsVisited += 1;                              //Count how many spots visited in current building
        //Debug.Log("SpotsVisited: " + spotsVisited+"from max"+ maxSpotVisits);
        int spotCount = Building.transform.childCount;  //New target from range of building.childCount
        int num = Random.Range(1, spotCount-1);         //-1 means leave out last child: building floor

        debugCounter += 1;//DEBUG
        //Debug.Log(debugCounter+" ChooseSpot at" + obj.name);

        if (spotsVisited > maxSpotVisits) //If enough spots visited: Exit
        {
            num = 0;
        }
        else
        { //Choose random spot /Choose again if new spot is same as current
            while (Building.transform.GetChild(num).gameObject == obj.gameObject)
            {
                num = Random.Range(1, spotCount);
            }
        }
        
        Target = Building.transform.GetChild(num).gameObject;

        //Debug.Log(debugCounter+" ChooseSpot New Spot:" + Target.name);
    }

    //Moves toward a spot (inside building)
    private void MoveToSpot()
    {
        Vector2 directionVector = (Target.transform.position - gameObject.transform.position).normalized;
        rb.velocity = directionVector;
    }
    //Exit building
    private void ExitBuilding()
    {
        //Debug.Log("at ExitBuilding()");
        spotsVisited = 0; //Reset visited spots counter
        //Vector2 directionVector = (Target.transform.position - gameObject.transform.position).normalized;
        //rb.velocity = directionVector; //Direction straight at exit

        BuildingsVisited += 1; //Count buildingvisists
        gameObject.transform.GetChild(2).gameObject.SetActive(false); //unEnable see-through mask
        NextTarget();
    }
    //Choose Next Target
    private void NextTarget()
    {
        //Debug.Log("at NextTarget()");
        waiting = true;
        if (BuildingsVisited < maxBuildingVisits)
        {
            ChooseBuilding();
            //Debug.Log("at NextTarget() -> ChooseBuilding() .....(BuildingsVisited:" + BuildingsVisited + "/"+ maxBuildingVisits + ")");
        }
        else { ChooseExit(); }// Debug.Log("at NextTarget() -> ChooseExit()"); }
        Invoke("walkFurther", 0.2f + Random.Range(0.3f, 0.3f) / speed); //0.2 + 0.3 /speed toimi
    }
    private void walkFurther()
    {
        StartCoroutine(targetWaiter()); //wait-method for Target selecting before setting direction
    }
    IEnumerator targetWaiter()
    {
        yield return new WaitUntil(() => waiting == false);

        entering = true; //Ready to enter buildings again
        //Debug.Log("at targetWaiter -> Newa Target:" + Target);
        //Move on x axis (coming out of building, have to turn to x axis)
        if (gameObject.transform.position.x < Target.transform.position.x)
        { rb.velocity = new Vector2(speed, 0.0f); }
        else
        { rb.velocity = new Vector2(-speed, 0.0f); }
    }

    //Exit city
    private void ChooseExit()
    {
        //Debug.Log("at ChooseExit()");
        exitingcity = true;
        //a random CitizenSpawner as an exit target
        int ExitCount = CitizenSpawners.transform.childCount;
        int num = Random.Range(0, ExitCount);
        Target = GameObject.Find(CitizenSpawners.transform.GetChild(num).name);

        //Target is choosen: Can continue with functions
        waiting = false;
    }
    //Cross sections
    private void TurningMoving()
    {
        {
            //direction
            float cx = transform.position.x;
            float tx = Target.transform.position.x;
            float xdifference = (cx - tx);
            float cy = transform.position.y;
            float ty = Target.transform.position.y;
            float ydifference = (cy - ty);

            //Old velocity check, to avoid looping movement
            Vector2 oldVelocity = rb.velocity;

            //velocity
            //Absolute value //itseisarvo -comparison
            if (Mathf.Abs(xdifference) > Mathf.Abs(ydifference))
            {
                //Move on x axis
                if (xdifference < 0) { 
                    rb.velocity = new Vector2(speed, 0.0f);
                    if (oldVelocity.x < 0){ directionChange += 1; }
                }
                else { rb.velocity = new Vector2(-speed, 0.0f);
                    if (oldVelocity.x > 0) { directionChange += 1; }
                }
            }
            else
            {
                //Move on y axis
                if (ydifference < 0) { rb.velocity = new Vector2(0.0f, speed);
                    if (oldVelocity.y < 0) { directionChange += 1; }
                }
                else { rb.velocity = new Vector2(0.0f, -speed);
                    if (oldVelocity.y > 0) { directionChange += 1; }
                }
            }

            //Check if citizien is looping
            if (directionChange > 2) { ChooseExit(); entering = true; }
        }
    }
    private void BackToCity()
    {
        //Check how many buildings is open to decide if to enter city again
        int buildingCount = Buildings.transform.childCount;
        int openBuildings = buildingCount;
        for (int i= 0; i < buildingCount ; i++)
        {
            if (Globals.Buildings[i].Closed) { openBuildings--; }
        }
        //Random chance to go to city
        if (Random.Range(1,buildingCount) <= openBuildings)
        {
            gameObject.GetComponent<SickSpreadControl>().enabled = false;
            //Debug.Log("BackToCity()");
            exitingcity = false;
            maxBuildingVisits = Random.Range(1, 4);
            BuildingsVisited = 0;
            spotsVisited = 0;
            ChooseBuilding();
            if (Mathf.Abs(gameObject.transform.position.x) > Mathf.Abs(gameObject.transform.position.y))
            {
                if (gameObject.transform.position.x < 0) { rb.velocity = new Vector2(speed, 0.0f); }
                else { rb.velocity = new Vector2(-speed, 0.0f); }
            }
            else
            {
                if (gameObject.transform.position.y < 0) { rb.velocity = new Vector2(0.0f, speed); }
                else { rb.velocity = new Vector2(0.0f, -speed); }
            }

            Vector3 temppos = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, 0);
            gameObject.transform.position = temppos;

            gameObject.transform.GetChild(2).gameObject.SetActive(false); //unEnable see-through mask
            directionChange = 0; // zero direction change
        }
        else { Invoke("BackToCity", 10); }

    }
    //For Debug
    void OnMouseOver()
    {
        ;
        //Debug.Log("Citizen Info: Target: "+Target.name+ "Target-Parent:"+Target.gameObject.transform.parent+ "entering:"+ entering + "exit city: "+exitingcity);
    }
}
