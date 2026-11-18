/****************/
/* Mã sinh viên: <MSSV> */
/* Họ tên: <HỌ TÊN> */
/****************/

namespace CampusSafetyHub.Observers;

using CampusSafetyHub.Alerts;

/// <summary>
/// Quan sát viên ghi nhật ký cảnh báo vào bộ nhớ và in ra Console (mẫu Observer).
/// </summary>
public sealed class AuditLogObserver : IAlertObserver
{
    private readonly List<string> logs = new();

    /// <summary>
    /// Lấy danh sách nhật ký chỉ đọc để kiểm thử truy xuất.
    /// </summary>
    public IReadOnlyList<string> Logs => logs.AsReadOnly();

    /// <summary>
    /// Ghi nhật ký sự kiện cảnh báo và in ra Console.
    /// </summary>
    /// <param name="alertEvent">Sự kiện cảnh báo cần lưu nhật ký.</param>
    public void Update(AlertEvent alertEvent)
    {
        string entry = $"[{alertEvent.OccurredAt:yyyy-MM-dd HH:mm:ss}] {alertEvent.SensorName}: {alertEvent.Value:F1}{alertEvent.Unit} [{alertEvent.Severity}]";
        logs.Add(entry);
        Console.WriteLine($"[AUDIT] {entry}");
    }
}
