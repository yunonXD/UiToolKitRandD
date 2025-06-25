using System.Collections.Generic;
using UnityEngine.UIElements;
using System.Collections;
using UnityEngine;
using System;

public class GameplayController : IScreenController
{
    private Action<string> navigate;
    public void Initialize(VisualElement root, Action<string> onNavigate){
        
        this.navigate = onNavigate;
        
        root.style.flexGrow = 1;
        root.style.height = Length.Percent(100);
        root.style.width = Length.Percent(100);
        
        GlobalFocusBlocker.ApplyTo(root);
    }
    
    
    public void OnButtonPressed(int buttonIndex){
        
        switch (buttonIndex)
        {
            case 1: navigate("MainMenu"); break;
            default: break;
        }
    }
}
