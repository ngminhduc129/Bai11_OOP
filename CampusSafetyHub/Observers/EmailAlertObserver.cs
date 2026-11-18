/****************/
/* Mã sinh viên: <MSSV> */
/* Họ tên: <HỌ TÊN> */
/****************/

namespace CampusSafetyHub.Observers;

using CampusSafetyHub.Alerts;

/// <summary>
/// Quan sát viên mô phỏng gửi email cảnh báo (không gửi email thật, mẫu Observer).
/// </summary>
public sealed class EmailAlertObserver : IAlertObserver
{
    private readonly string recipient;

    /// <summary>
    /// Khởi tạo quan sát viên giả lập gửi email.
    /// </summary>
    /// <param name="recipient">Người nhận email cảnh báo.</param>
    public EmailAlertObserver(string recipient)
    {
        this.recipient = recipient;
    }

    /// <summary>
    /// Ghi mô phỏng nội dung email cảnh báo ra Console.
    /// </summary>
    /// <param name="alertEvent">Sự kiện cảnh báo cần gửi qua email.</param>
    public void Update(AlertEvent alertEvent)
    {
        Console.WriteLine($"[EMAIL] To: {recipient}");
        Console.WriteLine($"[EMAIL] Subject: Cảnh báo an toàn - {alertEvent.Severity} tại {alertEvent.SensorName}");
        Console.WriteLine(
            $"[EMAIL] Body: Giá trị {alertEvent.Value:F1}{alertEvent.Unit}, mức độ {alertEvent.Severity}, xảy ra lúc {alertEvent.OccurredAt:HH:mm:ss}.");
        Console.WriteLine("[EMAIL] (Giả lập - không gửi email thật)");
    }
}
