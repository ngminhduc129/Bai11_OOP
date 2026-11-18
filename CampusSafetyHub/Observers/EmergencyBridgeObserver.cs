/****************/
/* Mã sinh viên: <MSSV> */
/* Họ tên: <HỌ TÊN> */
/****************/

namespace CampusSafetyHub.Observers;

using CampusSafetyHub.Alerts;
using CampusSafetyHub.Mediator;

/// <summary>
/// Quan sát viên cầu nối cảnh báo khói khẩn cấp sang EmergencyMediator (mẫu Observer).
/// </summary>
public sealed class EmergencyBridgeObserver : IAlertObserver
{
    private readonly IEmergencyMediator mediator;

    /// <summary>
    /// Khởi tạo cầu nối với mediator khẩn cấp.
    /// </summary>
    /// <param name="mediator">Mediator điều phối thiết bị khẩn cấp.</param>
    /// <exception cref="ArgumentNullException">Ném khi <paramref name="mediator"/> là null.</exception>
    public EmergencyBridgeObserver(IEmergencyMediator mediator)
    {
        this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    /// <summary>
    /// Khi severity là CRITICAL thì thông báo mediator sự kiện FIRE_DETECTED.
    /// WARNING chỉ ghi chú, không kích hoạt ứng phó khẩn cấp vì chưa đạt mức nguy hiểm.
    /// </summary>
    /// <param name="alertEvent">Sự kiện cảnh báo cần xử lý.</param>
    public void Update(AlertEvent alertEvent)
    {
        if (alertEvent.Severity == "CRITICAL")
        {
            mediator.Notify(this, "FIRE_DETECTED");
        }
        // WARNING không kích hoạt ứng phó: chưa đủ mức nguy hiểm để bật alarm/mở cửa.
    }
}
