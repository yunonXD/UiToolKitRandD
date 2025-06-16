using System.Collections.Generic;
using UnityEngine.UIElements;
using System.Collections;
using UnityEngine;
using System;

public class MainMenuController : IScreenController
{
    private Action<string> navigate;
    public void Initialize(VisualElement root, Action<string> onNavigate)
    {
        this.navigate = onNavigate;
        // 필요시 화면 초기화
    }

    public void OnButtonPressed(int buttonIndex)
    {
        switch (buttonIndex)
        {
            case 1: navigate("Gameplay"); break;
            case 2: navigate("Settings"); break;
            default: break;
        }
    }
}
