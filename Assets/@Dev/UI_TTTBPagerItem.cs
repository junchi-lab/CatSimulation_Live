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

    protected override void Awake()
    {
        base.Awake();
        
        BindButtons(typeof(Buttons));
        BindTexts(typeof(Texts));
    }

    public override void RefreshUI()
    {
        base.RefreshUI();
    }
}
