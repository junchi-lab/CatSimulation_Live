using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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

    protected override void Awake()
    {
        base.Awake();
        
        BindObjects(typeof(GameObjects));
        BindButtons(typeof(Buttons));
        BindTexts(typeof(Texts));
        
        _content = GetObject((int)GameObjects.Content).transform;
        LoadNoodle();
        
        // Close 버튼 이벤트 등록
        GetButton((int)Buttons.FoodOKButton).onClick.AddListener(() =>
        {
            UIManager.Instance.ClosePopupUI();
        });
        
        GetButton((int)Buttons.NoodleTabButton).onClick.AddListener(() =>
        {
            LoadNoodle();
        });
        
        GetButton((int)Buttons.RiceTabButton).onClick.AddListener(() =>
        {
            LoadRice();
        });
        
        GetButton((int)Buttons.MainTabButton).onClick.AddListener(() =>
        {
            LoadMain();
        });
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
