# Campus Safety Hub - Mẫu thiết kế Adapter + Observer + Mediator

## Thông tin sinh viên
- Mã sinh viên: 202418872
- Họ tên: Nguyễn Minh Đức

## Mô tả
Chương trình (.NET 8, C# 12, bật chế độ nullable) mô phỏng trung tâm giám sát an toàn phòng máy. Không giao diện đồ họa (GUI), không cơ sở dữ liệu (CSDL), không kết nối mạng, không gửi email thật. Trọng tâm là thiết kế hướng đối tượng và luồng truyền thông giữa các thành phần.

Luồng tổng quát:
LegacySmokeDetector -> SmokeSensorAdapter (ISensor) -> SensorMonitor --AlertEvent-->
  DashboardObserver, EmailAlertObserver, AuditLogObserver, EmergencyBridgeObserver
EmergencyBridgeObserver --Notify("FIRE_DETECTED")--> EmergencyMediator --> Alarm, DoorController, VentilationController

## Cấu trúc thư mục
CampusSafetyHub/
  CampusSafetyHub.csproj
  Program.cs
  Sensors/ISensor.cs, LegacySmokeDetector.cs, SmokeSensorAdapter.cs
  Alerts/AlertEvent.cs, IAlertObserver.cs, SensorMonitor.cs
  Observers/DashboardObserver.cs, EmailAlertObserver.cs, AuditLogObserver.cs, EmergencyBridgeObserver.cs
  Mediator/IEmergencyMediator.cs, EmergencyMediator.cs, Alarm.cs, DoorController.cs, VentilationController.cs
docs/ClassDiagram.mmd, ClassDiagram.png, run-output.txt
README.md
.gitignore

## Cách chạy
dotnet build CampusSafetyHub\CampusSafetyHub.csproj
dotnet run --project CampusSafetyHub\CampusSafetyHub.csproj

## Quy ước mức độ (SensorMonitor quyết định)
- giá trị < 50 -> không cảnh báo
- 50 <= v < 70 -> WARNING
- v >= 70 -> CRITICAL

## Các pattern sử dụng
1. Adapter: SmokeSensorAdapter bọc dùng (composition) LegacySmokeDetector, biến đổi giao diện LegacySmokeDetector sang ISensor. ReadValue = GetSmokeLevel() / 10.0 (0..1000 -> 0..100%), GetUnit = "% smoke".
2. Observer: SensorMonitor là subject, các observer (Dashboard, Email, Audit, EmergencyBridge) đăng ký qua IAlertObserver. SensorMonitor không biết lớp observer cụ thể.
3. Mediator: EmergencyMediator điều phối Alarm, DoorController, VentilationController. Các thiết bị không tham chiếu lẫn nhau, chỉ mediator biết cả ba.

## 4 câu hỏi thiết kế (xem chi tiết trong báo cáo)
1. Adapter nên kế thừa hay chứa LegacySmokeDetector? -> Chứa (composition), vì LegacySmokeDetector là sealed nên không kế thừa được; ưu tiên composition hơn kế thừa.
2. SensorMonitor có nên biết EmailAlertObserver cụ thể không? -> Không; chỉ phụ thuộc IAlertObserver -> đảm bảo nguyên tắc Open/Closed.
3. Alarm có cần tham chiếu DoorController không? -> Không; mọi sự phối hợp đều thông qua EmergencyMediator -> giảm coupling.
4. Ai quyết định giá trị vượt ngưỡng? -> SensorMonitor, vì ngưỡng cảnh báo là chính sách giám sát của hệ thống, không thuộc trách nhiệm của sensor hay observer.

## Kết quả build & chạy
- dotnet build: 0 warning, 0 error.
- dotnet run: 31 test PASS, 0 FAIL.
- Output lưu tại docs/run-output.txt.
