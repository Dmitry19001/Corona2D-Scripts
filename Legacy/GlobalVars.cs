using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    class GlobalVars
    {
        //Info
        public static string g_infoChar = "";
        public static bool g_hovering_Char = false;

        //Player
        public static string playerName = "";
        public static float playerScore = 0;

        //Level Option
        public static int levelOption = 0;

        //Modifiers
        public static float g_getSickOnSpawnChance_modifier = 0.01f; // 1.0 = 100%
        public static float g_getSickFromAnother_modifier = 0.25f; // 1.0 = 100%
        public static float g_healChance_modifier = 0.01f; // 1.0 = 100%
        public static float g_nearSick_modifier = 0.05f;
        public static float g_cashModifier = 0;

        //Random Chances
        public static float g_highestGetSickChance = 0.0f;
        public static float g_chanceHealthPerk = 0.075f;
        public static float g_chanceNeatPerk = 0.1f;
        public static float g_chanceDeath = 0.045f;
        
        //Others
        public static float g_spreadRadius = 0.75f;

        //UI Only
        public static int g_sickCount = 0; //Used for sick count display for user
        public static int g_sickCountModifier = 0; //Used for sick modifier
        public static List<Notification> g_Notifications = new List<Notification>();
        public static List<GameObject> g_gotSick = new List<GameObject>();
        public static List<string> g_gotSickNames = new List<string>();

        //Prices
        public static float g_maskPrice = 50000;
        public static float g_desiPrice = 100000;
        public static float g_pillsPrice = 150000;
        public static float g_resPrice = 200000;
        public static float g_vaccinePrice = 500000;

        //Debbuging
        public static bool g_debugMode = false;
        public static int g_debuggingID = 0;
        public static string g_debugLog = "";
        public static float normalBiggesHeal = 0;
        public static float normalBiggestSick = 0;

        //GameLogic
        public static List<Building> g_Buildings = new List<Building>();

        public static bool g_zeroPatientExcists = false;

        public static bool g_masksObtained = false;
        public static bool g_desiObtained = false;
        public static bool g_pillsObtained = false;
        public static bool g_resObtained = false;
        public static bool g_vacObtained = false;
        
        public static float g_profitMultiplier = 50;
        public static float g_buildingRent = 100;
        public static float g_maskEffect = 0.10f;
        public static float g_desiEffect = 0.10f;
        public static float g_pillsEffect = 0.10f;
        public static float g_researchEffect = 0.10f;
        public static float g_vaccineEffect = 0.7f;

        public static float g_totalHealthEffect = 0;
        public static float g_totalHealEffect = 0;

        public static float g_citizenTax = 25;
        public static int g_citizenCount = 0;
        public static int g_chosenBuildingID = 0;
        public static float g_budget = 0;


        public static GameObject g_chosenBuilding = null;

        public static bool g_uiGotFocus = false;
        public static bool g_buildingSwitched = false;


        //Names
        public static string[] g_citizenNames = {
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
        public static string[] g_citizenSurnames = {
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

    class Building {

        public Building() {
            ID = 0;
            building = null;
            Closed = false;
            CitizenLimit = 0;
            WasInsideCount = 0;
            NowInsideCount = 0;
            Profit = 0;
        }

        public Building(int id, GameObject building_gameobject, bool closed = false, int citizen_lim = 0, int was_inside_count = 0, int now_inside_count = 0, float profit = 0)
        {
            ID = id;
            building = building_gameobject;
            Closed = closed;
            CitizenLimit = citizen_lim;
            WasInsideCount = was_inside_count;
            NowInsideCount = now_inside_count;
            Profit = profit;
        }

        public int ID { get; }
        public int CitizenLimit { get; set; }
        public int WasInsideCount { get; set; }
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
            Profit = GlobalVars.g_profitMultiplier * WasInsideCount;
        }
    }

    class Notification {
        public Notification()
        {
            ID = 0;
            SecondsLeft = 0;
            NotificationObject = null;
            NotificationTimerText = null;
            TargetObject = null;
        }

        public Notification(int id, float seconds, GameObject createdNotification, GameObject timerText, GameObject target)
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
