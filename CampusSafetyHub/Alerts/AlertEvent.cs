/****************/
/* Mã sinh viên: <MSSV> */
/* Họ tên: <HỌ TÊN> */
/****************/

namespace CampusSafetyHub.Alerts;

/// <summary>
/// Sự kiện cảnh báo bất biến, chứa thông tin cảm biến và mức độ nghiêm trọng.
/// </summary>
/// <param name="SensorName">Tên cảm biến phát sinh sự kiện.</param>
/// <param name="Value">Giá trị đo được tại thời điểm sự kiện.</param>
/// <param name="Unit">Đơn vị đo lường của giá trị.</param>
/// <param name="Severity">Mức độ nghiêm trọng ("WARNING" hoặc "CRITICAL").</param>
/// <param name="OccurredAt">Thời điểm xảy ra sự kiện.</param>
public sealed record AlertEvent(string SensorName, double Value, string Unit, string Severity, DateTime OccurredAt);
