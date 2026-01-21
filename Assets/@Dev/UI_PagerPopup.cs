using System.Collections.Generic;
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
    private FoodData _currentFoodData;
    
    protected override void Awake()
    {
        base.Awake();
        
        BindObjects(typeof(GameObjects));
        BindButtons(typeof(Buttons));
        BindTexts(typeof(Texts));
        
        _content = GetObject((int)GameObjects.Content).transform;
    }

    public void SetInfo(FoodData foodData)
    {
        _currentFoodData = foodData;
        
        // 타이틀에 메뉴 이름 표시
        GetText((int)Texts.BTBTitleText).SetLocalizedText(foodData.NameTextID);
        
        // 메뉴 상세 정보 표시
        ShowFoodDetails();
    }
    
    private void ShowFoodDetails()
    {
        if (_currentFoodData == null)
            return;
        
        // 기존 아이템 제거
        _content.DestroyChildren();
        _items.Clear();
        
        // 재료 정보 표시
        if (_currentFoodData.Ingredients != null && _currentFoodData.Ingredients.Count > 0)
        {
            foreach (var ingredient in _currentFoodData.Ingredients)
            {
                var ingredientData = DataManager.Instance.IngredientDict[ingredient.IngredientID];
                if (ingredientData != null)
                {
                    UI_TTTBPagerItem item = UIManager.Instance.ShowUI<UI_TTTBPagerItem>("UI_TTTBPagerItem");
                    item.transform.SetParent(_content);
                    // item.SetInfo(ingredientData, ingredient.Quantity); // 재료 정보와 수량 전달
                    _items.Add(item);
                }
            }
        }
    }

    public override void RefreshUI()
    {
        base.RefreshUI();
    }
}
