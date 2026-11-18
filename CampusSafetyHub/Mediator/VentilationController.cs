/****************/
/* Mã sinh viên: <MSSV> */
/* Họ tên: <HỌ TÊN> */
/****************/

namespace CampusSafetyHub.Mediator;

/// <summary>
/// Điều khiển quạt hút khói (mẫu Mediator, chỉ quản lý trạng thái của mình).
/// </summary>
public sealed class VentilationController
{
    /// <summary>
    /// Lấy trạng thái quạt hút khói đang chạy hay không (chỉ đọc).
    /// </summary>
    public bool IsExhaustRunning { get; private set; }

    /// <summary>
    /// Bật quạt hút khói.
    /// </summary>
    public void StartExhaust() => IsExhaustRunning = true;

    /// <summary>
    /// Tắt quạt hút khói.
    /// </summary>
    public void Stop() => IsExhaustRunning = false;
}
