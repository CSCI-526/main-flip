using UnityEngine;
using System.Collections.Generic;

public class CarryPassenger : MonoBehaviour
{
    [Header("Who to carry")]
    public string playerTag = "Player";

    // 追踪当前接触到的平台上的乘客
    private readonly HashSet<Rigidbody2D> passengers = new HashSet<Rigidbody2D>();
    private readonly Dictionary<Rigidbody2D, Vector2> lastPassengerPos = new Dictionary<Rigidbody2D, Vector2>();

    private Rigidbody2D pivotRb;

    // 记录上一物理帧的平台位姿
    private Vector2 lastPivotPos;
    private float lastPivotAngleDeg;

    void Awake()
    {
        pivotRb = GetComponent<Rigidbody2D>();
        if (!pivotRb)
            Debug.LogError("[CarryPassengers2D_Delta] Pivot 需要 Rigidbody2D (Kinematic).");
    }

    void Start()
    {
        lastPivotPos = pivotRb.position;
        lastPivotAngleDeg = pivotRb.rotation;
    }

    void FixedUpdate()
    {
        // 计算平台这一帧的位移 + 旋转增量
        Vector2 pivotNow = pivotRb.position;
        float angleNow = pivotRb.rotation;

        Vector2 dPos = pivotNow - lastPivotPos;
        float dAngleDeg = Mathf.DeltaAngle(lastPivotAngleDeg, angleNow);
        float dAngleRad = dAngleDeg * Mathf.Deg2Rad;

        // 对每个仍在接触的乘客，按照 “绕Pivot旋转 + 平移” 计算新位置
        foreach (var rb in passengers)
        {
            if (!rb) continue;

            // 取上一帧乘客位置（世界坐标）
            Vector2 prevP = lastPassengerPos.TryGetValue(rb, out var v) ? v : rb.position;

            // 上一帧相对 Pivot 的向量
            Vector2 relPrev = prevP - lastPivotPos;

            // 旋转增量作用到相对向量
            Vector2 relNow = Rotate(relPrev, dAngleRad);

            // 新位置 = 新的 Pivot 位置 + 旋转后的相对向量（已包含平台平移）
            Vector2 targetPos = pivotNow + relNow;

            rb.MovePosition(targetPos); // 物理友好搬运，不影响玩家脚本输入

            // 记录用于下一帧增量计算
            lastPassengerPos[rb] = targetPos;
        }

        // 更新平台上一帧位姿
        lastPivotPos = pivotNow;
        lastPivotAngleDeg = angleNow;
    }

    // —— 碰撞保持期间，加入/移除乘客 —— //
    void OnCollisionEnter2D(Collision2D c) => TryAddPassenger(c.collider);
    void OnCollisionStay2D(Collision2D c) => TryAddPassenger(c.collider); // 保底：持续接触也保证在集合里
    void OnCollisionExit2D(Collision2D c) => TryRemovePassenger(c.collider);

    // 若你的平台使用 Trigger 接触（不推荐），也兼容：
    void OnTriggerEnter2D(Collider2D other) => TryAddPassenger(other);
    void OnTriggerStay2D(Collider2D other) => TryAddPassenger(other);
    void OnTriggerExit2D(Collider2D other) => TryRemovePassenger(other);

    void TryAddPassenger(Collider2D col)
    {
        if (!col || !col.CompareTag(playerTag)) return;
        var rb = col.attachedRigidbody;
        if (!rb) return;

        if (passengers.Add(rb))
        {
            // 新上车：记录其当前世界位置作为上一帧位置
            lastPassengerPos[rb] = rb.position;
        }
    }

    void TryRemovePassenger(Collider2D col)
    {
        if (!col || !col.CompareTag(playerTag)) return;
        var rb = col.attachedRigidbody;
        if (!rb) return;

        passengers.Remove(rb);
        lastPassengerPos.Remove(rb);
    }

    // 2D 向量绕原点旋转
    static Vector2 Rotate(Vector2 v, float radians)
    {
        float cos = Mathf.Cos(radians);
        float sin = Mathf.Sin(radians);
        return new Vector2(
            v.x * cos - v.y * sin,
            v.x * sin + v.y * cos
        );
    }
}
