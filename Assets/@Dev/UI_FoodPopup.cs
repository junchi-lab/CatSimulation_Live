using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UI_FoodPopup : UI_UGUI, IUI_Popup
{
    enum GameObjects
    {
        Content,
    }
    
    enum Buttons
    {
        
        FoodOKButton,
        NoodleTabButton,
        RiceTabButton,
        MainTabButton
    }
    
    enum Texts
    {
        FoodTitleText,
        FoodOKButtonText,
        NoodleTabButtonText,
        RiceTabButtonText,
        MainTabButtonText
    }

    private Transform _content;
    private List<UI_AddPopupItem> _items = new List<UI_AddPopupItem>();
    
    // 탭 색상 정의
    private Color _selectedTabColor = new Color(1f, 0.8f, 0.4f); // 선택된 탭 색상 (주황색)
    private Color _normalTabColor = Color.white; // 기본 탭 색상

    protected override void Awake()
    {
        base.Awake();
        
        BindObjects(typeof(GameObjects));
        BindButtons(typeof(Buttons));
        BindTexts(typeof(Texts));
        
        _content = GetObject((int)GameObjects.Content).transform;
        LoadNoodle();
        
        // 초기 탭 색상 설정
        UpdateTabColors(Buttons.NoodleTabButton);
        
        // Close 버튼 이벤트 등록
        GetButton((int)Buttons.FoodOKButton).onClick.AddListener(() =>
        {
            UIManager.Instance.ClosePopupUI();
        });
        
        GetButton((int)Buttons.NoodleTabButton).onClick.AddListener(() =>
        {
            LoadNoodle();
            UpdateTabColors(Buttons.NoodleTabButton);
        });
        
        GetButton((int)Buttons.RiceTabButton).onClick.AddListener(() =>
        {
            LoadRice();
            UpdateTabColors(Buttons.RiceTabButton);
        });
        
        GetButton((int)Buttons.MainTabButton).onClick.AddListener(() =>
        {
            LoadMain();
            UpdateTabColors(Buttons.MainTabButton);
        });
    }

    private void UpdateTabColors(Buttons selectedTab)
    {
        // 모든 탭 버튼을 기본 색상으로 설정
        GetButton((int)Buttons.NoodleTabButton).GetComponent<Image>().color = _normalTabColor;
        GetButton((int)Buttons.RiceTabButton).GetComponent<Image>().color = _normalTabColor;
        GetButton((int)Buttons.MainTabButton).GetComponent<Image>().color = _normalTabColor;

        // 선택된 탭만 강조 색상으로 변경
        GetButton((int)selectedTab).GetComponent<Image>().color = _selectedTabColor;
    }

    private void LoadNoodle()
    {
        var foodDataDic = DataManager.Instance.FoodDict;
        if (foodDataDic == null || foodDataDic.Count == 0)
        {
            Debug.LogWarning("No food data available!");
            return;
        }

        // FoodType이 1인 면 종류 필터링
        var noodleList = foodDataDic.Values.Where(f => f.FoodType == 1).ToList();
        LoadFoodItems(noodleList);
    }

    private void LoadRice()
    {
        var foodDataDic = DataManager.Instance.FoodDict;
        if (foodDataDic == null || foodDataDic.Count == 0)
        {
            Debug.LogWarning("No food data available!");
            return;
        }

        // FoodType이 2인 밥 종류 필터링
        var riceList = foodDataDic.Values.Where(f => f.FoodType == 2).ToList();
        LoadFoodItems(riceList);
    }

    private void LoadMain()
    {
        var foodDataDic = DataManager.Instance.FoodDict;
        if (foodDataDic == null || foodDataDic.Count == 0)
        {
            Debug.LogWarning("No food data available!");
            return;
        }

        // FoodType이 3인 메인 요리 필터링
        var mainList = foodDataDic.Values.Where(f => f.FoodType == 3).ToList();
        LoadFoodItems(mainList);
    }

    private void LoadFoodItems(List<FoodData> foodList)
    {
        // 기존 아이템 제거
        _content.DestroyChildren();
        _items.Clear();

        // 음식 아이템 생성
        foreach (var foodData in foodList)
        {
            UI_AddPopupItem item = UIManager.Instance.ShowUI<UI_AddPopupItem>("UI_AddPopupItem");
            item.transform.SetParent(_content);
            item.SetInfo(foodData);
            _items.Add(item);
        }
    }

    public override void RefreshUI()
    {
        base.RefreshUI();
        
        GetText((int)Texts.FoodTitleText).SetLocalizedText("AddFood");
        GetText((int)Texts.FoodOKButtonText).SetLocalizedText("CONFIRM");
        
        GetText((int)Texts.NoodleTabButtonText).SetLocalizedText("Noodle");
        GetText((int)Texts.RiceTabButtonText).SetLocalizedText("Rice");
        GetText((int)Texts.MainTabButtonText).SetLocalizedText("MainDish");
    }
}
