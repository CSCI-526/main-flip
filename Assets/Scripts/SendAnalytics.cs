using UnityEngine;
using UnityEngine.Networking;
using System.Text;

public class SendAnalytics : MonoBehaviour
{
    private const string FORM_URL = "https://script.google.com/macros/s/AKfycbzNSDrHq6YQLMMASKa3KmIK1gvG88ecSAqNzxZfewLLjXSu8czyb0igJh0yNfoXMgzgkQ/exec";

    public static void SendDeath(string levelName, float x, float y)
    {
        Debug.Log($"[DEBUG] Sending death data: Level={levelName}, X={x}, Y={y}");

        // Prepare JSON
        string json = JsonUtility.ToJson(new DeathData { level = levelName, x = x, y = y });
        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);

        UnityWebRequest request = new UnityWebRequest(FORM_URL, "POST");
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        request.SendWebRequest().completed += (op) =>
        {
            if (request.result != UnityWebRequest.Result.Success)
                Debug.LogError("[SendAnalytics] POST failed: " + request.error);
            else
                Debug.Log("[SendAnalytics] POST successful! Response: " + request.downloadHandler.text);
        };
    }

    [System.Serializable]
    public class DeathData
    {
        public string level;
        public float x;
        public float y;
    }
}
