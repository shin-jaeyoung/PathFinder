using UnityEngine;
using UnityEditor;
using TMPro;
using System.Collections.Generic;

public class PrefabFontReplacer : EditorWindow
{
    public TMP_FontAsset newFont;

    [MenuItem("Tools/Replace All Prefab Fonts")]
    public static void ShowWindow() => GetWindow<PrefabFontReplacer>("Prefab Font Replacer");

    void OnGUI()
    {
        GUILayout.Label("교체할 새 TMP 폰트를 넣어주세요", EditorStyles.boldLabel);
        newFont = (TMP_FontAsset)EditorGUILayout.ObjectField("New TMP Font", newFont, typeof(TMP_FontAsset), false);

        if (GUILayout.Button("Project 내 모든 프리팹 폰트 교체 시작"))
        {
            if (newFont == null) { Debug.LogError("폰트를 먼저 할당하세요!"); return; }
            ReplaceFontsInPrefabs();
        }
    }

    void ReplaceFontsInPrefabs()
    {
        // 1. 프로젝트 내의 모든 프리팹 경로를 가져옴
        string[] allPrefabGuids = AssetDatabase.FindAssets("t:Prefab");
        int count = 0;

        foreach (string guid in allPrefabGuids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            // 2. 프리팹 로드 (Edit Mode)
            GameObject prefabRoot = PrefabUtility.LoadPrefabContents(path);

            // 3. 프리팹 내부의 모든 TMP 컴포넌트 찾기
            var texts = prefabRoot.GetComponentsInChildren<TextMeshProUGUI>(true);

            if (texts.Length > 0)
            {
                foreach (var t in texts)
                {
                    Undo.RecordObject(t, "Replace Font");
                    t.font = newFont;
                    EditorUtility.SetDirty(t);
                }
                // 4. 변경사항 저장
                PrefabUtility.SaveAsPrefabAsset(prefabRoot, path);
                count++;
            }

            // 5. 로드된 프리팹 메모리 해제
            PrefabUtility.UnloadPrefabContents(prefabRoot);
        }

        AssetDatabase.SaveAssets();
        Debug.Log($"{count}개의 프리팹에서 폰트 교체 완료!");
    }
}