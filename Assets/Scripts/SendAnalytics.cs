// using UnityEngine;
// using UnityEngine.Networking;
// using System.Text;

// public class SendAnalytics : MonoBehaviour
// {
//     private const string FORM_URL = "https://script.google.com/macros/s/AKfycbw6bakGRNng2EfvmUtC0IyiOpGd2_SYzt9ixTqWFNtUdKCIGPWGFFVTcRXIM-MYvmboHg/exec";

//     public static void SendDeath(string levelName, float x, float y)
//     {
//         Debug.Log($"[DEBUG] Sending death data: Level={levelName}, X={x}, Y={y}");

//         // Prepare JSON
//         string json = JsonUtility.ToJson(new DeathData { level = levelName, x = x, y = y });
//         byte[] bodyRaw = Encoding.UTF8.GetBytes(json);

//         UnityWebRequest request = new UnityWebRequest(FORM_URL, "POST");
//         request.uploadHandler = new UploadHandlerRaw(bodyRaw);
//         request.downloadHandler = new DownloadHandlerBuffer();
//         request.SetRequestHeader("Content-Type", "application/json");

//         request.SendWebRequest().completed += (op) =>
//         {
//             if (request.result != UnityWebRequest.Result.Success)
//                 Debug.LogError("[SendAnalytics] POST failed: " + request.error);
//             else
//                 Debug.Log("[SendAnalytics] POST successful! Response: " + request.downloadHandler.text);
//         };
//     }

//     [System.Serializable]
//     public class DeathData
//     {
//         public string level;
//         public float x;
//         public float y;
//     }
// }


using UnityEngine;
using UnityEngine.Networking;
using System.Text;
using System.Collections;

public class SendAnalytics : MonoBehaviour
{
    private const string SCRIPT_URL = "https://script.google.com/macros/s/AKfycbyWhQAH5IY0asqZdEL_49pE4qYf2RXjFy5RdBEeHzVOg9wQebk3OkLuMS5ZIuJIIr2uYA/exec"; // Replace with your Apps Script URL

    [System.Serializable]
    public class DeathData
    {
        public string level;
        public float x;
        public float y;
    }

    // Call this to send death data
    public static void SendDeath(MonoBehaviour caller, string levelName, float x, float y)
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        // WebGL: use GET request via URL
        caller.StartCoroutine(SendDeathWebGL(levelName, x, y));
#else
        // Editor / standalone: use POST
        caller.StartCoroutine(SendDeathPOST(levelName, x, y));
#endif
    }

    // --- POST method for Unity Editor / Standalone ---
    private static IEnumerator SendDeathPOST(string levelName, float x, float y)
    {
        Debug.Log($"[DEBUG] Sending death data via POST: Level={levelName}, X={x}, Y={y}");

        string json = JsonUtility.ToJson(new DeathData { level = levelName, x = x, y = y });
        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);

        UnityWebRequest request = new UnityWebRequest(SCRIPT_URL, "POST");
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
            Debug.Log("[SendAnalytics] POST successful! Response: " + request.downloadHandler.text);
        else
            Debug.LogError("[SendAnalytics] POST failed: " + request.error);
    }

    // --- GET method for WebGL / browser ---
    private static IEnumerator SendDeathWebGL(string levelName, float x, float y)
    {
        Debug.Log($"[DEBUG] Sending death data via GET (WebGL): Level={levelName}, X={x}, Y={y}");

        string url = $"{SCRIPT_URL}?level={UnityWebRequest.EscapeURL(levelName)}&x={x}&y={y}";

        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
            Debug.Log("[SendAnalytics] GET successful! Response: " + request.downloadHandler.text);
        else
            Debug.LogError("[SendAnalytics] GET failed: " + request.error);
    }
}
