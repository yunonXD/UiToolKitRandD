using System.Collections.Generic;
using UnityEngine.UIElements;
using UnityEngine;
using System.Linq;

public class CustomDataGridView : VisualElement {
    private readonly VisualElement headerRow;
    private readonly ScrollView itemScrollView;
    private List<float> columnWidths = new();
    private readonly List<VisualElement> rows = new();
    private VisualElement selectedRow;
    private const int RowHeight = 28;

    private int tabIndexOffset = 0;

    public CustomDataGridView() {
        focusable = true;
        style.flexDirection = FlexDirection.Column;
        style.flexGrow = 1;
        style.flexShrink = 1;
        style.width = Length.Percent(100);
        style.height = Length.Percent(100);
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
            focusable = true,
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

            case KeyCode.Tab: {
                evt.StopImmediatePropagation();
                evt.PreventDefault();

                // 루트 가져오기
                VisualElement root = this;
                while (root.hierarchy.parent != null) {
                    root = root.hierarchy.parent;
                }

                // shift 키 여부
                bool shift = evt.shiftKey;

                // 포커서블 요소 리스트
                List<VisualElement> focusables = root.Query<VisualElement>().ToList()
                    .Where(e => e.focusable && e.tabIndex >= 0)
                    .ToList();

                focusables.Sort((a, b) => a.tabIndex.CompareTo(b.tabIndex));
                // 현재 인덱스 찾기
                int current = focusables.IndexOf(this);

                if (current >= 0) {
                    int next = shift
                        ? (current - 1 + focusables.Count) % focusables.Count
                        : (current + 1) % focusables.Count;

                    focusables[next].Focus();
                }
                break;
            }
        }
    }

    public void UpdateTabIndexes(int offset = 0) {
        tabIndexOffset = offset;

        int index = tabIndexOffset;

        foreach (var row in rows) {
            foreach (var child in row.Children()) {
                if (child.focusable)
                    child.tabIndex = index++;
            }
        }
    }


    private int CountFocusable(VisualElement root) {
        int count = 0;
        if (root.focusable) count++;
        foreach (var child in root.Children()) {
            count += CountFocusable(child);
        }
        return count;
    }

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
