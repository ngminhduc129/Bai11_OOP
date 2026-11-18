/****************/
/* Mã sinh viên: <MSSV> */
/* Họ tên: <HỌ TÊN> */
/****************/

namespace CampusSafetyHub.Alerts;

using CampusSafetyHub.Sensors;

/// <summary>
/// Chủ thể trong mẫu Observer: theo dõi cảm biến và thông báo cho các quan sát viên khi vượt ngưỡng.
/// </summary>
public sealed class SensorMonitor
{
    private readonly List<IAlertObserver> observers = new();

    /// <summary>
    /// Đăng ký một quan sát viên nhận thông báo.
    /// </summary>
    /// <param name="observer">Quan sát viên cần đăng ký.</param>
    /// <exception cref="ArgumentNullException">Ném khi <paramref name="observer"/> là null.</exception>
    public void Attach(IAlertObserver observer)
    {
        ArgumentNullException.ThrowIfNull(observer);
        if (!observers.Contains(observer))
        {
            observers.Add(observer);
        }
    }

    /// <summary>
    /// Hủy đăng ký một quan sát viên.
    /// </summary>
    /// <param name="observer">Quan sát viên cần hủy đăng ký.</param>
    public void Detach(IAlertObserver observer)
    {
        observers.Remove(observer);
    }

    /// <summary>
    /// Kiểm tra cảm biến và phát sinh cảnh báo nếu giá trị vượt ngưỡng.
    /// </summary>
    /// <param name="sensorName">Tên cảm biến.</param>
    /// <param name="sensor">Cảm biến cần kiểm tra.</param>
    public void CheckSensor(string sensorName, ISensor sensor)
    {
        double value = sensor.ReadValue();
        string unit = sensor.GetUnit();

        string? severity = value switch
        {
            >= 70 => "CRITICAL",
            >= 50 => "WARNING",
            _ => null
        };

        // Dưới ngưỡng WARNING -> không có sự kiện cảnh báo, thoát sớm.
        if (severity is null)
        {
            return;
        }

        var alert = new AlertEvent(sensorName, value, unit, severity, DateTime.Now);
        Notify(alert);
    }

    /// <summary>
    /// Thông báo sự kiện cảnh báo cho tất cả quan sát viên đã đăng ký.
    /// Một quan sát viên bị lỗi không ảnh hưởng đến các quan sát viên còn lại.
    /// </summary>
    /// <param name="alert">Sự kiện cảnh báo cần thông báo.</param>
    private void Notify(AlertEvent alert)
    {
        // Chụp ảnh chập danh sách để quan sát viên tự hủy đăng ký trong Update không làm lỗi vòng lặp.
        foreach (var observer in observers.ToArray())
        {
            try
            {
                observer.Update(alert);
            }
            catch (Exception ex)
            {
                // Một quan sát viên lỗi không được phép làm gián đoạn các quan sát viên khác.
                Console.WriteLine($"[MONITOR][ERROR] Observer {observer.GetType().Name} failed: {ex.Message}");
            }
        }
    }
}
