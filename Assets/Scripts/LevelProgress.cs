using UnityEngine;

public static class LevelProgress
{
    private const string K_INIT = "LP_INIT_DONE";
    private const string K_PREFIX = "LP_UNLOCKED_";

    // run once: only Tutorial is unlocked
    private static void EnsureInit()
    {
        if (PlayerPrefs.HasKey(K_INIT)) return;
        PlayerPrefs.SetInt(K_INIT, 1);
        PlayerPrefs.SetInt(K_PREFIX + "flip gravity tutorial", 1);  
        PlayerPrefs.Save();
    }

    public static bool IsUnlocked(string sceneName)
    {
        if (string.IsNullOrEmpty(sceneName)) return false;
        EnsureInit();
        return PlayerPrefs.GetInt(K_PREFIX + sceneName, 0) == 1;
    }

    public static void Unlock(string sceneName)
    {
        if (string.IsNullOrEmpty(sceneName)) return;
        PlayerPrefs.SetInt(K_PREFIX + sceneName, 1);
        PlayerPrefs.Save();
    }

#if UNITY_EDITOR
    [UnityEditor.MenuItem("Game/Progress/Clear LevelProgress")]
    public static void ClearAll()
    {
        PlayerPrefs.DeleteKey(K_INIT);
        PlayerPrefs.DeleteAll();
    }
#endif
}
