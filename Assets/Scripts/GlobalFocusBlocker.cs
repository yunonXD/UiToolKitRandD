using UnityEngine.UIElements;
using UnityEngine;
using System.Linq;

/// UI Toolkit 전체에서 방향키 위/아래로 포커스 이동을 막고, 포커싱된 요소를 강조하는 전역 차단 유틸리티
public static class GlobalFocusBlocker {

    /// 포커스 이동 차단 이벤트 + 강조 효과를 루트에 등록
    public static void ApplyTo(VisualElement root) {
        if (root == null) {
            Debug.LogWarning("[GlobalFocusBlocker] 루트가 null.");
            return;
        }

        // 방향키 포커스 이동 차단
        root.RegisterCallback<NavigationMoveEvent>(evt => {
            if (evt.direction == NavigationMoveEvent.Direction.Up ||
                evt.direction == NavigationMoveEvent.Direction.Down) {
                evt.StopImmediatePropagation();
                evt.PreventDefault();
                Debug.Log($"[GlobalFocusBlocker] 방향키 포커스 이동 차단: {evt.direction}");
            }
        }, TrickleDown.TrickleDown);

        // TextField 관련 처리
        var textFields = root.Query<TextField>().ToList();
        foreach (var tf in textFields) {
            // TextField 자체에 키 이벤트 등록
            RegisterTabHandler(tf, tf);

            // 내부 입력 필드에도 키 이벤트 등록 (unity-text-input)
            var textInput = tf.Q("unity-text-input");
            if (textInput != null) {
                RegisterTabHandler(tf, textInput);
            }
        }

        // 모든 포커서블 요소 강조 처리
        var focusables = root.Query<VisualElement>().ToList().Where(e => e.focusable);
        foreach (var element in focusables) {
            element.RegisterCallback<FocusInEvent>(evt => {
                element.style.borderBottomWidth = 2;
                element.style.borderTopWidth = 2;
                element.style.borderLeftWidth = 2;
                element.style.borderRightWidth = 2;
                element.style.borderBottomColor = Color.yellow;
                element.style.borderTopColor = Color.yellow;
                element.style.borderLeftColor = Color.yellow;
                element.style.borderRightColor = Color.yellow;
            });

            element.RegisterCallback<FocusOutEvent>(evt => {
                element.style.borderBottomWidth = 0;
                element.style.borderTopWidth = 0;
                element.style.borderLeftWidth = 0;
                element.style.borderRightWidth = 0;
            });
        }
    }

    /// Tab / Shift+Tab 키 처리를 등록
    private static void RegisterTabHandler(TextField owner, VisualElement target) {
        target.RegisterCallback<KeyDownEvent>(evt => {
            if (evt.keyCode == KeyCode.Tab) {
                evt.StopImmediatePropagation();
                evt.PreventDefault();

                bool shift = evt.shiftKey;
                var parentRoot = GetRoot(owner);

                var focusables = parentRoot.Query<VisualElement>().ToList()
                    .Where(e => e.focusable && e.tabIndex >= 0)
                    .OrderBy(e => e.tabIndex)
                    .ToList();

                var current = focusables.IndexOf(owner);
                if (current >= 0 && focusables.Count > 0) {
                    int next = shift
                        ? (current - 1 + focusables.Count) % focusables.Count
                        : (current + 1) % focusables.Count;

                    owner.schedule.Execute(() => {
                        focusables[next].Focus();
                    }).ExecuteLater(1);
                }
            }
        }, TrickleDown.TrickleDown); 

    }

    /// 루트 VisualElement 찾기
    private static VisualElement GetRoot(VisualElement element) {
        var current = element;
        while (current.parent != null) {
            current = current.parent;
        }
        return current;
    }
}
