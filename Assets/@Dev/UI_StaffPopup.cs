using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UI_StaffPopup : UI_UGUI, IUI_Popup
{
    enum GameObjects
    {
        Content,
    }
    
    enum Buttons
    {
        StaffOKButton,
        StaffRefreshButton
    }
    
    enum Texts
    {
        StaffTitleText,
        StaffOKButtonText,
        StaffRefreshButtonText,
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
        
        // Close 버튼 이벤트 등록
        GetButton((int)Buttons.StaffOKButton).onClick.AddListener(() =>
        {
            UIManager.Instance.ClosePopupUI();
        });
        
        // Refresh 버튼 이벤트 등록
        GetButton((int)Buttons.StaffRefreshButton).onClick.AddListener(() =>
        {
            OnClickRefresh();
        });
    }
    
    protected override void OnEnable()
    {
        base.OnEnable();
        
        // 팝업이 활성화될 때마다 새로운 랜덤 스태프 로드
        LoadRandomStaff();
    }
    
    private void OnClickRefresh()
    {
        Debug.Log("Staff Refresh Button Clicked");
        LoadRandomStaff();
        RefreshUI();
    }

    private void LoadRandomStaff()
    {
        // DataManager에서 스태프 데이터 가져오기
        if (DataManager.Instance == null)
        {
            Debug.LogWarning("DataManager is not initialized!");
            return;
        }

        var staffDataDic = DataManager.Instance.StaffDict;
        if (staffDataDic == null || staffDataDic.Count == 0)
        {
            Debug.LogWarning("No staff data available!");
            return;
        }

        // 고용되지 않은 스태프만 필터링
        var availableStaff = staffDataDic.Values
            .Where(staff => !GameManager.Instance.IsStaffHired(staff.StaffID))
            .ToList();

        // if (availableStaff.Count == 0)
        // {
        //     Debug.LogWarning("No available staff to hire!");
        //     return;
        // }

        // 랜덤하게 5명 선택 (또는 사용 가능한 스태프가 5명 미만이면 전부 선택)
        int count = Mathf.Min(5, availableStaff.Count);
        var randomStaff = availableStaff.OrderBy(x => Random.value).Take(count).ToList();

        SetInfo(randomStaff);
    }

    public override void RefreshUI()
    {
        base.RefreshUI();
        
        GetText((int)Texts.StaffTitleText).SetLocalizedText("HireStaff");
        GetText((int)Texts.StaffOKButtonText).SetLocalizedText("CONFIRM");
        GetText((int)Texts.StaffRefreshButtonText).SetLocalizedText("REFRESH");
    }
    
    public void SetInfo(List<StaffData> randomStaff)
    {
        _content.DestroyChildren();
        _items.Clear();

        // UI_Add_Popup_item 생성

        foreach (var staffData in randomStaff)
        {
            
            UI_AddPopupItem item = UIManager.Instance.ShowUI<UI_AddPopupItem>("UI_AddPopupItem");
            item.transform.SetParent(_content);
            item.SetInfo(staffData);
            _items.Add(item);
            
        }
    }
}
