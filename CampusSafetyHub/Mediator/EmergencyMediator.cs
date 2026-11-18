/****************/
/* Mã sinh viên: <MSSV> */
/* Họ tên: <HỌ TÊN> */
/****************/

namespace CampusSafetyHub.Mediator;

/// <summary>
/// Mediator điều phối Alarm, DoorController, VentilationController khi khói khẩn cấp (mẫu Mediator).
/// </summary>
public sealed class EmergencyMediator : IEmergencyMediator
{
    private readonly Alarm alarm;
    private readonly DoorController doorController;
    private readonly VentilationController ventilationController;
    private bool fireActive;

    /// <summary>
    /// Khởi tạo mediator với ba thiết bị ứng phó.
    /// </summary>
    /// <param name="alarm">Thiết bị báo động.</param>
    /// <param name="doorController">Điều khiển cửa thoát hiểm.</param>
    /// <param name="ventilationController">Điều khiển quạt hút khói.</param>
    /// <exception cref="ArgumentNullException">Ném khi bất kỳ tham số nào là null.</exception>
    public EmergencyMediator(Alarm alarm, DoorController doorController, VentilationController ventilationController)
    {
        this.alarm = alarm ?? throw new ArgumentNullException(nameof(alarm));
        this.doorController = doorController ?? throw new ArgumentNullException(nameof(doorController));
        this.ventilationController = ventilationController ?? throw new ArgumentNullException(nameof(ventilationController));
    }

    /// <summary>
    /// Xử lý sự kiện và điều phối thiết bị tương ứng.
    /// </summary>
    /// <param name="sender">Người gửi sự kiện.</param>
    /// <param name="eventName">Tên sự kiện cần xử lý.</param>
    public void Notify(object sender, string eventName)
    {
        switch (eventName)
        {
            case "FIRE_DETECTED":
                HandleFireDetected();
                break;
            case "ALL_CLEAR":
            case "RESET":
                HandleAllClear();
                break;
            default:
                // Sự kiện không được biết: bỏ qua và ghi log, không ném lỗi.
                Console.WriteLine($"[MEDIATOR] Bỏ qua sự kiện không xác định: {eventName}");
                break;
        }
    }

    private void HandleFireDetected()
    {
        // Idempotent: nếu đã ở trạng thái FIRE thì không kích hoạt lại nhiều lần.
        if (fireActive)
        {
            return;
        }

        fireActive = true;
        alarm.Activate();
        doorController.Unlock();
        ventilationController.StartExhaust();
        Console.WriteLine("[MEDIATOR] Đã kích hoạt ứng phó CHÁY: alarm bật, cửa mở, quạt hút khói chạy.");
    }

    private void HandleAllClear()
    {
        if (!fireActive)
        {
            return;
        }

        fireActive = false;
        alarm.Deactivate();
        doorController.Lock();
        ventilationController.Stop();
        Console.WriteLine("[MEDIATOR] Đã khôi phục trạng thái an toàn: alarm tắt, cửa khóa, quạt tắt.");
    }
}
