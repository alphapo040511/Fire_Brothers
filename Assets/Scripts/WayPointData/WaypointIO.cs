using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class WaypointIO
{
    private const string c_SaveFolder = "Assets/Resources/Database";

    private static string GetFullPath(int stageIndex)
    {
        return Path.Combine(c_SaveFolder, $"stage_{stageIndex}_waypoints.json");
    }

    private static string GetResourcesLoadPath(int stageIndex)
    {
        return $"DataBase/stage_{stageIndex}_waypoints";
    }

    public static void SaveWaypoints(List<Waypoint> waypoints, int stageIndex)
    {
        if (!Directory.Exists(c_SaveFolder))
            Directory.CreateDirectory(c_SaveFolder);

        var data = new WaypointData();
        foreach (var wp in waypoints)
            data.points.Add(wp.transform.position);

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(GetFullPath(stageIndex), json);
        Debug.Log($"웨이포인트 저장 완료: {GetFullPath(stageIndex)}");
    }

    public static List<Vector3> LoadWaypointPositions(int stageIndex)
    {
        TextAsset jsonFile = Resources.Load<TextAsset>(GetResourcesLoadPath(stageIndex));

        if (jsonFile == null)
        {
            Debug.LogWarning($"Resources에서 웨이포인트 JSON을 찾을 수 없습니다: {GetResourcesLoadPath(stageIndex)}");
            return null;
        }

        var data = JsonUtility.FromJson<WaypointData>(jsonFile.text);
        return data.points;
    }
}

