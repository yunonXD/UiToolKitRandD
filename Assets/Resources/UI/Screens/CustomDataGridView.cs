using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;

public class CustomDataGridView : VisualElement {
    private VisualElement headerRow;
    private ScrollView itemScrollView;
    private List<float> columnWidths = new();
    private List<VisualElement> rows = new();
    private VisualElement selectedRow;
    private const int RowHeight = 28;
    
    private int tabIndexOffset = 0;

    public CustomDataGridView() {
        style.flexDirection = FlexDirection.Column;
        style.flexGrow = 1;
        style.flexShrink = 1;
        style.width = Length.Percent(100);
        style.height = Length.Percent(100);
        style.justifyContent = Justify.FlexStart;

        RegisterCallback<KeyDownEvent>(OnKeyDownEvent);
        RegisterCallback<FocusInEvent>(_ => HighlightSelectedRow(true));
        RegisterCallback<FocusOutEvent>(_ => HighlightSelectedRow(false));


        style.backgroundColor = new Color(1f, 0.5f, 0.5f); 

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
                height = Length.Percent(100),
                flexDirection = FlexDirection.Column,
                overflow = Overflow.Visible,
                justifyContent = Justify.FlexStart
            }
        };
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

        row.RegisterCallback<ClickEvent>(_ => {
            this.Focus();  // 포커스를 이 컨트롤로 이동
            SelectRow(row);
        });

        for (int i = 0; i < cellTexts.Count; i++) {
            var cell = new Label(cellTexts[i]) {
                focusable = true, // focusable 활성화
                tabIndex = tabIndexOffset + rows.Count * cellTexts.Count + i,
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

        if (selectedRow == null)
            SelectRow(row);
    }

    private void SelectRow(VisualElement row) {
        if (selectedRow != null)
            selectedRow.style.backgroundColor = StyleKeyword.Null;

        selectedRow = row;

        if (this.focusController?.focusedElement == this)
            selectedRow.style.backgroundColor = new Color(0.2f, 0.4f, 0.8f, 0.3f);
    }

    private void HighlightSelectedRow(bool highlight) {
        if (selectedRow == null) return;
        selectedRow.style.backgroundColor = highlight
            ? new Color(0.2f, 0.4f, 0.8f, 0.3f)
            : StyleKeyword.Null;
    }
    
    private void OnKeyDownEvent(KeyDownEvent evt) {
        if (rows.Count == 0 ) return;
        
        int currentIndex = selectedRow != null ? rows.IndexOf(selectedRow) : -1;
    
        switch (evt.keyCode) {
            case KeyCode.UpArrow:
                if (currentIndex > 0) {
                    SelectRow(rows[currentIndex - 1]);
                    evt.StopImmediatePropagation();
                }
                break;
    
            case KeyCode.DownArrow:
                if (currentIndex < rows.Count - 1) {
                    SelectRow(rows[currentIndex + 1]);
                    evt.StopImmediatePropagation();
                }
                break;
        }
    }
    
    /// 자신의 부모에서 앞에 있는 모든 focusable 컨트롤 수를 기준으로 tabIndex 오프셋을 계산
    public void UpdateTabIndexes() {
        tabIndexOffset = 0;

        if (hierarchy.parent == null) return;

        var parent = this.hierarchy.parent;

        foreach (var child in parent.Children()) {
            if (child == this) { break; }

            if (child.focusable || HasFocusableDescendants(child)) {
                tabIndexOffset += CountFocusable(child);
            }
        }
    }
    
    /// 자식까지 포함해서 focusable 요소 수를 계산
    private int CountFocusable(VisualElement root) {
        int count = 0;

        if (root.focusable) count++;

        foreach (var child in root.Children()) {
            count += CountFocusable(child);
        }

        return count;
    }
    
    /// 해당 요소 및 자식 중에 focusable이 있는지 확인
    private bool HasFocusableDescendants(VisualElement root) {
        if (root.focusable) return true;

        foreach (var child in root.Children()) {
            if (HasFocusableDescendants(child)) return true;
        }

        return false;
    }

    public void ClearRows() {
        itemScrollView.Clear();
        rows.Clear();
        selectedRow = null;
    }
}