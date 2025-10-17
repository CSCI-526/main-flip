# flip-csci526-2025fall
[English Version](./README.md)
## 磁力效果实现
- 磁力效果实现代码为`./Assets/Scripts/Magnetism.cs`
  - 枚举类型
    - MagnetismAxis：磁力作用轴，可选值有{None, X, Y}
    - MagnetismMode：磁力作用效果模式，用于实现不同的磁力效果，可选值有{None, Radial, Axial}
      - Radial：径向磁力，磁力方向为两个物体中心连线
      - Axial：轴向磁力，磁力方向为MagnetismAxis指定的方向轴
    - MagneticPole：磁力极性，可选值有{None, North, South}
  - 参数列表
    - [float] maxMagnetismStrength：磁力最大强度
    - [float] minMagnetismStrength：磁力最小强度
    - [bool] changeableMode：是否允许当前的磁力作用效果模式（currentMode）自动改变，用于实现不同的磁力效果
    - [MagneticPole] currentPole：当前的磁极性
    - [MagnetismMode] currentMode：当前的磁力作用效果模式
    - [MagnetismAxis] currentAxis：当前的磁力作用轴
    - [List<Rigidbody2D>] attractedObjects：限制磁力作用范围的Collider内部的Rigidbody2D列表
    - [List<GameObject>] collidingMagnets：与当前物体接触的物体的GameObject列表

  - 核心函数说明
    - UpdateColor：根据物体磁极性更新物体颜色
    - UpdateMagnetism：更新物体的磁力作用效果
      1. 磁力效果只在特定的物体上生效
          1. 磁力效果不对自身进行作用
          2. 磁力效果不对没有"Player"标签的物体进行作用
      2. 磁力效果在特定的Collider上生效
          1. 对挂载此脚本的物品，添加特定的Collider组件并设置Is Trigger为True，此Collider的范围限制了磁力效果生效的范围
          2. 在Collider范围内，磁力大小限制在[minMagnetismStrength, maxMagnetismStrength]范围内，大小随距离平方衰减
        ```csharp
        float currentMagnetismStrength = maxMagnetismStrength;
        float distance = direction.magnitude;
        currentMagnetismStrength /= (distance * distance);
        currentMagnetismStrength = Mathf.Max(minMagnetismStrength, Mathf.Min(currentMagnetismStrength, maxMagnetismStrength));
        ``` 
      3. 径向磁力/轴向磁力切换机制
          1. 径向磁力的设计有利于避免磁力方向变化导致的复杂物理效果，但也导致了磁吸效果在两物体贴合时不自然的问题，想想如下场景-->磁力平台沿X轴移动，我们希望Player通过切换磁极能够沿Y轴移动并吸附到磁力平台，在两者贴合后一同沿X轴移动，存在如下两个问题：
             - 两物体未贴合时，若采用径向磁力，则Player在X轴上异常移动
             - 两物体贴合时，若采用Y轴轴向磁力，则Player在X轴上无法正常移动
          2. 切换机制设计：允许轴向磁力物体在接触后切换为径向磁力，从而解决了上述问题

- Player磁极性切换实现代码为`./Assets/Scripts/PlayerMagnetismController.cs`，按`shift`键切换极性（当前为South则切换为North，反之亦然），Player磁极性初始化为South

## 摩擦力效果实现
- 实现摩擦力的目的：令Player在平台上移动时速度逐渐衰减，避免Player过度位移
- 摩擦力效果的实现代码为`./Assets/Scripts/Friction.cs`
- 实现逻辑为Player与指定平台接触并移动时，沿Player移动方向的反方向添加阻力
```csharp
Vector2 frictionDirection = -otherRb.linearVelocity.normalized;
float frictionMagnitude = frictionStrength;
otherRb.AddForce(frictionDirection * frictionMagnitude, ForceMode2D.Force);
```