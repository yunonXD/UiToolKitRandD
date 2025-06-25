using System.Collections.Generic;
using UnityEngine.UIElements;
using UnityEngine;
using System.Linq;

public class CustomDataGridView : VisualElement {
    
    private readonly List<VisualElement> rows = new();
    private readonly ScrollView itemScrollView;
    private readonly VisualElement headerRow;
    private List<float> columnWidths = new();
    private VisualElement selectedRow;
    private const int RowHeight = 28;

    public CustomDataGridView() {
        focusable = true;
        style.flexDirection = FlexDirection.Column;
        style.flexGrow = 1;
        style.flexShrink = 1;
        style.justifyContent = Justify.FlexStart;

        RegisterCallback<KeyDownEvent>(OnKeyDownEvent);
        RegisterCallback<FocusInEvent>(evt => {
            HighlightSelectedRow(true);
            if (selectedRow == null && rows.Count > 0) SelectRow(rows[0]);
        });
        RegisterCallback<FocusOutEvent>(_ => HighlightSelectedRow(false));

        headerRow = new VisualElement {
            style = {
                flexDirection = FlexDirection.Row,
                flexGrow = 0,
                flexShrink = 0,
                height = RowHeight,
                width = Length.Percent(100),
                justifyContent = Justify.FlexStart
            }
        };
        Add(headerRow);

        itemScrollView = new ScrollView(ScrollViewMode.Vertical) {
            style = {
                flexGrow = 1,
                flexShrink = 1,
                width = Length.Percent(100),
                flexDirection = FlexDirection.Column,
            }
        };
        //itemScrollView.verticalScrollerVisibility = ScrollerVisibility.Hidden;
        itemScrollView.verticalScroller.focusable = false;
        itemScrollView.verticalScroller.slider.focusable = false;
        itemScrollView.horizontalScroller.focusable = false;
        Add(itemScrollView);
    }

    public void SetHeaders(List<string> headers, List<float> widths = null) {
        headerRow.Clear();
        columnWidths = widths ?? new();

        for (int i = 0; i < headers.Count; i++) {
            var label = new Label(headers[i]) {
                style = {
                    height = RowHeight,
                    unityTextAlign = TextAnchor.MiddleCenter,
                    unityFontStyleAndWeight = FontStyle.Bold,
                    borderBottomWidth = 1,
                    borderBottomColor = Color.black,
                    paddingLeft = 4,
                    paddingRight = 4,
                    overflow = Overflow.Hidden,
                    whiteSpace = WhiteSpace.NoWrap,
                    unityTextOverflowPosition = TextOverflowPosition.End,
                    alignSelf = Align.Center
                }
            };

            if (columnWidths.Count > i) {
                label.style.width = columnWidths[i];
                label.style.flexShrink = 0;
                label.style.flexGrow = 0;
            } else {
                label.style.flexGrow = 1;
            }

            headerRow.Add(label);
        }
    }

    public void AddRow(List<string> cellTexts) {
        var row = new VisualElement {
            focusable = false,
            style = {
                flexDirection = FlexDirection.Row,
                alignItems = Align.Center,
                height = RowHeight,
                width = new Length(100, LengthUnit.Percent),
                marginBottom = 2
            }
        };

        row.RegisterCallback<FocusInEvent>(_ => SelectRow(row));

        for (int i = 0; i < cellTexts.Count; i++) {
            var cell = new Label(cellTexts[i]) {
                focusable = false,
                style = {
                    height = RowHeight,
                    unityTextAlign = TextAnchor.MiddleCenter,
                    paddingLeft = 4,
                    paddingRight = 4,
                    overflow = Overflow.Hidden,
                    whiteSpace = WhiteSpace.NoWrap,
                    unityTextOverflowPosition = TextOverflowPosition.End,
                    borderBottomWidth = 1,
                    borderBottomColor = new Color(0.8f, 0.8f, 0.8f),
                    alignSelf = Align.Center
                }
            };

            if (columnWidths.Count > i) {
                cell.style.width = columnWidths[i];
                cell.style.flexGrow = 0;
                cell.style.flexShrink = 0;
            } else {
                cell.style.flexGrow = 1;
            }

            row.Add(cell);
        }

        itemScrollView.Add(row);
        rows.Add(row);
        if (selectedRow == null) SelectRow(row);
    }

    private void SelectRow(VisualElement row) {
        if (selectedRow != null)
            selectedRow.style.backgroundColor = StyleKeyword.Null;

        selectedRow = row;

        var focusedVisual = this.focusController?.focusedElement as VisualElement;
        if (focusedVisual != null && (this == focusedVisual || row.Contains(focusedVisual))) {
            selectedRow.style.backgroundColor = new Color(0.2f, 0.4f, 0.8f, 0.3f);
        }
        itemScrollView.ScrollTo(row);
    }

    private void HighlightSelectedRow(bool highlight) {
        if (selectedRow == null) return;
        selectedRow.style.backgroundColor = highlight
            ? new Color(0.2f, 0.4f, 0.8f, 0.3f)
            : StyleKeyword.Null;
    }

    private void OnKeyDownEvent(KeyDownEvent evt) {
        if (rows.Count == 0) return;

        int currentIndex = selectedRow != null ? rows.IndexOf(selectedRow) : -1;

        switch (evt.keyCode) {
            case KeyCode.UpArrow:
                if (currentIndex > 0) {
                    SelectRow(rows[currentIndex - 1]);
                    evt.StopImmediatePropagation();
                    evt.PreventDefault();
                }
                break;

            case KeyCode.DownArrow:
                if (currentIndex < rows.Count - 1) {
                    SelectRow(rows[currentIndex + 1]);
                    evt.StopImmediatePropagation();
                    evt.PreventDefault();
                }
                break;

            case KeyCode.Tab:
                MoveFocusToNext(evt.shiftKey);
                evt.StopImmediatePropagation();
                evt.PreventDefault();
                break;
        }
    }
    
    private void MoveFocusToNext(bool shift) {
        // 루트 찾기
        VisualElement root = this;
        while (root.hierarchy.parent != null)
            root = root.hierarchy.parent;

        var focusables  = root.Query<VisualElement>().ToList()
            .Where(e => e.focusable && e.tabIndex >= 0 && e != this)
            .OrderBy(e => e.tabIndex)
            .ToList();
        
        // 현재 포커스 요소 가져오기
        var currentFocused = root.panel?.focusController?.focusedElement as VisualElement;
        if (currentFocused == null) return;

        int currentIndex = focusables.IndexOf(currentFocused);
        if (currentIndex < 0 || focusables.Count == 0) return;

        int next = shift
            ? (currentIndex - 1 + focusables.Count) % focusables.Count
            : (currentIndex + 1) % focusables.Count;
        
        // Debug.Log($"[Tab] 현재: {currentFocused.name}, 이동 대상: {focusables[next].name}");
        // focusables.ForEach(e => Debug.Log($"{e.name} - {e.tabIndex}"));
        focusables[next].Focus();
    }
    
    public void UpdateTabIndexes(int offset = 0) {
        this.tabIndex = offset;
    }

    public void ClearRows() {
        itemScrollView.Clear();
        rows.Clear();
        selectedRow = null;
    }
}
