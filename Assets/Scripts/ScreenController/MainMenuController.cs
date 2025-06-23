using System.Collections.Generic;
using UnityEngine.UIElements;
using Mono.Data.Sqlite;
using UnityEngine;
using System.IO;
using System;

public class MainMenuController : IScreenController {
    private Action<string> navigate;

    public void Initialize(VisualElement root, Action<string> onNavigate) {
        this.navigate = onNavigate;
        
        root.style.flexGrow = 1;
        root.style.height = Length.Percent(100);
        root.style.width = Length.Percent(100);
        
        var items1 = root.Q<DropdownField>("Drop1");
        items1.choices = new List<string> { "Option A", "Option B", "Option C" };
        items1.value = "Option A"; // 초기 선택 값
        
        var items2 = root.Q<DropdownField>("Drop2");
        items2.choices = new List<string> { "Option D", "Option E", "Option F" };
        items2.value = "Option D"; // 초기 선택 값
        
        var items3 = root.Q<DropdownField>("Drop3");
        items3.choices = new List<string> { "Option G", "Option H", "Option I" };
        items3.value = "Option G"; // 초기 선택 
        
        // var vElement = root.Q<VisualElement>("GroupBox");
        // if (vElement != null) {
        //     vElement.Clear();
        //     var grid = new CustomDataGridView();
        //     grid.SetHeaders(new List<string> { "ID", "Name", "Level", "Description", "Modified", "Data" },
        //         new List<float> { 200, 150, 80, 150, 160, 100 });
        //     LoadScenariosIntoGrid(grid);
        //
        //     // GroupBox가 배치되기 전에 tabIndex로 몇 개 요소가 있는지 계산
        //     int offset = CountFocusableBefore(root, vElement);
        //
        //     vElement.Add(grid);
        //     grid.UpdateTabIndexes(offset);
        // }

        
        GlobalFocusBlocker.ApplyTo(root);
    }
    
    /// GroupBox 이전까지 모든 focusable 요소를 탐색해 tabIndex offset을 구함
    private int CountFocusableBefore(VisualElement root, VisualElement target) {
        var allElements = new List<VisualElement>();
        CollectElementsInOrder(root, allElements);

        int count = 0;
        foreach (var e in allElements) {
            if (e == target) break;
            if (e.focusable) count++;
        }
        return count;
    }

    /// 트리 순회하면서 실제 배치 순서대로 요소 수집
    private void CollectElementsInOrder(VisualElement root, List<VisualElement> list) {
        list.Add(root);
        foreach (var child in root.Children()) {
            CollectElementsInOrder(child, list);
        }
    }

    private void LoadScenariosIntoGrid(CustomDataGridView grid) {
        string dbPath = Path.Combine(Application.streamingAssetsPath, "knmbt3.db");
        string dbConnStr = "URI=file:" + dbPath;

        using (var conn = new SqliteConnection(dbConnStr)) {
            conn.Open();
            using (var cmd = conn.CreateCommand()) {

                cmd.CommandText = "SELECT Identifier, Name, Level, Description, LastModifiedDate, Data FROM Scenarios ORDER BY Identifier DESC LIMIT 10";

                using (var reader = cmd.ExecuteReader()) {
                    while (reader.Read()) {
                        var row = new List<string> {
                            reader["Identifier"].ToString(),
                            reader["Name"].ToString(),
                            reader["Level"].ToString(),
                            reader["Description"].ToString(),
                            reader["LastModifiedDate"].ToString(),
                            reader["Data"].ToString()
                        };

                        grid.AddRow(row);
                    }
                }
            }
        }
    }

    public void OnButtonPressed(int buttonIndex) {
        switch (buttonIndex)
        {
            case 1: navigate("Gameplay"); break;
            case 2: navigate("MapControlPlay"); break;
            case 3: navigate("Settings"); break;
            default: break;
        }
    }
}
