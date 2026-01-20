using System.Collections.Generic;
using UnityEngine;

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
    
    protected override void Awake()
    {
        BindObjects(typeof(GameObjects));
        BindButtons(typeof(Buttons));
        BindTexts(typeof(Texts));
        
        _content = GetObject((int)GameObjects.Content).transform;
        
        
    }

    public override void RefreshUI()
    {
        base.RefreshUI();
    }
}
