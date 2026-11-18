/****************/
/* Mã sinh viên: <MSSV> */
/* Họ tên: <HỌ TÊN> */
/****************/

namespace CampusSafetyHub.Alerts;

/// <summary>
/// Giao diện quan sát viên nhận thông báo sự kiện cảnh báo từ SensorMonitor.
/// </summary>
public interface IAlertObserver
{
    /// <summary>
    /// Nhận thông báo về một sự kiện cảnh báo mới.
    /// </summary>
    /// <param name="alertEvent">Sự kiện cảnh báo vừa xảy ra.</param>
    void Update(AlertEvent alertEvent);
}
