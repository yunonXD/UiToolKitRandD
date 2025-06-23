using System.Collections.Generic;
using UnityEngine.UIElements;
using UnityEngine;
using System;
using UnityEditor;

public class SettingsController : IScreenController {
    
    private Action<string> navigate;
    
    public void Initialize(VisualElement root, Action<string> onNavigate) {
        
        this.navigate = onNavigate;
        
        var dropdown = root.Q<DropdownField>("DropdownField");
        dropdown.choices = new List<string> { "Option A", "Option B", "Option C" };
        dropdown.value = "Option A"; // 초기 선택 값
        
        // 데이터 리스트
        List<string> items = new List<string> { "A", "B", "C", "D", "E", "F", "G", "H", "I" };

        var listview = root.Q<ListView>("MyListView");
        listview.makeItem = () =>
        {
            var container = new VisualElement();
            container.Add(new Label());
            container.Add(new Button(() => Debug.Log("Clicked")) { text = "Click Me" });
            return container;
        };

        listview.bindItem = (element, i) =>
        {
            var label = element.Q<Label>();
            label.text = items[i];
        };
        
        listview.itemsSource = items;
        listview.selectionType = SelectionType.Single;

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
