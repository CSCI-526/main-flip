using UnityEngine;

public class CameraAspectRatio : MonoBehaviour
{
    void Start()
    {
        // 设定目标长宽比为 16:9
        float targetaspect = 16.0f / 9.0f;

        // 获取当前窗口的长宽比
        float windowaspect = (float)Screen.width / (float)Screen.height;

        // 计算缩放比例
        float scaleheight = windowaspect / targetaspect;

        // 获取当前摄像机组件
        Camera camera = GetComponent<Camera>();

        // 如果 scaleheight < 1.0，说明窗口太窄/太高（例如 iPad 或 4:3 屏幕）
        // 我们需要上下留黑边 (Letterbox)
        if (scaleheight < 1.0f)
        {
            Rect rect = camera.rect;

            rect.width = 1.0f;
            rect.height = scaleheight;
            rect.x = 0;
            rect.y = (1.0f - scaleheight) / 2.0f;

            camera.rect = rect;
        }
        else // 如果 scaleheight > 1.0，说明窗口太宽（例如带鱼屏）
        // 我们需要左右留黑边 (Pillarbox)
        {
            float scalewidth = 1.0f / scaleheight;

            Rect rect = camera.rect;

            rect.width = scalewidth;
            rect.height = 1.0f;
            rect.x = (1.0f - scalewidth) / 2.0f;
            rect.y = 0;

            camera.rect = rect;
        }
    }
}