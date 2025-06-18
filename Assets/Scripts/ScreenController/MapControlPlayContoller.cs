using System.Collections.Generic;
using UnityEngine.UIElements;
using System.Collections;
using UnityEngine;
using System;
using TMPro;

public class MapControlPlayContoller : IScreenController
{
    private Action<string> navigate;
    public void Initialize(VisualElement root, Action<string> onNavigate){
        
        this.navigate = onNavigate;
        
        root.style.flexGrow = 1;
        root.style.height = Length.Percent(100);
        root.style.width = Length.Percent(100);
        
        var baseView = root.Q<VisualElement>("MapViewEle");
        if (baseView != null)
        {
            baseView.Clear();
            var map = new MapView();
            baseView.Add(map);
            map.SetWorldBounds(-1987, 1987, -1987, 1987);
            //map.FocusOnWorldPosition(1987/2, -1987/2);
        }
    }
    
    
    public void OnButtonPressed(int buttonIndex){
        
        switch (buttonIndex)
        {
            case 1: navigate("MainMenu"); break;
            default: break;
        }
    }
}
