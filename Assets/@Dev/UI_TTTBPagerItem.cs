using UnityEngine;

public class UI_TTTBPagerItem : UI_UGUI
{
    enum Buttons
    {
        TTTBPagerItemAddButton
    }
    enum Texts
    {
        TTTBPagerItemNameText,
        TTTBPagerItemOwnedText,
        TTTBPagerItemAmountText,
        TTTBPagerItemAddButtonText
    }

    private IngredientInfo _ingredientInfo;
    private IngredientsData _ingredientData;

    protected override void Awake()
    {
        base.Awake();
        
        BindButtons(typeof(Buttons));
        BindTexts(typeof(Texts));
        
        GetButton((int)Buttons.TTTBPagerItemAddButton).onClick.AddListener(OnClickAddButton);
    }

    public void SetInfo(IngredientInfo ingredientInfo)
    {
        _ingredientInfo = ingredientInfo;
        
        // IngredientID로 IngredientData 가져오기
        if (DataManager.Instance.IngredientDict.TryGetValue(ingredientInfo.IngredientID, out var ingredientData))
        {
            _ingredientData = ingredientData;
        }
        else
        {
            Debug.LogWarning($"Ingredient data not found for ID: {ingredientInfo.IngredientID}");
        }
        
        RefreshUI();
    }
    
    public override void RefreshUI()
    {
        base.RefreshUI();
        
        if (_ingredientInfo == null || _ingredientData == null)
        {
            Debug.LogWarning("Ingredient info or data is null in RefreshUI");
            return;
        }
        
        // 재료 이름 표시
        GetText((int)Texts.TTTBPagerItemNameText).text = _ingredientData.NameTextID;
        
        // 현재 보유량 표시
        int currentQuantity = GameManager.Instance.GetIngredientQuantity(_ingredientData.IngredientID);
        GetText((int)Texts.TTTBPagerItemOwnedText).text = currentQuantity.ToString();
        
        // 필요한 수량 표시
        GetText((int)Texts.TTTBPagerItemAmountText).text = _ingredientInfo.Quantity.ToString();
        
        // 버튼 텍스트 설정 (가격 표시)
        GetText((int)Texts.TTTBPagerItemAddButtonText).text = _ingredientData.Price.ToString();
    }
    
    private void OnClickAddButton()
    {
        if (_ingredientData == null)
        {
            Debug.LogWarning("Ingredient data is null!");
            return;
        }
        
        // 재료 수량 1개 증가
        bool success = GameManager.Instance.AddIngredient(_ingredientData.IngredientID, 1);
        
        if (success)
        {
            Debug.Log($"Added ingredient: {_ingredientData.NameTextID}, New quantity: {GameManager.Instance.GetIngredientQuantity(_ingredientData.IngredientID)}");
            
            // UI 갱신
            RefreshUI();
        }
    }
}
