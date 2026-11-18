# Campus Safety Hub - Mau thiet ke Adapter + Observer + Mediator

## Thong tin sinh vien
- Ma sinh vien: <MSSV>
- Ho ten: <HỌ TÊN>
- GitHub repo: <URL repo hoac de trong neu chua co>

## Mo ta
Chuong trinh (.NET 8, C# 12, nullable enabled) mo phong trung hop giams sat an toan phong may. Khong GUI, khong CSDL, khong mang, khong gui email thong. Trong tam la thiet ke doi tuong va luong tuong thong.

Luong tong quat:
LegacySmokeDetector -> SmokeSensorAdapter (ISensor) -> SensorMonitor --AlertEvent-->
  DashboardObserver, EmailAlertObserver, AuditLogObserver, EmergencyBridgeObserver
EmergencyBridgeObserver --Notify("FIRE_DETECTED")--> EmergencyMediator --> Alarm, DoorController, VentilationController

## Cau truc thu muc
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

## Cach chay
dotnet build CampusSafetyHub\CampusSafetyHub.csproj
dotnet run --project CampusSafetyHub\CampusSafetyHub.csproj

## Quy uoc muc do (SensorMonitor quyet duyen)
- gia tri < 50 -> khong canh bao
- 50 <= v < 70 -> WARNING
- v >= 70 -> CRITICAL

## Cac pattern su dung
1. Adapter: SmokeSensorAdapter bao dung (composition) LegacySmokeDetector, bien doi giao diện LegacySmokeDetector sang ISensor. ReadValue = GetSmokeLevel() / 10.0 (0..1000 -> 0..100%), GetUnit = "% smoke".
2. Observer: SensorMonitor la subject, cac observer (Dashboard, Email, Audit, EmergencyBridge) dang ky qua IAlertObserver. SensorMonitor khong biet observer cu the.
3. Mediator: EmergencyMediator dieu phoi Alarm, DoorController, VentilationController. Cac thiet bi khong tham chieu nhau, chi mediator biet ca ba.

## 4 cau hoi thiet ke (xem chi tiet trong bao cao)
1. Adapter nen ke thua hay chua LegacySmokeDetector? -> Chua (composition), vi LegacySmokeDetector la sealed khong ke thua duoc; ưu tiên composition hon ke thua.
2. SensorMonitor co nen biet EmailAlertObserver cu the khong? -> Khong; chi phu thuoc IAlertObserver -> Open/Closed.
3. Alarm co can tham chieu DoorController khong? -> Khong; moi phep hop qua EmergencyMediator -> giam coupling.
4. Ai quyet duyen gia tri vuot ngưỡng? -> SensorMonitor, vi ngưỡng la chinh pham giamsat, khong phai cua sensor hay observer.

## Ket qua build & chay
- dotnet build: 0 warning, 0 error.
- dotnet run: 31 test PASS, 0 FAIL.
- Output luu tai docs/run-output.txt.

## De bai (can tien hanh)
- Dien MSSV va ho ten vao header moi file .cs vao README/bao cao.
- Neu co remote GitHub, push len repo va cap nhat link GitHub vao header/bao cao.
- Tao bao cao nop bai: BaoCao_<MSSV>.docx (xem yeu cau trong de bai).
