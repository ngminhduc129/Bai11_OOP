/****************/
/* Mã sinh viên: <MSSV> */
/* Họ tên: <HỌ TÊN> */
/****************/

namespace CampusSafetyHub.Observers;

using CampusSafetyHub.Alerts;

/// <summary>
/// Quan sát viên hiển thị cảnh báo trên bảng điều khiển (mẫu Observer).
/// </summary>
public sealed class DashboardObserver : IAlertObserver
{
    /// <summary>
    /// Nhận và hiển thị sự kiện cảnh báo ra Console.
    /// </summary>
    /// <param name="alertEvent">Sự kiện cảnh báo cần hiển thị.</param>
    public void Update(AlertEvent alertEvent)
    {
        Console.WriteLine($"[DASHBOARD] {alertEvent.SensorName}: {alertEvent.Value:F1}{alertEvent.Unit} [{alertEvent.Severity}]");
    }
}
