using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class UI_Behavior : MonoBehaviour
    {

        [SerializeField] private GameObject UI;
        [SerializeField] private GameObject worldUI;
        [SerializeField] private GameObject citizenLsit;
        [SerializeField] private Sprite[] LockPics;
        [SerializeField] private GameObject uiNotificationPrefab;
        [SerializeField] private float buildingUI_posOffsetX = 0f;
        [SerializeField] private float buildingUI_posOffsetY = -3.5f;
        [SerializeField] private float notificationPosOffsetX = 2f;
        [SerializeField] private float notificationPosOffsetY = 2f;
        [SerializeField] private float notificationLifeTime = 3f;


        // Start is called before the first frame update
        private Slider uiSickProgress;

        private Image uiCitizenLimitButtonImage;

        private TextMeshProUGUI uiCharInfoText;
        private Text debugText;
        private TextMeshProUGUI uiBudget;
        private TextMeshProUGUI uiIncome;
        private TextMeshProUGUI uiCitizenCounter;
        private TextMeshProUGUI uiCitizenIncome;
        private TextMeshProUGUI uiSickCounter;
        private TextMeshProUGUI uiSickIncome;
        private TextMeshProUGUI uiCitizenLimit;

        private GameObject uiCharInfo;
        private GameObject uiMoneySector;
        private GameObject uiCitizenSector;
        private GameObject uiBuildingInfoPanel;
        private GameObject uiShopPanel = null;
        private GameObject lastHovered;


        /// TECH TREE OBJECTS
        private GameObject uiMask;
        private GameObject uiDesi;
        private GameObject uiMed;
        private GameObject uiRes;
        private GameObject uiVac;

        private GameObject uiDecoMask;
        private GameObject uiDecoDesi;
        private GameObject uiDecoMed;
        private GameObject uiDecoRes;

        private int curCitizenVal = 0;
        private int newCinizenVal = 0;

        private int lastSickVal = 0;

        private Color uiRed = new Color32(255, 0, 0, 255);
        private Color uiGreen = new Color32(0, 255, 0, 255);
        private Color uiNeutral = new Color32(32, 42, 37, 255);
        private Color uiYellow = new Color32(255 ,195 ,0, 255);
        private Color uiBlue = new Color32(0 ,178 ,255 ,255);

        void Start()
        {
            SetupUI();
            SetupTechTree();

            uiCharInfo.SetActive(false);
            uiCharInfoText = uiCharInfo.transform.GetChild(0).GetComponent<TextMeshProUGUI>();

            uiBuildingInfoPanel.SetActive(false);
            uiCitizenLimit.gameObject.SetActive(false);

            StartCoroutine(UpdateUIFast());
            StartCoroutine(UpdateUISlow());
        }

        private void SetupTechTree()
        {
            //Needed Buttons
            //UI_Mask
            //UI_Desi
            //UI_Med
            //UI_Res
            //UI_Vac
            ////////////////
            ///Needed Decorative
            //UI_DecoMask
            //UI_DecoDesi
            //UI_DecoMed
            //UI_DecoRes
            //UI_DecoVac
            /////////////

            uiMask = uiShopPanel.transform.Find("UI_Mask").gameObject;
            uiDesi = uiShopPanel.transform.Find("UI_Desi").gameObject;
            uiMed = uiShopPanel.transform.Find("UI_Med").gameObject;
            uiRes = uiShopPanel.transform.Find("UI_Res").gameObject;
            uiVac = uiShopPanel.transform.Find("UI_Vac").gameObject;

            uiDecoMask = uiShopPanel.transform.Find("UI_DecoMask").gameObject;
            uiDecoDesi = uiShopPanel.transform.Find("UI_DecoDesi").gameObject;
            uiDecoMed = uiShopPanel.transform.Find("UI_DecoMed").gameObject;
            uiDecoRes = uiShopPanel.transform.Find("UI_DecoRes").gameObject;


            uiMask.GetComponent<Button>().interactable = false;
            uiDesi.GetComponent<Button>().interactable = false;
            uiMed.GetComponent<Button>().interactable = false;
            uiRes.GetComponent<Button>().interactable = false;
            uiVac.GetComponent<Button>().interactable = false;

            
            uiMask.GetComponent<Button>().onClick.AddListener(() => GetReward(0,uiMask, uiDecoMask));
            uiDesi.GetComponent<Button>().onClick.AddListener(() => GetReward(1, uiDesi, uiDecoDesi));
            uiMed.GetComponent<Button>().onClick.AddListener(() => GetReward(2, uiMed, uiDecoMed));
            uiRes.GetComponent<Button>().onClick.AddListener(() => GetReward(3, uiRes, uiDecoRes));
            uiVac.GetComponent<Button>().onClick.AddListener(() => GetReward(4, uiVac));
        }

        public void GetReward(int rewardtype, GameObject button, GameObject rayobject = null)
        {
            switch (rewardtype)
            {
                case 0:
                    Globals.masksObtained = true;
                    Globals.budget -= Globals.maskPrice;
                    break;

                case 1:
                    Globals.desiObtained = true;
                    Globals.budget -= Globals.desiPrice;
                    break;

                case 2:
                    Globals.pillsObtained = true;
                    Globals.budget -= Globals.pillsPrice;
                    break;

                case 3:
                    Globals.resObtained = true;
                    Globals.budget -= Globals.resPrice;
                    break;

                case 4:
                    Globals.vacObtained = true;
                    Globals.budget -= Globals.vaccinePrice;
                    break;

                default: break;
            }

            button.GetComponent<Button>().interactable = false;

            if (rayobject != null)
            {
                rayobject.GetComponent<Image>().color = uiBlue;
                rayobject.transform.GetChild(0).GetComponent<Image>().enabled = false;
            }

            var colorBlock = button.GetComponent<Button>().colors;
            colorBlock.disabledColor = uiGreen;
            button.GetComponent<Button>().colors = colorBlock;
        }


        private void SetupUI()
        {
            uiShopPanel = UI.transform.Find("UI_ShopPanel").gameObject;

            uiMoneySector = UI.transform.Find("UI_MoneySector").gameObject;
            uiBudget = uiMoneySector.transform.GetChild(0).Find("UI_Budget").GetComponent<TextMeshProUGUI>();
            uiIncome = uiMoneySector.transform.GetChild(0).Find("UI_Income").GetComponent<TextMeshProUGUI>();

            uiCitizenSector = UI.transform.Find("UI_CitizenSector").gameObject;
            uiCitizenIncome = uiCitizenSector.transform.GetChild(0).Find("UI_Income").GetComponent<TextMeshProUGUI>();
            uiCitizenCounter = uiCitizenSector.transform.GetChild(0).Find("UI_Budget").GetComponent<TextMeshProUGUI>();

            uiSickCounter = UI.transform.Find("UI_SickSector").GetChild(0).Find("UI_Budget").GetComponent<TextMeshProUGUI>();
            uiSickIncome = UI.transform.Find("UI_SickSector").GetChild(0).Find("UI_Income").GetComponent<TextMeshProUGUI>();

            uiBuildingInfoPanel = worldUI.transform.Find("UI_BuildingInfoPanel").gameObject;
            uiCitizenLimit = worldUI.transform.Find("UI_CitizenLimitCounter").GetComponent<TextMeshProUGUI>();
            uiCitizenLimitButtonImage = uiBuildingInfoPanel.transform.Find("UI_CitizenLockButton").GetChild(0).GetComponent<Image>();

            debugText = UI.transform.Find("UI_DebugText").GetComponent<Text>();
            uiCharInfo = UI.transform.Find("UI_CharInfoPanel").gameObject;
            uiSickProgress = uiCharInfo.transform.Find("ImmunityPanel").Find("ImmunityBar").GetComponent<Slider>();
        }

        static string shortenValue(float input, bool plusMode = false)
        {
            string output = "";

            string[] suf = {"", "K", "M", "B" }; //Longs run out around EB
            if (input == 0)
                return "0" + suf[0];
            float val = Mathf.Abs(input);
            int place = (int) Mathf.Floor(Mathf.Log(val, 1000));
            double num = Mathf.Round(val / Mathf.Pow(1000, place));

            output = (Mathf.Sign(input) * num).ToString() + suf[place];

            if (plusMode && input > 0)
            {
                output = "+" + output;
            }

            return output;
        }


        // Update is called once per frame
        void FixedUpdate()
        {
            Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);

            if (hit)
            {
                checkHover(hit);
            }
            else
            {
                if (lastHovered != null)
                {
                    lastHovered.GetComponent<Animator>().SetFloat("Hovered", 0f);
                    lastHovered = null;
                }
                    
            }

            if (Input.GetAxis("Fire2") > 0)
            {            
                if (hit.transform != null)
                {
                    selectBuilding(hit);
                }
                else
                {
                    unselectBuilding();
                }
            }
            if (Globals.debugMode && Globals.chosenBuilding != null)
            {
                Globals.debuggingID = 0;
                Globals.debugLog = string.Format(
                    "BuildingID: {0}\n" +
                    "Limit: {1}\n" +
                    "Closed: {2}\n" +
                    "Profit: {3}\n" +
                    "\nNowInside: {4}",

                    Globals.chosenBuildingID,
                    Globals.Buildings[Globals.chosenBuildingID].CitizenLimit,
                    Globals.Buildings[Globals.chosenBuildingID].Closed ? "true" : "false",
                    Globals.Buildings[Globals.chosenBuildingID].Profit,
                    Globals.Buildings[Globals.chosenBuildingID].NowInsideCount

                    );
            }
        }

        private void checkHover(RaycastHit2D hit)
        {
            if (hit.transform.CompareTag("Building") || hit.transform.CompareTag("BuildingInsideSpot"))
            {
                var hitObject = hit.transform;

                if (!hitObject.CompareTag("Building"))
                    hitObject = hitObject.parent;

                if (lastHovered != null && lastHovered != hitObject.gameObject)
                    lastHovered.GetComponent<Animator>().SetFloat("Hovered", 0f);

                if (hitObject.gameObject != lastHovered)
                {
                    var chosenFadeAnimator = hitObject.GetComponent<Animator>();
                    chosenFadeAnimator.SetFloat("Hovered", 1f);

                    lastHovered = hitObject.gameObject;
                }


            }
            if (hit.transform.gameObject == uiIncome.gameObject)
            {
                Debug.Log("Income ToolTip HERE");
            }
        }

        private void unselectBuilding()
        {
            if (Globals.chosenBuilding != null)
            {
                Globals.chosenBuilding.GetComponent<Animator>().SetBool("Selected", false);
                Globals.chosenBuilding = null;
            }

            uiCharInfo.SetActive(false);
            uiBuildingInfoPanel.SetActive(false);
            uiCitizenLimit.gameObject.SetActive(false);
        }

        private void selectBuilding(RaycastHit2D hit)
        {
            if (hit.transform.CompareTag("Building") || hit.transform.CompareTag("BuildingInsideSpot"))
            {
                Globals.uiGotFocus = false;

                if (Globals.chosenBuilding != null)
                    Globals.chosenBuilding.GetComponent<Animator>().SetBool("Selected", false);

                if (hit.transform.CompareTag("Building"))
                    Globals.chosenBuilding = hit.transform.gameObject;
                else
                    Globals.chosenBuilding = hit.transform.parent.gameObject;

                Globals.buildingSwitched = true;

                Globals.chosenBuildingID = Globals.chosenBuilding.transform.GetSiblingIndex();

                Globals.chosenBuilding.GetComponent<Animator>().SetBool("Selected", true);

            }
        }

        IEnumerator UpdateUISlow()
        {
            for(; ; )
            {
                updateSickCount();

                citizenCheck();

                RefreshCash();

                RefreshTechButtons();

                yield return new WaitForSeconds(0.25f);
            }
        }

        private void RefreshTechButtons()
        {
            uiMask.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = shortenValue(Globals.maskPrice);
            uiDesi.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = shortenValue(Globals.desiPrice);
            uiMed.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = shortenValue(Globals.pillsPrice);
            uiRes.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = shortenValue(Globals.resPrice);
            uiVac.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = shortenValue(Globals.vaccinePrice);

            if (Globals.budget >= Globals.maskPrice && !Globals.masksObtained)
                uiMask.GetComponent<Button>().interactable = true;                 
            else if (!Globals.masksObtained)
                uiMask.GetComponent<Button>().interactable = false;

            if (Globals.budget >= Globals.desiPrice && !Globals.desiObtained)
                uiDesi.GetComponent<Button>().interactable = true;
            else if (!Globals.desiObtained)
                uiDesi.GetComponent<Button>().interactable = false;

            if (Globals.budget >= Globals.pillsPrice && !Globals.pillsObtained)
                uiMed.GetComponent<Button>().interactable = true; 
            else if (!Globals.pillsObtained)
                uiMed.GetComponent<Button>().interactable = false;

            if (Globals.budget >= Globals.resPrice && !Globals.resObtained)
                uiRes.GetComponent<Button>().interactable = true;
            else if (!Globals.resObtained)
                uiRes.GetComponent<Button>().interactable = false;

            if (Globals.budget >= Globals.vaccinePrice && !Globals.vacObtained && (Globals.masksObtained && Globals.desiObtained && Globals.pillsObtained && Globals.resObtained))
                uiVac.GetComponent<Button>().interactable = true;
            else if (!Globals.vacObtained)
                uiVac.GetComponent<Button>().interactable = false;
        }

        private void updateSickCount()
        {
            Globals.sickCount = citizenLsit.transform.Find("SickList").childCount;

            Globals.sickCountModifier = Globals.sickCount - lastSickVal;

            uiSickCounter.text = Globals.sickCount.ToString();
            
            if (Globals.sickCountModifier > 0)
            {
                uiSickIncome.text = "+" + Globals.sickCountModifier.ToString();
                uiSickIncome.color = uiRed;
            }
            else if (Globals.sickCountModifier < 0)
            {
                uiSickIncome.text = Globals.sickCountModifier.ToString();
                uiSickIncome.color = uiGreen;
            }
            else
            {
                uiSickIncome.text = "";
                uiSickIncome.color = uiNeutral;
            }

            lastSickVal = Globals.sickCount;
        }

        private void citizenCheck()
        {
            newCinizenVal = citizenLsit.transform.GetChild(0).childCount + citizenLsit.transform.GetChild(1).childCount;

            uiCitizenIncome.text = (newCinizenVal - curCitizenVal).ToString();

            if (newCinizenVal - curCitizenVal > 0)
            {
                uiCitizenIncome.color = uiGreen;
                uiCitizenIncome.text = "+" + uiCitizenIncome.text;
            }
            else if (newCinizenVal - curCitizenVal < 0)
                uiCitizenIncome.color = uiRed;
            else
            {
                uiCitizenIncome.color = uiNeutral;
                uiCitizenIncome.text = "";
            }


            uiCitizenCounter.text = newCinizenVal.ToString();

            curCitizenVal = newCinizenVal;
            Globals.citizenCount = curCitizenVal;
        }

        public void SwitchOpenShop(bool mode)
        {
            uiShopPanel.GetComponent<Animator>().SetBool("ShopOpened", mode);
        }

        public void switchLockState(bool forcemode = false)
        {
            if (!forcemode)
            {
                if (uiCitizenLimitButtonImage.sprite == LockPics[0])
                {
                    Globals.Buildings[Globals.chosenBuildingID].Closed = false;
                    Globals.Buildings[Globals.chosenBuildingID].building.GetComponent<Animator>().SetBool("Closed", false);
                    uiCitizenLimitButtonImage.sprite = LockPics[1];
                    uiCitizenLimitButtonImage.color = uiGreen;
                    ChangeLimitValue(0);
                }
                else
                {
                    Globals.Buildings[Globals.chosenBuildingID].Closed = true;
                    Globals.Buildings[Globals.chosenBuildingID].building.GetComponent<Animator>().SetBool("Closed", true);
                    uiCitizenLimitButtonImage.sprite = LockPics[0];
                    uiCitizenLimitButtonImage.color = uiRed;
                    uiCitizenLimit.text = "CLOSED";
                    uiCitizenLimit.color = uiRed;
                }
            }
            else
            {
                if (Globals.Buildings[Globals.chosenBuildingID].Closed)
                {
                    uiCitizenLimitButtonImage.sprite = LockPics[0];
                    uiCitizenLimitButtonImage.color = uiRed;
                    uiCitizenLimit.text = "CLOSED";
                    uiCitizenLimit.color = uiRed;
                }
                else
                {
                    uiCitizenLimitButtonImage.sprite = LockPics[1];
                    uiCitizenLimitButtonImage.color = uiGreen;
                }
            }
        }

        public void ChangeLimitValue(int inputvalue)
        {
            if (inputvalue != 0 && uiCitizenLimit.text != "CLOSED")
            {
                Globals.Buildings[Globals.chosenBuildingID].CitizenLimit += inputvalue;
                if (Globals.Buildings[Globals.chosenBuildingID].CitizenLimit > 0) 
                {
                    uiCitizenLimit.text = Globals.Buildings[Globals.chosenBuildingID].CitizenLimit.ToString();
                    uiCitizenLimit.color = uiYellow;
                }
                else
                {
                    Globals.Buildings[Globals.chosenBuildingID].CitizenLimit = 0;
                    uiCitizenLimit.text = "NO LIMITS";
                    uiCitizenLimit.color = uiGreen;
                }
            }
            else if (inputvalue == 0)
            {
                if (Globals.Buildings[Globals.chosenBuildingID].CitizenLimit > 0 && uiCitizenLimitButtonImage.sprite != LockPics[0])
                {
                    uiCitizenLimit.text = Globals.Buildings[Globals.chosenBuildingID].CitizenLimit.ToString();
                    uiCitizenLimit.color = uiYellow;
                }
                else if (uiCitizenLimitButtonImage.sprite != LockPics[0])
                {
                    uiCitizenLimit.text = "NO LIMITS";
                    uiCitizenLimit.color = uiGreen;
                }
                else
                {
                    uiCitizenLimit.text = "CLOSED";
                    uiCitizenLimit.color = uiRed;
                }
            }
        }

        private void InitBuildingInfo()
        {
            switchLockState(true);

            ChangeLimitValue(0);

            Globals.buildingSwitched = false;
        }

        private void RefreshCash()
        {

            uiIncome.text = shortenValue(Globals.cashModifier, true);

            if (Globals.cashModifier > 0)
                uiIncome.color = uiGreen;
            else if (Globals.cashModifier < 0)
                uiIncome.color = uiRed;
            else
                uiIncome.color = uiNeutral;

            uiBudget.text = shortenValue(Globals.budget);
        }

        private void ProcessTime()
        {
            for (int x = 0; x < Globals.Notifications.Count; x++)
            {
                var obj = Globals.Notifications[x];
                obj.SecondsLeft -= 0.025f;

                if (obj.SecondsLeft <= 0)
                    DeleteNotification(obj.ID);
                else
                    obj.NotificationTimerText.GetComponent<TextMeshProUGUI>().text = Mathf.RoundToInt(obj.SecondsLeft).ToString() + " sec";

            }
        }

        public void DeleteNotification(int id)
        {
            var obj_id = Globals.Notifications.FindIndex(x => x.ID == id);

            var obj = Globals.Notifications[obj_id];

            Destroy(obj.NotificationObject);

            Globals.Notifications.RemoveAt(obj_id);
        }

        private void AddNotification()
        {
            for (int x = 0; x < Globals.gotSick.Count; x++)
            {
                var notifObj = Instantiate(uiNotificationPrefab, worldUI.transform).gameObject;
                var timerObj = notifObj.transform.Find("UI_NotificationBubleMain").Find("UI_NotificationTimerText").gameObject;
                Button extbut = notifObj.transform.Find("UI_NotificationBubleMain").Find("UI_NotificationDelete").GetComponent<Button>();

                extbut.onClick.AddListener(() => DeleteNotification(notifObj.transform.GetInstanceID()));

                notifObj.transform.Find("UI_NotificationBubleMain").Find("UI_NotificationText").GetComponent<TextMeshProUGUI>().text = Globals.gotSickNames[x] + " got sick!";

                Globals.Notifications.Insert(0, new Notification(notifObj.transform.GetInstanceID(), notificationLifeTime, notifObj, timerObj, Globals.gotSick[x]));
            }

            Globals.gotSick.Clear();
            Globals.gotSickNames.Clear();
        }

        float testval = 0;

        IEnumerator UpdateUIFast()
        {
            for (; ; )
            {
                if (Globals.Notifications.Count > 0)
                    ProcessTime();

                if (Globals.gotSick.Count > 0)
                    AddNotification();

                if (Globals.Notifications.Count > 0)
                {

                    for (int x = 0; x < Globals.Notifications.Count; x++)
                    {
                        var obj = Globals.Notifications[x];
                        var pos = new Vector2(obj.TargetObject.transform.position.x + notificationPosOffsetX, obj.TargetObject.transform.position.y + notificationPosOffsetY);
                        obj.NotificationObject.transform.position = pos;
                    }
                }

                if (Globals.chosenBuilding != null && !Globals.uiGotFocus)
                {
                    Globals.uiGotFocus = true;

                    uiBuildingInfoPanel.SetActive(true);
                    uiCitizenLimit.gameObject.SetActive(true);

                    transform.GetComponent<Animator>().SetBool("Hovered", true);

                    Vector3 transfpos = Globals.chosenBuilding.transform.position;
                    uiCitizenLimit.transform.position = transfpos;
                    uiBuildingInfoPanel.transform.position = new Vector3(transfpos.x + buildingUI_posOffsetX, transfpos.y + buildingUI_posOffsetY, 0);
                }

                debugText.text = Globals.debugLog;

                if (uiCharInfo.activeSelf)
                {
                    uiCharInfo.transform.position = Input.mousePosition + new Vector3(140, -65, 0);

                    if (testval < 1)
                        testval += 0.01f;
                    else
                        testval = 0;

                    uiSickProgress.value = /*testval;*/Globals.sicknessProgress;
                }

                if (Globals.hoveringChar != 0 && !uiCharInfo.activeSelf)
                {
                    uiCharInfoText.text = Globals.infoChar;

                    uiCharInfo.SetActive(true);
                }
                else if (Globals.hoveringChar == 0 && uiCharInfo.activeSelf)
                {
                    uiCharInfo.SetActive(false);
                }


                if (Globals.buildingSwitched)
                    InitBuildingInfo();

                yield return new WaitForSeconds(0.025f);
            }
        }
    }
}


