using UnityEngine;

public static class Utils
{
    public static float MapSize = 0;
    public static Camera MyCamera;
    public static void SetMapSize(int mapSize)
    {
        MapSize = mapSize;
    }
    public static void SetCamera(Camera camera)
    {
        MyCamera = camera;
    }
    public static void DebugLog(string message, LogType type = LogType.Log)
    {
        if (type == LogType.Log)
        {
            //Debug.Log(Time.time + " - " + message);
        }
        else if (type == LogType.Warning)
        {
            Debug.LogWarning(Time.time + " - " + message);
        }
        else if (type == LogType.Error)
        {
            Debug.LogError(Time.time + " - " + message);
        }
    }
    public static Color LearnRandomColor_0_1()
    {
        return new Color(Random.Range(0.0f, 1), Random.Range(0.0f, 1), Random.Range(0.0f, 1), 1);
    }
    public static Color LearnRandomColor_0_255()
    {
        return new Color(Random.Range(0, 255), Random.Range(00, 255), Random.Range(00, 255), 255);
    }
    public static Vector2 LearnRandomSpawnPoint(int mapSize)
    {
        float halfMapSize = mapSize * 0.5f;
        float edgeMapSize = 0.9f;
        return edgeMapSize * new Vector2(Random.Range(-halfMapSize, halfMapSize), Random.Range(-halfMapSize, halfMapSize));
    }
    public static Vector2 LearnRandomSpawnPoint()
    {
        float halfMapSize = MapSize * 0.5f;
        float edgeMapSize = 0.9f;
        return edgeMapSize * new Vector2(Random.Range(-halfMapSize, halfMapSize), Random.Range(-halfMapSize, halfMapSize));
    }
    public static Vector2 LearnRandomSpawnPoint(int mapSizeX, int mapSizeY)
    {
        float halfMapSizeX = mapSizeX * 0.5f;
        float halfMapSizeY = mapSizeY * 0.5f;
        float edgeMapSize = 0.9f;
        return edgeMapSize * new Vector2(Random.Range(-halfMapSizeX, halfMapSizeX), Random.Range(-halfMapSizeY, halfMapSizeY));
    }
    public static Vector2 LearnRandomSpawnPoint(Vector2 mapSize)
    {
        Vector2 halfMapSize = mapSize * 0.5f;
        float edgeMapSize = 0.9f;
        return edgeMapSize * new Vector2(Random.Range(-halfMapSize.x, halfMapSize.x), Random.Range(-halfMapSize.y, halfMapSize.y));
    }
    public static string GetRandomName(bool isBot)
    {
        string[] names = { "Hüseyin", "Emre", "Can", "Osman", "Seyfi", "Burhan", "Gürcan", "Efe", "Enes", "Ýpek", "Mert", "Asu", "Erol", "Ezgi", "Su"
                , "Adnan", "Burak", "Ceyda", "Deniz", "Haluk", "Iþýk", "Kemal", "Leyla" };

        return names[Random.Range(0, names.Length)] + (isBot ? (" - " + Random.Range(0, 1000)) : "");
    }
    public static Vector3 LearnMouseWorldPosition()
    {
        Vector3 mousePoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePoint.z = 0;
        return mousePoint;
    }
}