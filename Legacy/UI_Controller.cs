using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class UI_Controller : MonoBehaviour
    {

        [SerializeField] private GameObject UI;
        [SerializeField] private GameObject worldUI;
        [SerializeField] private GameObject citizenLsit;
        [SerializeField] private Sprite[] LockPics;
        [SerializeField] private GameObject uiNotificationPrefab;
        [SerializeField] private float buildingUI_posOffsetX = 3f;
        [SerializeField] private float buildingUI_posOffsetY = 0f;
        [SerializeField] private float notificationPosOffsetX = 2f;
        [SerializeField] private float notificationPosOffsetY = 2f;
        [SerializeField] private float notificationLifeTime = 3f;


        // Start is called before the first frame update
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
        private GameObject uiBase;
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
                    GlobalVars.g_masksObtained = true;
                    GlobalVars.g_budget -= GlobalVars.g_maskPrice;
                    break;

                case 1:
                    GlobalVars.g_desiObtained = true;
                    GlobalVars.g_budget -= GlobalVars.g_desiPrice;
                    break;

                case 2:
                    GlobalVars.g_pillsObtained = true;
                    GlobalVars.g_budget -= GlobalVars.g_pillsPrice;
                    break;

                case 3:
                    GlobalVars.g_resObtained = true;
                    GlobalVars.g_budget -= GlobalVars.g_resPrice;
                    break;

                case 4:
                    GlobalVars.g_vacObtained = true;
                    GlobalVars.g_budget -= GlobalVars.g_vaccinePrice;
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

            uiBase = UI.transform.Find("UI_Base").gameObject;

            uiMoneySector = uiBase.transform.Find("UI_MoneySector").gameObject;
            uiBudget = uiMoneySector.transform.GetChild(0).Find("UI_Budget").GetComponent<TextMeshProUGUI>();
            uiIncome = uiMoneySector.transform.GetChild(0).Find("UI_Income").GetComponent<TextMeshProUGUI>();

            uiCitizenSector = uiBase.transform.Find("UI_CitizenSector").gameObject;
            uiCitizenIncome = uiCitizenSector.transform.GetChild(0).Find("UI_Income").GetComponent<TextMeshProUGUI>();
            uiCitizenCounter = uiCitizenSector.transform.GetChild(0).Find("UI_Budget").GetComponent<TextMeshProUGUI>();

            uiSickCounter = uiBase.transform.Find("UI_SickSector").GetChild(0).Find("UI_Budget").GetComponent<TextMeshProUGUI>();
            uiSickIncome = uiBase.transform.Find("UI_SickSector").GetChild(0).Find("UI_Income").GetComponent<TextMeshProUGUI>();

            uiBuildingInfoPanel = worldUI.transform.Find("UI_BuildingInfoPanel").gameObject;
            uiCitizenLimit = worldUI.transform.Find("UI_CitizenLimitCounter").GetComponent<TextMeshProUGUI>();
            uiCitizenLimitButtonImage = uiBuildingInfoPanel.transform.Find("UI_CitizenLockButton").GetChild(0).GetComponent<Image>();

            debugText = UI.transform.Find("UI_DebugText").GetComponent<Text>();
            uiCharInfo = UI.transform.Find("UI_CharInfoPanel").gameObject;
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
            if (GlobalVars.g_debugMode && GlobalVars.g_chosenBuilding != null)
            {
                GlobalVars.g_debuggingID = 0;
                GlobalVars.g_debugLog = string.Format(
                    "BuildingID: {0}\n" +
                    "Limit: {1}\n" +
                    "Closed: {2}\n" +
                    "Profit: {3}\n" +
                    "WasInside: {4}\n" +
                    "\nNowInside: {5}" +
                    "\nbiggestHeal: {6}" +
                    "\nbiggestSick: {7}",
                    GlobalVars.g_chosenBuildingID,
                    GlobalVars.g_Buildings[GlobalVars.g_chosenBuildingID].CitizenLimit,
                    GlobalVars.g_Buildings[GlobalVars.g_chosenBuildingID].Closed ? "true" : "false",
                    GlobalVars.g_Buildings[GlobalVars.g_chosenBuildingID].Profit,
                    GlobalVars.g_Buildings[GlobalVars.g_chosenBuildingID].WasInsideCount,
                    GlobalVars.g_Buildings[GlobalVars.g_chosenBuildingID].NowInsideCount,
                    GlobalVars.normalBiggesHeal,
                    GlobalVars.normalBiggestSick
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
        }

        private void unselectBuilding()
        {
            if (GlobalVars.g_chosenBuilding != null)
            {
                GlobalVars.g_chosenBuilding.GetComponent<Animator>().SetBool("Selected", false);
                GlobalVars.g_chosenBuilding = null;
            }

            uiCharInfo.SetActive(false);
            uiBuildingInfoPanel.SetActive(false);
            uiCitizenLimit.gameObject.SetActive(false);
        }

        private void selectBuilding(RaycastHit2D hit)
        {
            if (hit.transform.CompareTag("Building") || hit.transform.CompareTag("BuildingInsideSpot"))
            {
                GlobalVars.g_uiGotFocus = false;

                if (GlobalVars.g_chosenBuilding != null)
                    GlobalVars.g_chosenBuilding.GetComponent<Animator>().SetBool("Selected", false);

                if (hit.transform.CompareTag("Building"))
                    GlobalVars.g_chosenBuilding = hit.transform.gameObject;
                else
                    GlobalVars.g_chosenBuilding = hit.transform.parent.gameObject;

                GlobalVars.g_buildingSwitched = true;

                GlobalVars.g_chosenBuildingID = GlobalVars.g_chosenBuilding.transform.GetSiblingIndex();

                GlobalVars.g_chosenBuilding.GetComponent<Animator>().SetBool("Selected", true);

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
            uiMask.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = shortenValue(GlobalVars.g_maskPrice);
            uiDesi.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = shortenValue(GlobalVars.g_desiPrice);
            uiMed.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = shortenValue(GlobalVars.g_pillsPrice);
            uiRes.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = shortenValue(GlobalVars.g_resPrice);
            uiVac.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = shortenValue(GlobalVars.g_vaccinePrice);

            if (GlobalVars.g_budget >= GlobalVars.g_maskPrice && !GlobalVars.g_masksObtained)
                uiMask.GetComponent<Button>().interactable = true;                 
            else if (!GlobalVars.g_masksObtained)
                uiMask.GetComponent<Button>().interactable = false;

            if (GlobalVars.g_budget >= GlobalVars.g_desiPrice && !GlobalVars.g_desiObtained)
                uiDesi.GetComponent<Button>().interactable = true;
            else if (!GlobalVars.g_desiObtained)
                uiDesi.GetComponent<Button>().interactable = false;

            if (GlobalVars.g_budget >= GlobalVars.g_pillsPrice && !GlobalVars.g_pillsObtained)
                uiMed.GetComponent<Button>().interactable = true; 
            else if (!GlobalVars.g_pillsObtained)
                uiMed.GetComponent<Button>().interactable = false;

            if (GlobalVars.g_budget >= GlobalVars.g_resPrice && !GlobalVars.g_resObtained)
                uiRes.GetComponent<Button>().interactable = true;
            else if (!GlobalVars.g_resObtained)
                uiRes.GetComponent<Button>().interactable = false;

            if (GlobalVars.g_budget >= GlobalVars.g_vaccinePrice && !GlobalVars.g_vacObtained && (GlobalVars.g_masksObtained && GlobalVars.g_desiObtained && GlobalVars.g_pillsObtained && GlobalVars.g_resObtained))
                uiVac.GetComponent<Button>().interactable = true;
            else if (!GlobalVars.g_vacObtained)
                uiVac.GetComponent<Button>().interactable = false;
        }

        private void updateSickCount()
        {
            GlobalVars.g_sickCount = citizenLsit.transform.Find("SickList").childCount;

            GlobalVars.g_sickCountModifier = GlobalVars.g_sickCount - lastSickVal;

            uiSickCounter.text = GlobalVars.g_sickCount.ToString();
            
            if (GlobalVars.g_sickCountModifier > 0)
            {
                uiSickIncome.text = "+" + GlobalVars.g_sickCountModifier.ToString();
                uiSickIncome.color = uiRed;
            }
            else if (GlobalVars.g_sickCountModifier < 0)
            {
                uiSickIncome.text = GlobalVars.g_sickCountModifier.ToString();
                uiSickIncome.color = uiGreen;
            }
            else
            {
                uiSickIncome.text = "";
                uiSickIncome.color = uiNeutral;
            }

            lastSickVal = GlobalVars.g_sickCount;
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
            GlobalVars.g_citizenCount = curCitizenVal;
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
                    GlobalVars.g_Buildings[GlobalVars.g_chosenBuildingID].Closed = false;
                    GlobalVars.g_Buildings[GlobalVars.g_chosenBuildingID].building.GetComponent<Animator>().SetBool("Closed", false);
                    uiCitizenLimitButtonImage.sprite = LockPics[1];
                    uiCitizenLimitButtonImage.color = uiGreen;
                    ChangeLimitValue(0);
                }
                else
                {
                    GlobalVars.g_Buildings[GlobalVars.g_chosenBuildingID].Closed = true;
                    GlobalVars.g_Buildings[GlobalVars.g_chosenBuildingID].building.GetComponent<Animator>().SetBool("Closed", true);
                    uiCitizenLimitButtonImage.sprite = LockPics[0];
                    uiCitizenLimitButtonImage.color = uiRed;
                    uiCitizenLimit.text = "CLOSED";
                    uiCitizenLimit.color = uiRed;
                }
            }
            else
            {
                if (GlobalVars.g_Buildings[GlobalVars.g_chosenBuildingID].Closed)
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
                GlobalVars.g_Buildings[GlobalVars.g_chosenBuildingID].CitizenLimit += inputvalue;
                if (GlobalVars.g_Buildings[GlobalVars.g_chosenBuildingID].CitizenLimit > 0) 
                {
                    uiCitizenLimit.text = GlobalVars.g_Buildings[GlobalVars.g_chosenBuildingID].CitizenLimit.ToString();
                    uiCitizenLimit.color = uiYellow;
                }
                else
                {
                    GlobalVars.g_Buildings[GlobalVars.g_chosenBuildingID].CitizenLimit = 0;
                    uiCitizenLimit.text = "NO LIMITS";
                    uiCitizenLimit.color = uiGreen;
                }
            }
            else if (inputvalue == 0)
            {
                if (GlobalVars.g_Buildings[GlobalVars.g_chosenBuildingID].CitizenLimit > 0 && uiCitizenLimitButtonImage.sprite != LockPics[0])
                {
                    uiCitizenLimit.text = GlobalVars.g_Buildings[GlobalVars.g_chosenBuildingID].CitizenLimit.ToString();
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

            GlobalVars.g_buildingSwitched = false;
        }

        private void RefreshCash()
        {

            uiIncome.text = shortenValue(GlobalVars.g_cashModifier, true);

            if (GlobalVars.g_cashModifier > 0)
                uiIncome.color = uiGreen;
            else if (GlobalVars.g_cashModifier < 0)
                uiIncome.color = uiRed;
            else
                uiIncome.color = uiNeutral;

            uiBudget.text = shortenValue(GlobalVars.g_budget);
        }

        private void ProcessTime()
        {
            for (int x = 0; x < GlobalVars.g_Notifications.Count; x++)
            {
                var obj = GlobalVars.g_Notifications[x];
                obj.SecondsLeft -= 0.025f;

                if (obj.SecondsLeft <= 0)
                    DeleteNotification(obj.ID);
                else
                    obj.NotificationTimerText.GetComponent<TextMeshProUGUI>().text = Mathf.RoundToInt(obj.SecondsLeft).ToString() + " sec";

            }
        }

        public void DeleteNotification(int id)
        {
            var obj_id = GlobalVars.g_Notifications.FindIndex(x => x.ID == id);

            var obj = GlobalVars.g_Notifications[obj_id];

            Destroy(obj.NotificationObject);

            GlobalVars.g_Notifications.RemoveAt(obj_id);
        }

        private void AddNotification()
        {
            for (int x = 0; x < GlobalVars.g_gotSick.Count; x++)
            {
                var notifObj = Instantiate(uiNotificationPrefab, worldUI.transform).gameObject;
                var timerObj = notifObj.transform.Find("UI_NotificationBubleMain").Find("UI_NotificationTimerText").gameObject;
                Button extbut = notifObj.transform.Find("UI_NotificationBubleMain").Find("UI_NotificationDelete").GetComponent<Button>();

                extbut.onClick.AddListener(() => DeleteNotification(notifObj.transform.GetInstanceID()));

                notifObj.transform.Find("UI_NotificationBubleMain").Find("UI_NotificationText").GetComponent<TextMeshProUGUI>().text = GlobalVars.g_gotSickNames[x] + " got sick!";

                GlobalVars.g_Notifications.Insert(0, new Notification(notifObj.transform.GetInstanceID(), notificationLifeTime, notifObj, timerObj, GlobalVars.g_gotSick[x]));
            }

            GlobalVars.g_gotSick.Clear();
            GlobalVars.g_gotSickNames.Clear();
        }

        IEnumerator UpdateUIFast()
        {
            for (; ; )
            {
                if (GlobalVars.g_Notifications.Count > 0)
                    ProcessTime();

                if (GlobalVars.g_gotSick.Count > 0)
                    AddNotification();

                if (GlobalVars.g_Notifications.Count > 0)
                {

                    for (int x = 0; x < GlobalVars.g_Notifications.Count; x++)
                    {
                        var obj = GlobalVars.g_Notifications[x];
                        var pos = new Vector2(obj.TargetObject.transform.position.x + notificationPosOffsetX, obj.TargetObject.transform.position.y + notificationPosOffsetY);
                        obj.NotificationObject.transform.position = pos;
                    }
                }

                if (GlobalVars.g_chosenBuilding != null && !GlobalVars.g_uiGotFocus)
                {
                    GlobalVars.g_uiGotFocus = true;

                    uiBuildingInfoPanel.SetActive(true);
                    uiCitizenLimit.gameObject.SetActive(true);

                    transform.GetComponent<Animator>().SetBool("Hovered", true);

                    Vector3 transfpos = GlobalVars.g_chosenBuilding.transform.position;
                    uiCitizenLimit.transform.position = transfpos;
                    uiBuildingInfoPanel.transform.position = new Vector3(transfpos.x + buildingUI_posOffsetX, transfpos.y + buildingUI_posOffsetY, 0);
                }

                debugText.text = GlobalVars.g_debugLog;

                if (uiCharInfo.activeSelf)
                {
                    uiCharInfo.transform.position = Input.mousePosition + new Vector3(140, -65, 0);
                }

                if (GlobalVars.g_hovering_Char && !uiCharInfo.activeSelf)
                {
                    uiCharInfoText.text = GlobalVars.g_infoChar;

                    uiCharInfo.SetActive(true);
                }
                else if (!GlobalVars.g_hovering_Char && uiCharInfo.activeSelf)
                {
                    uiCharInfo.SetActive(false);
                }


                if (GlobalVars.g_buildingSwitched)
                    InitBuildingInfo();

                yield return new WaitForSeconds(0.025f);
            }
        }
    }
}


