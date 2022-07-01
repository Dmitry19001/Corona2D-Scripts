using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    class Globals
    {
        //Info
        public static string infoChar = "";
        public static int hoveringChar = 0;
        public static float sicknessProgress = 0f;

        //Player
        public static string playerName = "";
        public static float playerScore = 0;

        //Level Option
        public static int levelOption = 0;

        //Modifiers
        public static float getSickOnSpawnChance_modifier = 0.01f; // 1.0 = 100%
        public static float cashModifier = 0;
        public static float BaseImmunityDecrease = 0;
        public static float BaseImmunityIncrease = 0;
        public static float NearSickModifier = 0;
        public static float InsideSicknessIncreaser = 0;

        //Random Chances
        public static float chanceHealthPerk = 0.075f;
        public static float chanceNeatPerk = 0.1f;
        //public static float chanceDeath = 0.045f; // DEATH DISABLED
        
        //Others
        public static float spreadRadius = 0.75f;

        //UI Only
        public static int sickCount = 0; //Used for sick count display for user
        public static int sickCountModifier = 0; //Used for sick modifier
        public static List<Notification> Notifications = new List<Notification>();
        public static List<GameObject> gotSick = new List<GameObject>();
        public static List<string> gotSickNames = new List<string>();
        public static bool uiGotFocus = false;
        public static bool buildingSwitched = false;

        //Prices
        public static float maskPrice = 50000;
        public static float desiPrice = 100000;
        public static float pillsPrice = 150000;
        public static float resPrice = 200000;
        public static float vaccinePrice = 500000;

        //Debbuging
        public static bool debugMode = false;
        public static int debuggingID = 0;
        public static string debugLog = "";
        public static string rndSeed = ""; 

        //GameLogic
        public static List<Bldng> Buildings = new List<Bldng>();

        public static bool zeroPatientExcists = false;

        public static bool masksObtained = false;
        public static bool desiObtained = false;
        public static bool pillsObtained = false;
        public static bool resObtained = false;
        public static bool vacObtained = false;

        public static float baseImmunity = 100;
        public static float profitMultiplier = 50;
        public static float buildingRent = 100;
        public static float maskEffect = 0.10f;
        public static float desiEffect = 0.10f;
        public static float pillsEffect = 0.10f;
        public static float researchEffect = 0.10f;
        public static float vaccineEffect = 0.7f;

        public static float totalHealthEffect = 0;
        public static float totalHealEffect = 0;

        public static float citizenTax = 25;
        public static int citizenCount = 0;
        public static int chosenBuildingID = 0;
        public static float budget = 0;

        public static GameObject chosenBuilding = null;


        //Names
        public static readonly string[] citizenNames = {
            "Anssi",
            "Dmitry",
            "Riku",
            "Simo",
            "Arttu",
            "John",
            "Nick",
            "Frank",
            "Patrick",
            "Phil",
            "Mike",
            "Anna",
            "Katherine",
            "Bill",
            "Bob",
            "Jessica",
            "Ville",
            "Scarlett",
            "Robert",
            "Jack",
            "Karl",
            "Vladimir",
            "Adolf",
            "Arnold",
            "Gordon",
            "Gabe",
            "Elizabeth",
            "Sean",
            "Elon",
            "Michael",
            "Peter",
            "Matt",
            "Lily",
            "David",
            "Emilia",
            "Chris",
            "Steven"
        };
        public static readonly string[] citizenSurnames = {
            "Johnsos",
            "Tyson",
            "Pattinson",
            "Downey",
            "Nicholson",
            "Marx",
            "Schwarznegger",
            "Brown",
            "Black",
            "White",
            "Ruffalo",
            "Ford",
            "Cavill",
            "Parker",
            "Simpson",
            "Gates",
            "Collins",
            "Smith",
            "Freeman",
            "Newell",
            "Evans",
            "Johansson",
            "Connery",
            "Musk",
            "Depp",
            "Sanchez",
            "Jackson",
            "Phillips",
            "Newman",
            "Oldman",
            "Tennant",
            "Fresco",
            "Smith",
            "Stone",
            "Wood",
            "Carter"
        };
    }

    //Classes

    class Bldng {

        public Bldng() {
            ID = 0;
            building = null;
            Closed = false;
            CitizenLimit = 0;
            NowInsideCount = 0;
            Profit = 0;
        }

        public Bldng(int id, GameObject buildingameobject, bool closed = false, int citizen_lim = 0, /*int was_inside_count = 0,*/ int now_inside_count = 0, float profit = 0)
        {
            ID = id;
            building = buildingameobject;
            Closed = closed;
            CitizenLimit = citizen_lim;
            //WasInsideCount = was_inside_count;
            NowInsideCount = now_inside_count;
            Profit = profit;
        }

        public int ID { get; }
        public int CitizenLimit { get; set; }
        public float Profit { get; set; }
        public float NowInsideCount { get; set; }

        public GameObject building { get; }

        public bool Closed { get; set; }

        public override string ToString()
        {
            return ID.ToString();
        }

        public void CountProfit()
        {
            Profit = 0;
            Profit = Globals.profitMultiplier * NowInsideCount;
        }
    }

    class Ntfctn {
        public Ntfctn()
        {
            ID = 0;
            SecondsLeft = 0;
            NotificationObject = null;
            NotificationTimerText = null;
            TargetObject = null;
        }

        public Ntfctn(int id, float seconds, GameObject createdNotification, GameObject timerText, GameObject target)
        {
            ID = id;
            SecondsLeft = seconds;
            NotificationObject = createdNotification;
            NotificationTimerText = timerText;
            TargetObject = target;
        }

        public int ID { get; }
        public float SecondsLeft { get; set; }
        public GameObject NotificationObject { get; }
        public GameObject NotificationTimerText { get;  }
        public GameObject TargetObject { get; }
    }


}
