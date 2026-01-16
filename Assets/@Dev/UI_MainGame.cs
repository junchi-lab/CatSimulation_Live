using UnityEngine;

public class UI_MainGame : UI_UGUI, IUI_Scene
{
     private UI_BottomPanel _bottomPanel;
     

     protected override void Awake()
     {
          base.Awake();
          
          _bottomPanel = Utils.FindChildComponent<UI_BottomPanel>(gameObject, recursive: true);
     }
     
     public override void RefreshUI()
     {
          _bottomPanel.RefreshUI();
     }
}
