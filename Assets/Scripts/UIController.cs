using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;
using UnityEngine.Windows;
using ColorUtility = UnityEngine.ColorUtility;
using Input = UnityEngine.Input;

public class UIController : MonoBehaviour
{
    // Gameobject
    public GameObject AddDieBtn;
    public GameObject RemoveDieBtn;
    public GameObject SettingsBtn;
    public GameObject SettingsPannel;

    // string
    string SavedBackgroundColorKey = "Background";
    string SavedDieColorKey = "Die";

    // FlexibleColorPicker
    public FlexibleColorPicker BackgroundColorPicker;
    public FlexibleColorPicker DieColorPicker;

    // Materials
    public Material BackGroundMat;
    public Material DieMat;

    // Other
    public Canvas GameCanvas;
    bool IsInput;
    Roller DieRoller;

    // Start is called before the first frame update
    void Start()
    {
        DieRoller = transform.GetComponent<Roller>();

        if (PlayerPrefs.HasKey(SavedBackgroundColorKey))
        {
            Color color;
            ColorUtility.TryParseHtmlString("#"+ PlayerPrefs.GetString(SavedBackgroundColorKey), out color);
            BackGroundMat.color = color;
        }

        else
            BackGroundMat.color = BackgroundColorPicker.GetColor();

        if (PlayerPrefs.HasKey(SavedDieColorKey))
        {
            Color color;
            ColorUtility.TryParseHtmlString("#" + PlayerPrefs.GetString(SavedDieColorKey), out color);
            DieMat.color = color;
        }

        else
            DieMat.color = DieColorPicker.color;

        BackgroundColorPicker.color = BackGroundMat.color;
        DieColorPicker.color = DieMat.color;
        
        SaveColors();
        SetUI();
    }

    // Update is called once per frame
    void Update()
    {
        BackGroundMat.color = BackgroundColorPicker.color;
        DieMat.color = DieColorPicker.color;

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.tapCount == 1 && touch.phase == TouchPhase.Ended && !IsInput && GameCanvas.gameObject && !IsPointOnUI(touch))
            {
                DieRoller.RollDice();
                IsInput = true;
            }
        }
        else
            IsInput = false;
    }  
    public void HideUnHideColorPicker()
    {
        if (SettingsPannel.activeSelf)
            SaveColors();

        SettingsPannel.SetActive(!SettingsPannel.activeSelf);
    }

    bool IsPointOnUI(Touch touch)
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = touch.position;
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }

    void SetUI()
    {
        RectTransform addBtnRecTrans = AddDieBtn.GetComponent<RectTransform>();
        RectTransform removeBtnRecTrans = RemoveDieBtn.GetComponent<RectTransform>();
        RectTransform settingsBtnRecTrans = SettingsBtn.GetComponent<RectTransform>();
        RectTransform settingsPannelRecTrans = SettingsPannel.GetComponent<RectTransform>();

        Vector2 newAddRemoveBtnDimensions = GetNewDimensions(addBtnRecTrans.sizeDelta);
        Vector2 newSettingsBtnDimension = GetNewDimensions(settingsBtnRecTrans.sizeDelta);
        Vector2 newSettingsPannelDimensions = GetNewDimensions(settingsPannelRecTrans.sizeDelta);

        addBtnRecTrans.localScale = new Vector3(newAddRemoveBtnDimensions.x/addBtnRecTrans.sizeDelta.x, newAddRemoveBtnDimensions.y / addBtnRecTrans.sizeDelta.y,1);
        removeBtnRecTrans.localScale = new Vector3(newAddRemoveBtnDimensions.x / removeBtnRecTrans.sizeDelta.x, newAddRemoveBtnDimensions.y / addBtnRecTrans.sizeDelta.y, 1);
        settingsBtnRecTrans.localScale = new Vector3(newSettingsBtnDimension.y / settingsBtnRecTrans.sizeDelta.y, newSettingsBtnDimension.y / settingsBtnRecTrans.sizeDelta.y, 1);
        settingsPannelRecTrans.localScale = new Vector3(newSettingsPannelDimensions.x / settingsPannelRecTrans.sizeDelta.x, newSettingsPannelDimensions.y / settingsPannelRecTrans.sizeDelta.y, 1);
    }

    public void SaveColors()
    {
        PlayerPrefs.SetString(SavedBackgroundColorKey, ColorUtility.ToHtmlStringRGB(BackGroundMat.color));
        PlayerPrefs.SetString(SavedDieColorKey, ColorUtility.ToHtmlStringRGB(DieMat.color));
        PlayerPrefs.Save();
    }

    Vector2 GetNewDimensions(Vector2 oldDimensions)
    {
        return new Vector2(GameCanvas.renderingDisplaySize.x * (oldDimensions.x / 540), GameCanvas.renderingDisplaySize.y * (oldDimensions.y / 960));
    }
}
