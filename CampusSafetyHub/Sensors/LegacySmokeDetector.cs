/****************/
/* Mã sinh viên: <MSSV> */
/* Họ tên: <HỌ TÊN> */
/****************/

namespace CampusSafetyHub.Sensors;

/// <summary>
/// Lớp phát hiện khói kế thừa (legacy), không được phép sửa đổi.
/// </summary>
public sealed class LegacySmokeDetector
{
    private int smokeLevel;

    /// <summary>
    /// Khởi tạo cảm biến khói legacy với mức khói ban đầu.
    /// </summary>
    /// <param name="initialSmokeLevel">Mức khói ban đầu (0..1000).</param>
    public LegacySmokeDetector(int initialSmokeLevel)
    {
        smokeLevel = Math.Clamp(initialSmokeLevel, 0, 1000);
    }

    /// <summary>
    /// Lấy mức khói hiện tại.
    /// </summary>
    /// <returns>Mức khói (0..1000).</returns>
    public int GetSmokeLevel() => smokeLevel;

    /// <summary>
    /// Lấy mô tả mức khói hiện tại.
    /// </summary>
    /// <returns>Chuỗi mô tả mức khói.</returns>
    public string GetDescription() => $"Mức khói: {smokeLevel}";

    /// <summary>
    /// Thiết lập mức khói mới (được clamp về khoảng 0..1000).
    /// </summary>
    /// <param name="value">Giá trị mức khói mới.</param>
    public void SetSmokeLevel(int value)
    {
        smokeLevel = Math.Clamp(value, 0, 1000);
    }
}
