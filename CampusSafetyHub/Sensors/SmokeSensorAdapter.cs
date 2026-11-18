/****************/
/* Mã sinh viên: <MSSV> */
/* Họ tên: <HỌ TÊN> */
/****************/

namespace CampusSafetyHub.Sensors;

/// <summary>
/// Adapter biến đổi LegacySmokeDetector sang giao diện ISensor (mẫu Adapter, composición).
/// </summary>
public sealed class SmokeSensorAdapter : ISensor
{
    private readonly LegacySmokeDetector legacy;

    /// <summary>
    /// Khởi tạo adapter bao quanh một LegacySmokeDetector.
    /// </summary>
    /// <param name="legacy">Cảm biến khói legacy cần bao quanh.</param>
    /// <exception cref="ArgumentNullException">Ném khi <paramref name="legacy"/> là null.</exception>
    public SmokeSensorAdapter(LegacySmokeDetector legacy)
    {
        this.legacy = legacy ?? throw new ArgumentNullException(nameof(legacy));
    }

    /// <summary>
    /// Đọc giá trị khói từ lớp legacy và quy đổi sang tỷ lệ phần trăm (0..100%).
    /// Luôn đọc trực tiếp từ legacy, không sao chép trạng thái.
    /// </summary>
    /// <returns>Giá trị khói tính theo phần trăm (0..100).</returns>
    public double ReadValue() => legacy.GetSmokeLevel() / 10.0;

    /// <summary>
    /// Lấy đơn vị đo lường của cảm biến khói.
    /// </summary>
    /// <returns>Chuỗi "% smoke".</returns>
    public string GetUnit() => "% smoke";
}
