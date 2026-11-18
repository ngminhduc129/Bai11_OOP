/****************/
/* Mã sinh viên: <MSSV> */
/* Họ tên: <HỌ TÊN> */
/****************/

namespace CampusSafetyHub.Mediator;

/// <summary>
/// Điều khiển cửa thoát hiểm (mẫu Mediator, chỉ quản lý trạng thái của mình).
/// </summary>
public sealed class DoorController
{
    /// <summary>
    /// Lấy trạng thái cửa đã mở khóa hay chưa (chỉ đọc).
    /// </summary>
    public bool IsUnlocked { get; private set; }

    /// <summary>
    /// Mở khóa cửa thoát hiểm.
    /// </summary>
    public void Unlock() => IsUnlocked = true;

    /// <summary>
    /// Khóa lại cửa thoát hiểm.
    /// </summary>
    public void Lock() => IsUnlocked = false;
}
