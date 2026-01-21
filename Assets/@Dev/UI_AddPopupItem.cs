using UnityEngine;
using UnityEngine.UI;

public class UI_AddPopupItem : UI_UGUI
{
    enum Buttons
    {
        AddButton,
    }
    enum Texts
    {
        NameTextTitle,
        // PositionText,
        // SalaryText,
        // HireCostText,
        // StatsText,
        AddButtonText,
    }

    private FoodData _foodItemData;
    private StaffData _staffData;

    protected override void Awake()
    {
        base.Awake();
        
        
        BindButtons(typeof(Buttons));
        BindTexts(typeof(Texts));

        // AddButton 클릭 이벤트 - 메뉴 추가/직원 고용
        GetButton((int)Buttons.AddButton).onClick.AddListener(() =>
        {
            if (_foodItemData != null)
            {
                OnClickAddFood();
            }
            else if (_staffData != null)
            {
                OnClickHire();
            }
        });
        
        // GetButton((int)Buttons.ItemSelectButton).onClick.AddListener(OnClickItem);

    }
    
    /// <summary>
    /// 아이템 클릭 시 UI_PagerPopup 띄우기
    /// </summary>
    private void OnClickItem()
    {
        Debug.Log("UI_AddPopupItem clicked - Opening UI_PagerPopup");
        UIManager.Instance.ShowPopupUI<UI_PagerPopup>("UI_PagerPopup");
    }

    /// <summary>
    /// 음식 추가 버튼 클릭
    /// </summary>
    private void OnClickAddFood()
    {
        if (_foodItemData == null)
            return;
        
        // 이미 추가된 메뉴인지 확인
        if (GameManager.Instance.IsFoodAdded(_foodItemData.FoodID))
        {
            Debug.LogWarning($"Food {_foodItemData.FoodID} is already added!");
            return;
        }
        
        // 메뉴 추가 시도
        if (GameManager.Instance.AddFood(_foodItemData.FoodID))
        {
            RefreshUI();
            // 부모 팝업 새로고침 (추가 버튼 상태 업데이트)
            GetComponentInParent<UI_FoodPopup>()?.RefreshUI();
        }
    }

    /// <summary>
    /// 직원 고용 버튼 클릭
    /// </summary>
    private void OnClickHire()
    {
        if (_staffData == null)
            return;
        
        // 이미 고용된 직원인지 확인
        if (GameManager.Instance.IsStaffHired(_staffData.StaffID))
        {
            Debug.LogWarning($"Staff {_staffData.StaffID} is already hired!");
            return;
        }
        
        // 직원 고용 시도
        if (GameManager.Instance.HireStaff(_staffData.StaffID))
        {
            RefreshUI();
            // 부모 팝업 새로고침 (고용 버튼 상태 업데이트)
            GetComponentInParent<UI_StaffPopup>()?.RefreshUI();
        }
    }

    public void SetInfo(FoodData foodItemData)
    {
        _foodItemData = foodItemData;
        _staffData = null;
    }
    
    public void SetInfo(StaffData staffData)
    {
        _staffData = staffData;
        _foodItemData = null;
    }

    public override void RefreshUI()
    {
        base.RefreshUI();
        
        if (_foodItemData != null)
        {
            RefreshFoodUI();
        }
        else if (_staffData != null)
        {
            RefreshStaffUI();
        }
    }
    
    private void RefreshFoodUI()
    {
        if (_foodItemData == null)
            return;
        
        GetText((int)Texts.NameTextTitle).text = $"{_foodItemData.NameTextID}";
        
        // 이미 추가된 메뉴인지 확인
        bool isAdded = GameManager.Instance.IsFoodAdded(_foodItemData.FoodID);
        GetText((int)Texts.AddButtonText).text = isAdded ? "@ADDED" : _foodItemData.AddCost.ToString();
        GetButton((int)Buttons.AddButton).interactable = !isAdded;
    }
    
    private void RefreshStaffUI()
    {
        if (_staffData == null)
            return;
        
        GetText((int)Texts.NameTextTitle).text = _staffData.NameTextID;
        // GetText((int)Texts.PositionText).text = $"{_staffData.Position} (Type {_staffData.StaffType})";
        // GetText((int)Texts.SalaryText).text = $"Salary: {_staffData.Salary:N0}";
        // GetText((int)Texts.HireCostText).text = $"Hire: {_staffData.HireCost:N0}";
        //
        // // 스탯 정보
        // string stats = $"Speed:{_staffData.WorkSpeed} Eff:{_staffData.Efficiency} " +
        //               $"Sta:{_staffData.Stamina} Skill:{_staffData.Skill} Exp:{_staffData.Experience}";
        // GetText((int)Texts.StatsText).text = stats;
        
        // 이미 고용된 직원인지 확인
        bool isHired = GameManager.Instance.IsStaffHired(_staffData.StaffID);
        GetText((int)Texts.AddButtonText).text = isHired ? "@ADDED" : _staffData.HireCost.ToString();
        GetButton((int)Buttons.AddButton).interactable = !isHired;
    }   
}
