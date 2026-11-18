/****************/
/* Mã sinh viên: <MSSV> */
/* Họ tên: <HỌ TÊN> */
/****************/

namespace CampusSafetyHub.Sensors;

/// <summary>
/// Giao diện cảm biến: đọc giá trị và đơn vị đo lường.
/// </summary>
public interface ISensor
{
    /// <summary>
    /// Đọc giá trị hiện tại từ cảm biến.
    /// </summary>
    /// <returns>Giá trị đo được (double).</returns>
    double ReadValue();

    /// <summary>
    /// Lấy đơn vị đo lường của cảm biến.
    /// </summary>
    /// <returns>Chuỗi đơn vị (ví dụ "% smoke").</returns>
    string GetUnit();
}
