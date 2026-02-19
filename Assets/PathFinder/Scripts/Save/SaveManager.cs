using System.IO;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    private string savePath;
    private string fileName = "player_save.json";

    private void Awake()
    {
        savePath = Path.Combine(Application.persistentDataPath, fileName);

        Debug.Log($"저장 경로: {savePath}");
    }


    // GameManager에서 준비한 데이터를 실제 JSON 파일로 저장
    public void SaveToFile(SaveData data)
    {
        try
        {
            string json = JsonUtility.ToJson(data, true);
            File.WriteAllText(savePath, json);

            Debug.Log("파일 저장 성공");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"저장 중 오류 발생: {e.Message}");
        }
    }


    // 저장된 파일을 읽어서 SaveData 객체로 반환
    public SaveData GetLoadedData()
    {
        if (!File.Exists(savePath))
        {
            Debug.LogWarning("세이브 파일을 찾을 수 없습니다.");
            return null;
        }

        try
        {
            string json = File.ReadAllText(savePath);

            SaveData data = JsonUtility.FromJson<SaveData>(json);

            Debug.Log("데이터 로드 성공");
            return data;
        }
        catch (System.Exception e)
        {
            Debug.LogError($"로드 중 오류: {e.Message}");
            return null;
        }
    }

    public bool HasSaveFile()
    {
        return File.Exists(Path.Combine(Application.persistentDataPath, "player_save.json"));
    }
    // (테스트용) 저장된 세이브 파일을 삭제

    [ContextMenu("Delete Save File")]
    public void DeleteSaveFile()
    {
        if (File.Exists(savePath))
        {
            File.Delete(savePath);
            Debug.Log("세이브 파일 삭제 완료.");
        }
    }
}