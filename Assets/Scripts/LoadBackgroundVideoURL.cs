using UnityEngine;
using UnityEngine.Video;
using System.IO;

public class LoadBackgroundVideoURL : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    private const string VIDEO_FILE_NAME = "Background.mp4"; 

    void Awake()
    {
        if (videoPlayer == null)
        {
            Debug.LogError("VideoPlayer 引用丢失，请在 Inspector 中设置。");
            return;
        }

        videoPlayer.source = VideoSource.Url;
        string fullPath;

        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            fullPath = Application.streamingAssetsPath + "/" + VIDEO_FILE_NAME;
        }
        else
        {
            fullPath = Path.Combine(Application.streamingAssetsPath, VIDEO_FILE_NAME);
        }
        
        videoPlayer.url = fullPath;
        Debug.Log("VideoPlayer URL 已设置为: " + fullPath);
        videoPlayer.Prepare();
    }
}