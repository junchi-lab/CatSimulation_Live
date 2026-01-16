using UnityEngine;

public class UI_StoreInfoPopup : UI_UGUI, IUI_Popup
{
    // 캐싱된 데이터
    private StroeData _stroeData;
    private int _hallStaffCount;
    private int _kitchenStaffCount;
    
    enum GameObjects
    {
    }
     
    enum Buttons
    {
        StoreInfoOKButton
    }
     
    enum Texts
    {
        
        StoreTitleText,
        StoreRentalCostTextTitle,
        StoreRentalCostTextValue,
        StoreSizeTextTitle,
        StoreSizeTextValue,
        MaxCustomersTextTitle,
        MaxCustomersTextValue,
        HallStaffTextTitle,
        HallStaffTextValue,
        KitchenStaffTextTitle,
        KitchenStaffTextValue,
        StoreInfoOKButtonText
            
    }
    
    protected override void Awake()
    {
        base.Awake();
        
        BindObjects(typeof(GameObjects));
        BindButtons(typeof(Buttons));
        BindTexts(typeof(Texts));
        
        // 메인 메뉴 버튼 이벤트 등록
        GetButton((int)Buttons.StoreInfoOKButton).onClick.AddListener(() =>
        {
            Debug.Log("Store Info OK Button Clicked");
            UIManager.Instance.ClosePopupUI();
        });
        
        // 데이터 먼저 로드
        LoadData();
    }
    
    /// <summary>
    /// DataManager에서 데이터를 가져와서 캐싱
    /// </summary>
    private void LoadData()
    {
        // StoreData 가져오기
        _stroeData = DataManager.Instance.StroeData;
        
        if (_stroeData == null)
        {
            Debug.LogWarning("StroeData is null! Make sure DataManager.LoadData() has been called.");
            return;
        }
        
        // Staff 정보 계산
        var staffDict = DataManager.Instance.StaffDict;
        if (staffDict != null)
        {
            _hallStaffCount = 0;
            _kitchenStaffCount = 0;
            
            foreach (var staff in staffDict.Values)
            {
                if (staff.StaffType == 2) // Hall
                    _hallStaffCount++;
                else if (staff.StaffType == 1) // Kitchen
                    _kitchenStaffCount++;
            }
        }
    }
    
    /// <summary>
    /// UI 갱신 (캐싱된 데이터를 UI에 반영)
    /// </summary>
    public override void RefreshUI()
    {
        base.RefreshUI();
        
        if (_stroeData == null)
        {
            Debug.LogWarning("Data is not loaded yet!");
            return;
        }
        
        // Level 값 설정
        GetText((int)Texts.StoreTitleText).SetLocalizedText("StoreInfo");
        
        GetText((int)Texts.StoreRentalCostTextTitle).SetLocalizedText("Rent");
        GetText((int)Texts.StoreRentalCostTextValue).text = _stroeData.RentalCost.ToString();
        GetText((int)Texts.StoreSizeTextTitle).SetLocalizedText("StoreSize");
        GetText((int)Texts.StoreSizeTextValue).text = _stroeData.Size.ToString();
        GetText((int)Texts.MaxCustomersTextTitle).SetLocalizedText("MaxCustomers");
        GetText((int)Texts.MaxCustomersTextValue).text = _stroeData.MaxCustomers.ToString();
        GetText((int)Texts.HallStaffTextTitle).SetLocalizedText("HallStaff");
        GetText((int)Texts.HallStaffTextValue).text = _hallStaffCount.ToString();
        GetText((int)Texts.KitchenStaffTextTitle).SetLocalizedText("KitchenStaff");
        GetText((int)Texts.KitchenStaffTextValue).text = _kitchenStaffCount.ToString();
        GetText((int)Texts.StoreInfoOKButtonText).SetLocalizedText("CONFIRM");
        
    }
}
