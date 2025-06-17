using UnityEngine.UIElements;
using System;


/// @brief 화면 인터페이스 컨트롤러
public interface IScreenController{
    
    //Instantiate한 화면의 루트
    //다른 화면으로 전환할 수 있는 콜백
    void Initialize(VisualElement root, Action<string> onNavigate);

    // 버튼 클릭 처리용
    void OnButtonPressed(int buttonIndex);  
}