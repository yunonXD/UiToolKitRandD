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

    public CustomDataGridView() {
        style.flexDirection = FlexDirection.Column;
        style.flexGrow = 1;
        style.flexShrink = 1;
        style.width = Length.Percent(100);
        style.height = Length.Percent(100);
        style.justifyContent = Justify.FlexStart;
        RegisterCallback<KeyDownEvent>(OnKeyDownEvent);
        focusable = true;
        
        style.backgroundColor = new Color(1f, 0.5f, 0.5f); // 예시 배경색

        // 헤더 (고정)
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

        // 아이템 스크롤 영역
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
            style = {
                flexDirection = FlexDirection.Row,
                alignItems = Align.Center,
                height = RowHeight,
                width = new Length(100, LengthUnit.Percent),  // ✅ 가로 100%로
                marginBottom = 2
            }
        };

        row.RegisterCallback<ClickEvent>(_ => SelectRow(row));

        for (int i = 0; i < cellTexts.Count; i++) {
            var cell = new Label(cellTexts[i]) {
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
    }

    private void SelectRow(VisualElement row) {
        if (selectedRow != null)
            selectedRow.style.backgroundColor = StyleKeyword.Null;

        selectedRow = row;
        selectedRow.style.backgroundColor = new Color(0.2f, 0.4f, 0.8f, 0.3f);
    }

    public void ClearRows() {
        itemScrollView.Clear();
        rows.Clear();
        selectedRow = null;
    }
    
    private void OnKeyDownEvent(KeyDownEvent evt) {
        if (rows.Count == 0)
            return;

        int currentIndex = selectedRow != null ? rows.IndexOf(selectedRow) : -1;

        switch (evt.keyCode) {
            case KeyCode.DownArrow:
                if (currentIndex < rows.Count - 1)
                    SelectRow(rows[currentIndex + 1]);
                evt.StopPropagation();
                break;

            case KeyCode.UpArrow:
                if (currentIndex > 0)
                    SelectRow(rows[currentIndex - 1]);
                evt.StopPropagation();
                break;
        }
    }

}
