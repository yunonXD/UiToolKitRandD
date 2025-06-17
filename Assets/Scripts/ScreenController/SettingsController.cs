using UnityEngine.UIElements;
using UnityEngine;
using System;

public class SettingsController : IScreenController {

    private Action<string> navigate;
    public void Initialize(VisualElement root, Action<string> onNavigate) {
        
        this.navigate = onNavigate;

    }

    public void OnButtonPressed(int buttonIndex) {
        
        
        switch (buttonIndex)
        {
            case 1: Debug.Log("오디오 세팅"); break;
            case 2: Debug.Log("그래픽 세팅"); break;
            case 3: navigate("MainMenu"); break;
            default: break;
        }
    }
}
