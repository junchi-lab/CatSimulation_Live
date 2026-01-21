using UnityEngine;
using UnityEngine.UI;

public class UI_SelectPopupItem : UI_UGUI
{
    
    enum Texts
    {
        NameTextTitle,
    }

    enum Buttons
    {
        ItemSelectButton
    }
    
    private FoodData _foodData;

    protected override void Awake()
    {
        base.Awake();
        
        BindTexts(typeof(Texts));
        BindButtons(typeof(Buttons));
        
        GetButton((int)Buttons.ItemSelectButton).onClick.AddListener(OnClickSelectButton);
    }
    
    private void OnClickSelectButton()
    {
        if (_foodData == null)
        {
            Debug.LogWarning("No food data available!");
            return;
        }
        
        Debug.Log($"Opening UI_PagerPopup for food: {_foodData.NameTextID}");
        
        // UI_PagerPopup을 띄우고 선택한 메뉴 정보 전달
        var pagerPopup = UIManager.Instance.ShowPopupUI<UI_PagerPopup>("UI_PagerPopup");
        pagerPopup.SetInfo(_foodData.FoodID);
    }

    public void SetInfo(FoodData foodData)
    {
        _foodData = foodData;
    }
    
    public override void RefreshUI()
    {
        base.RefreshUI();
        
        // 음식 이름 표시
        GetText((int)Texts.NameTextTitle).text = _foodData.NameTextID;
        
    }
}
