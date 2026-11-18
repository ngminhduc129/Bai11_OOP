/****************/
/* Mã sinh viên: <MSSV> */
/* Họ tên: <HỌ TÊN> */
/****************/

namespace CampusSafetyHub.Mediator;

/// <summary>
/// Thiết bị báo động khẩn cấp (mẫu Mediator, chỉ quản lý trạng thái của mình).
/// </summary>
public sealed class Alarm
{
    /// <summary>
    /// Lấy trạng thái báo động đang bật hay không (chỉ đọc).
    /// </summary>
    public bool IsActive { get; private set; }

    /// <summary>
    /// Bật báo động.
    /// </summary>
    public void Activate() => IsActive = true;

    /// <summary>
    /// Tắt báo động.
    /// </summary>
    public void Deactivate() => IsActive = false;
}
