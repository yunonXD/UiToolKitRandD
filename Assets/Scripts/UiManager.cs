using System.Collections.Generic;
using UnityEngine.UIElements;
using System.Collections;
using UnityEngine;
using System;

public class UiManager : MonoBehaviour{

    private VisualElement root;
    private VisualElement divMain, divTop, divButton;
    private Label titleLabel;
	private Label timeLabel;
	private Label modeLabel;

    [SerializeField] private UIDocument UIDoc;

    private Dictionary<string, VisualTreeAsset> screenTemplates;
    private Dictionary<string, string[]> screenButtonLabels; // 화면별 버튼 이름
    private Dictionary<string, IScreenController> screenControllers;


    void Start(){

        root = UIDoc.rootVisualElement;
        var innerMain = root.Q<VisualElement>("Div_Main");
        divMain = innerMain.Q<VisualElement>("Main");
        divTop = root.Q<VisualElement>("Div_Top");
        divButton = root.Q<VisualElement>("Div_Button");
        titleLabel = divTop.Q<Label>("Title");
		timeLabel = divTop.Q<Label>("Time");
		modeLabel = divTop.Q<Label>("Mode");

        LoadScreens(); // UXML 등록
        SwitchToScreen("MainMenu");
    }

    private void Update()
    {
        timeLabel.text =  DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
    }

    void LoadScreens(){

        screenTemplates = new Dictionary<string, VisualTreeAsset> {
            { "MainMenu", Resources.Load<VisualTreeAsset>("UI/Screens/MainMenu") },
            { "Settings", Resources.Load<VisualTreeAsset>("UI/Screens/Settings") },
            { "Gameplay", Resources.Load<VisualTreeAsset>("UI/Screens/Gameplay") },
            { "MapControlPlay", Resources.Load<VisualTreeAsset>("UI/Screens/MapControlPlay") },
        };

        screenControllers = new Dictionary<string, IScreenController> {
            { "MainMenu", new MainMenuController() },
            { "Settings", new SettingsController() },
            { "Gameplay", new GameplayController() },
            { "MapControlPlay", new MapControlPlayContoller()},
        };

        screenButtonLabels = new Dictionary<string, string[]> {
            { "MainMenu", new[] { "Start","Map" ,"Settings", "Exit" } },
            { "Settings", new[] { "Audio", "Graphics", "Back" } },
            { "Gameplay", new[] { "Back"} },
            { "MapControlPlay", new[] { "Back"} },
        };
    }
    
    IScreenController currentController;
    private readonly Dictionary<Button, Action> buttonCallbacks = new(); // 버튼 핸들러를 Dictionary로 추적해서 덮어쓰기 전에 제거
    void SwitchToScreen(string screenName){

        titleLabel.text = screenName;
        divMain.Clear();
        
        if (screenTemplates.TryGetValue(screenName, out var template)){
            var instance = template.Instantiate();
            instance.style.width = new Length(100, LengthUnit.Percent);
            divMain.Add(instance);

            if (screenControllers.TryGetValue(screenName, out var controller)){
                currentController = controller;
                controller.Initialize(instance, SwitchToScreen);
            }
        }

        string[] labels = screenButtonLabels.ContainsKey(screenName) ? screenButtonLabels[screenName] : Array.Empty<string>();

        for (int i = 1; i <= 8; i++){
            var button = divButton.Q<Button>($"Button_{i}");
            button.style.display = DisplayStyle.Flex;
            button.style.minWidth = 80;
            button.text = i <= labels.Length ? labels[i - 1] : "";

            // 기존 콜백 제거
            if (buttonCallbacks.TryGetValue(button, out var prevCallback))
                button.clicked -= prevCallback;

            // 새 콜백 등록s
            int index = i;
            Action callback = () => currentController?.OnButtonPressed(index);
            button.clicked += callback;

            // 추적용 콜백 저장
            buttonCallbacks[button] = callback;
        }
    }

    // UiManager 가 현재 화면의 버튼을 온버튼으로 실행할 수 있게 도와주는 메저드
    public void TriggerButtonByIndex(int index){

        currentController?.OnButtonPressed(index);
    }
}