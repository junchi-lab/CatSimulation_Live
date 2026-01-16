using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_BottomPanel : UI_UGUI
{
    enum GameObjects
    {
        BottomOptionPanel
    }
     
    enum Buttons
    {
        // Bottom Menu
        StoreBtn,
        StaffBtn,
        MenuBtn,
        IngredientBtn,
    }
     
    enum Texts
    {
        // Bottom Menu
        StoreBtnText,
        StaffBtnText,
        MenuBtnText,
        IngredientBtnText,
    }
    
    // 각 메뉴별 옵션 버튼 정보
    private class OptionButtonInfo
    {
        public string Name;
        public System.Action OnClick;
        
        public OptionButtonInfo(string name, System.Action onClick)
        {
            Name = name;
            OnClick = onClick;
        }
    }
    
    private Dictionary<Buttons, List<OptionButtonInfo>> _menuOptions;
    private List<GameObject> _dynamicButtons = new List<GameObject>();
    private Buttons? _currentMenu = null; // 초기값을 null로 설정

    
    protected override void Awake()
    {
        base.Awake();

        
        
        BindObjects(typeof(GameObjects));
        BindButtons(typeof(Buttons));
        BindTexts(typeof(Texts));
        
        // 각 메뉴별 옵션 버튼 설정
        InitMenuOptions();
        
        // 메인 메뉴 버튼 이벤트 등록
        GetButton((int)Buttons.StoreBtn).onClick.AddListener(() => OnClickMainMenuBtn(Buttons.StoreBtn));
        GetButton((int)Buttons.StaffBtn).onClick.AddListener(() => OnClickMainMenuBtn(Buttons.StaffBtn));
        GetButton((int)Buttons.MenuBtn).onClick.AddListener(() => OnClickMainMenuBtn(Buttons.MenuBtn));
        GetButton((int)Buttons.IngredientBtn).onClick.AddListener(() => OnClickMainMenuBtn(Buttons.IngredientBtn));
    }
    
    protected override void Start()
    {
        base.Start();
        
        // BottomOptionPanel 기본 상태는 숨김
        GameObject panel = GetObject((int)GameObjects.BottomOptionPanel);
        panel.SetActive(false);
    }
    
    void InitMenuOptions()
    {
        _menuOptions = new Dictionary<Buttons, List<OptionButtonInfo>>();
        
        // Store 메뉴 옵션
        _menuOptions[Buttons.StoreBtn] = new List<OptionButtonInfo>
        {
            new OptionButtonInfo("StoreInfo", OnClickStoreInfo),
            new OptionButtonInfo("Promotion", OnClickPromotion)
        };
        
        // Staff 메뉴 옵션
        _menuOptions[Buttons.StaffBtn] = new List<OptionButtonInfo>
        {
            new OptionButtonInfo("HireStaff", OnClickHireStaff),
            new OptionButtonInfo("ManageStaff", OnClickManageStaff),
            // new OptionButtonInfo("@StaffSalary", OnClickStaffSalary)
        };
        
        // Menu 메뉴 옵션
        _menuOptions[Buttons.MenuBtn] = new List<OptionButtonInfo>
        {
            new OptionButtonInfo("AddFood", OnClickAddFood),
            new OptionButtonInfo("ManageFood", OnClickManageFood)
        };
        
        // Ingredient 메뉴 옵션
        _menuOptions[Buttons.IngredientBtn] = new List<OptionButtonInfo>
        {
            new OptionButtonInfo("OrderIngredient", OnClickOrderIngredient),
            new OptionButtonInfo("ManageIngredient", OnClickManageIngredient),
            
        };
    }
    
    void OnClickMainMenuBtn(Buttons menuType)
    {
        // BottomOptionPanel에 Horizontal Layout Group 설정
        GameObject panel = GetObject((int)GameObjects.BottomOptionPanel);
        HorizontalLayoutGroup layoutGroup = panel.GetComponent<HorizontalLayoutGroup>();
        if (layoutGroup == null)
        {
            layoutGroup = panel.AddComponent<HorizontalLayoutGroup>();
        }
        layoutGroup.spacing = 10;
        layoutGroup.childAlignment = TextAnchor.MiddleCenter;
        layoutGroup.childControlWidth = false;
        layoutGroup.childControlHeight = false;
        layoutGroup.childForceExpandWidth = false;
        layoutGroup.childForceExpandHeight = true;
        
        // 같은 버튼을 다시 클릭하면 패널 토글
        if (_currentMenu == menuType && panel.activeSelf)
        {
            panel.SetActive(false);
            _currentMenu = null;
            return;
        }
        
        // 다른 메뉴 선택 시
        _currentMenu = menuType;
        
        // 기존 동적 버튼들 삭제
        ClearDynamicButtons();
        
        // 선택된 메뉴에 해당하는 버튼들 생성
        CreateDynamicButtons(menuType);
        
        // 패널 표시
        panel.SetActive(true);
    }
    
    void ClearDynamicButtons()
    {
        foreach (var btn in _dynamicButtons)
        {
            Destroy(btn);
        }
        _dynamicButtons.Clear();
    }
    
    void CreateDynamicButtons(Buttons menuType)
    {
        if (!_menuOptions.ContainsKey(menuType))
            return;
        
        GameObject panel = GetObject((int)GameObjects.BottomOptionPanel);
        List<OptionButtonInfo> options = _menuOptions[menuType];
        

        foreach (var option in options)
        {
            // 버튼 GameObject 생성
            GameObject buttonObj = new GameObject(option.Name, typeof(RectTransform));
            buttonObj.transform.SetParent(panel.transform, false);
            
            // RectTransform 크기 설정
            RectTransform buttonRect = buttonObj.GetComponent<RectTransform>();
            buttonRect.sizeDelta = new Vector2(200, 140);
            
            // Image 컴포넌트 추가 (Button이 필요로 함)
            Image img = buttonObj.AddComponent<Image>();
            
            // UISprite 사용 (Resources에서 로드)
            Sprite uiSprite = ResourceManager.Instance.Get<Sprite>("UISprite");
            if (uiSprite != null)
            {
                img.sprite = uiSprite;
                img.type = Image.Type.Sliced;
            }
            img.color = Color.white;
            
            // Outline 컴포넌트 추가 (회색 테두리)
            Outline outline = buttonObj.AddComponent<Outline>();
            outline.effectColor = new Color(0.5f, 0.5f, 0.5f, 1f); // 회색
            outline.effectDistance = new Vector2(1, -1);
            outline.useGraphicAlpha = false;
            
            // Button 컴포넌트 추가
            Button btn = buttonObj.AddComponent<Button>();
            
            // Button Transition 설정 (클릭 시 시각적 피드백)
            btn.targetGraphic = img;
            btn.transition = Selectable.Transition.ColorTint;
            ColorBlock colorBlock = btn.colors;
            colorBlock.normalColor = new Color(0.9f, 0.9f, 0.9f, 1f);      // 기본 색상 (밝은 회색)
            colorBlock.highlightedColor = new Color(0.95f, 0.95f, 0.95f, 1f); // 마우스 오버
            colorBlock.pressedColor = new Color(0.7f, 0.7f, 0.7f, 1f);     // 클릭 시 (어두운 회색)
            colorBlock.selectedColor = new Color(0.9f, 0.9f, 0.9f, 1f);    // 선택 시
            colorBlock.disabledColor = new Color(0.6f, 0.6f, 0.6f, 0.5f);  // 비활성화
            colorBlock.colorMultiplier = 1f;
            colorBlock.fadeDuration = 0.1f;
            btn.colors = colorBlock;
            
            GameObject textObj = new GameObject("@Text", typeof(RectTransform));
            
            // TextMeshProUGUI 추가
            textObj.transform.SetParent(buttonObj.transform, false);
            TMP_Text text = textObj.AddComponent<TextMeshProUGUI>();
            text.fontSize = 40;
            text.alignment = TextAlignmentOptions.Center;
            text.color = Color.black;
            text.SetLocalizedText(option.Name);

            // TextMeshPro는 RectTransform을 Full로 설정
            RectTransform textRect = textObj.GetComponent<RectTransform>();
            textRect.anchorMin = Vector2.zero;
            textRect.anchorMax = Vector2.one;
            textRect.offsetMin = Vector2.zero;
            textRect.offsetMax = Vector2.zero;
            
            // 버튼 클릭 이벤트 등록
            btn.onClick.AddListener(() => option.OnClick?.Invoke());
            
            _dynamicButtons.Add(buttonObj);
        }
        
        Debug.Log($"{menuType} 메뉴에 {options.Count}개의 버튼 생성됨");
    }
    
    // 각 옵션 버튼 클릭 핸들러들
    void OnClickStoreInfo() 
    { 
        Debug.Log("가게클릭");
        UIManager.Instance.ShowPopupUI<UI_StoreInfoPopup>();
    }
    void OnClickPromotion() { Debug.Log("홍보 클릭"); }
    void OnClickHireStaff() 
    { 
        Debug.Log("직원 고용 클릭");
        UIManager.Instance.ShowPopupUI<UI_StaffPopup>();
    }
    void OnClickManageStaff() { Debug.Log("직원 관리 클릭"); }
    void OnClickStaffSalary() { Debug.Log("급여 설정 클릭"); }

    void OnClickAddFood()
    {
        Debug.Log("음식 추가 클릭");
        UIManager.Instance.ShowPopupUI<UI_FoodPopup>();
    }
    void OnClickManageFood() { Debug.Log("음식 편집 클릭"); }
    void OnClickOrderIngredient() { Debug.Log("재료 구매 클릭"); }
    void OnClickManageIngredient() { Debug.Log("재료 재고 클릭"); }
    

    public override void RefreshUI()
    {
        base.RefreshUI();
        
        GetText((int)Texts.StoreBtnText).SetLocalizedText("Store");
        GetText((int)Texts.StaffBtnText).SetLocalizedText("Staff");
        GetText((int)Texts.MenuBtnText).SetLocalizedText("Food");
        GetText((int)Texts.IngredientBtnText).SetLocalizedText("Ingredient");
    }
}
