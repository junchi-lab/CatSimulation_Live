using System.Collections.Generic;
using System.Linq;
using Unity.Android.Gradle.Manifest;
using UnityEngine;
using UnityEngine.UI;

public class UI_PagerPopup : UI_UGUI, IUI_Popup
{
    enum GameObjects
    {
        Content,
        PagerPictureImage
    }
    
    enum Buttons
    {
        BTBPreviousButton,
        BTBNextButton,
        PagerAddButton
        
    }
    
    enum Texts
    {
        BTBPreviousButtonText,
        BTBTitleText,
        BTBNextButtonText,
        PagerAddButtonText
    }
    
    private Transform _content;
    private List<UI_TTTBPagerItem> _items = new List<UI_TTTBPagerItem>();
    private List<FoodData> _menuList = new List<FoodData>();
    private int _index = 0;
    
    protected override void Awake()
    {
        base.Awake();
        
        BindObjects(typeof(GameObjects));
        BindButtons(typeof(Buttons));
        BindTexts(typeof(Texts));
        
        _content = GetObject((int)GameObjects.Content).transform;
        
        var foodDataDic = DataManager.Instance.FoodDict;
        var addedFoodIDs = GameManager.Instance.GameData.AddedFoodIDs;
        
        _menuList = foodDataDic.Values
            .Where(f => addedFoodIDs.Contains(f.FoodID))
            .ToList();
        
        // 이전/다음 버튼 이벤트 등록
        GetButton((int)Buttons.BTBPreviousButton).onClick.AddListener(OnClickPrevious);
        GetButton((int)Buttons.BTBNextButton).onClick.AddListener(OnClickNext);
        GetButton((int)Buttons.PagerAddButton).onClick.AddListener(OnClickClose);
    }
    
    public void SetInfo(int foodId)
    {
        _index = _menuList.FindIndex(f => f.FoodID == foodId);
        
        if (_index < 0)
        {
            _index = 0; // 찾지 못하면 첫 번째로
            Debug.LogWarning($"FoodID {foodId} not found in menu list. Defaulting to index 0.");
        }
        
        
    }
    
    private void ClearItems()
    {
        // 기존 아이템 제거
        foreach (var item in _items)
        {
            if (item != null)
            {
                Destroy(item.gameObject);
            }
        }
        _items.Clear();
    }
    
    private void CreateItems()
    {
        // 기존 아이템 제거
        ClearItems();
        
        // 현재 메뉴의 재료 아이템 생성
        if (_menuList == null || _menuList.Count == 0 || _index < 0 || _index >= _menuList.Count)
        {
            Debug.LogWarning("Cannot create items: Invalid menu list or index");
            return;
        }
        
        foreach (var ingredients in _menuList[_index].Ingredients)
        {
            UI_TTTBPagerItem item = UIManager.Instance.ShowUI<UI_TTTBPagerItem>("UI_TTTBPagerItem");
            item.transform.SetParent(_content);
            item.SetInfo(ingredients);
            _items.Add(item);
        }
    }
    
    private void OnClickPrevious()
    {
        if (_menuList.Count == 0) return;
        
        _index--;
        if (_index < 0)
        {
            _index = _menuList.Count - 1; // 마지막 메뉴로 이동
        }

        RefreshUI();
    }

    private void OnClickNext()
    {
        if (_menuList.Count == 0) return;
        
        _index++;
        if (_index >= _menuList.Count)
        {
            _index = 0; // 처음 메뉴로 이동
        }

        RefreshUI();
    }
    
    private void OnClickClose()
    {
        UIManager.Instance.ClosePopupUI();
    }
    
    public override void RefreshUI()
    {
        base.RefreshUI();

        // _menuList가 비어있거나 _index가 유효하지 않으면 리턴
        if (_menuList == null || _menuList.Count == 0 || _index < 0 || _index >= _menuList.Count)
        {
            Debug.LogWarning("Invalid menu list or index in RefreshUI");
            return;
        }
        
        GetText((int)Texts.PagerAddButtonText).SetLocalizedText("CONFIRM");
        
        // 타이틀 표시
        GetText((int)Texts.BTBTitleText).text = _menuList[_index].NameTextID;
        // 재료 아이템 생성
        CreateItems();

    }
}
