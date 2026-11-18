/****************/
/* Mã sinh viên: <MSSV> */
/* Họ tên: <HỌ TÊN> */
/****************/

namespace CampusSafetyHub.Mediator;

/// <summary>
/// Giao diện mediator điều phối các thiết bị ứng phó khẩn cấp (mẫu Mediator).
/// </summary>
public interface IEmergencyMediator
{
    /// <summary>
    /// Thông báo mediator về một sự kiện để điều phối thiết bị.
    /// </summary>
    /// <param name="sender">Người gửi sự kiện.</param>
    /// <param name="eventName">Tên sự kiện ("FIRE_DETECTED", "ALL_CLEAR"...).</param>
    void Notify(object sender, string eventName);
}
