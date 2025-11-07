using UnityEngine;

public class FoVFocusObject : MonoBehaviour
{
    [Header("Basic Parameters 基础参数")]
    public Camera cam;            // Camera to control 要控制的摄像机
    public Transform player;      // Player trigger reference 玩家（触发检测对象）
    public Transform focusTarget; // The object to focus on 要聚焦的目标物体

    [Header("Camera Parameters 镜头参数")]
    public float moveSpeed = 2f;      // Camera move speed 镜头移动速度
    public float zoomSpeed = 2f;      // Camera zoom speed 镜头变焦速度
    public float focusSize = 0.2f;    // Target orthographic size 聚焦时的正交视野大小
    public float focusDuration = 2f;  // Duration of focus in seconds 聚焦持续时间（秒）

    [Header("Usage Settings 使用设置")]
    public bool canReuse = true;      // Whether the trigger can be reused 是否允许重复触发（true=可多次，false=仅一次）

    private Vector3 originalPosition; // Camera position before focusing 聚焦前摄像机位置
    private float originalSize;       // Camera orthographic size before focusing 聚焦前视野大小
    private Vector3 focusPosition;    // Target camera position during focus 聚焦目标位置

    private bool isFocusing = false;  // Currently focusing 是否正在聚焦
    private bool isReturning = false; // Returning to original state 是否正在返回原状态
    private float focusTimer = 0f;    // Focus countdown timer 聚焦计时器
    private bool hasPlayed = false;   // Whether focus has already been played 是否已触发过

    void Start()
    {
        // Pre-calculate focus position (keep Z unchanged)
        // 预计算聚焦位置（保持原 Z 深度）
        focusPosition = focusTarget != null ? focusTarget.position : transform.position;
        focusPosition.z = cam.transform.position.z;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform != player) return;

        // Ignore if reuse not allowed and already triggered
        // 如果不允许复用且已播放过，直接返回
        if (!canReuse && hasPlayed) return;

        // Mark as used immediately if non-reusable
        // 若不允许复用，首次触发后立即标记为已播放
        if (!canReuse)
            hasPlayed = true;

        // Record current camera state before focusing
        // 记录当前摄像机状态以便返回
        originalPosition = cam.transform.position;
        originalSize = cam.orthographicSize;

        isFocusing = true;
        isReturning = false;
        focusTimer = focusDuration;
    }

    void Update()
    {
        if (isFocusing)
        {
            // ① Focus phase 聚焦阶段：移动并缩放镜头
            cam.transform.position = Vector3.Lerp(cam.transform.position, focusPosition, moveSpeed * Time.deltaTime);
            cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, focusSize, zoomSpeed * Time.deltaTime);

            focusTimer -= Time.deltaTime;
            if (focusTimer <= 0f)
            {
                isFocusing = false;
                isReturning = true;
            }
        }
        else if (isReturning)
        {
            // ② Return phase 回归阶段：恢复镜头位置与视野
            cam.transform.position = Vector3.Lerp(cam.transform.position, originalPosition, moveSpeed * Time.deltaTime);
            cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, originalSize, zoomSpeed * Time.deltaTime);

            // Snap to original position once near to prevent jitter
            // 当接近原位时强制对齐，避免抖动
            if (Vector3.Distance(cam.transform.position, originalPosition) < 0.01f &&
                Mathf.Abs(cam.orthographicSize - originalSize) < 0.01f)
            {
                cam.transform.position = originalPosition;
                cam.orthographicSize = originalSize;
                isReturning = false;

                // Optionally disable after playing if non-reusable
                // 若不允许复用，可在执行完后禁用脚本
                if (!canReuse)
                    enabled = false;
            }
        }
        // When idle, do nothing so other camera scripts can run
        // 非聚焦阶段不干预相机，让其他控制逻辑接管
    }
}