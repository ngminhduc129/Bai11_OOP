/****************/
/* Ma sinh vien: <MSSV> */
/* Ho ten: <HO TEN> */
/****************/

using CampusSafetyHub.Alerts;
using CampusSafetyHub.Mediator;
using CampusSafetyHub.Observers;
using CampusSafetyHub.Sensors;
using System.IO;

namespace CampusSafetyHub;

public static class Program
{
    private static int passed;
    private static int failed;

    public static void Main()
    {
        Run();
        Console.WriteLine();
        Console.WriteLine("================ TONG KET ================");
        Console.WriteLine($"PASS: {passed} | FAIL: {failed}");
    }

    private static void Section(string title)
    {
        Console.WriteLine();
        Console.WriteLine($"================ {title} ================");
    }

    private static void Test(string name, bool ok, string expected, string actual)
    {
        if (ok)
        {
            passed++;
            Console.WriteLine($"[PASS] {name}");
        }
        else
        {
            failed++;
            Console.WriteLine($"[FAIL] {name} | mong doi: {expected} | thuc te: {actual}");
        }
    }

    private static string Capture(Action action)
    {
        var sw = new StringWriter();
        var old = Console.Out;
        Console.SetOut(sw);
        try
        {
            action();
        }
        finally
        {
            Console.SetOut(old);
        }
        return sw.ToString();
    }

    private static int CountLines(string text, string marker)
    {
        return text.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries)
            .Count(line => line.Contains(marker));
    }

    private sealed class SelfDetachingObserver : IAlertObserver
    {
        private readonly SensorMonitor monitor;
        public int TimesUpdated { get; private set; }
        public SelfDetachingObserver(SensorMonitor m) => monitor = m;
        public void Update(AlertEvent alertEvent)
        {
            TimesUpdated++;
            monitor.Detach(this);
        }
    }

    private sealed class ThrowingObserver : IAlertObserver
    {
        public int TimesUpdated { get; private set; }
        public void Update(AlertEvent alertEvent)
        {
            TimesUpdated++;
            throw new InvalidOperationException("observer loi gia lap");
        }
    }

    private static void Run()
    {

        // ================= 1. Adapter =================
        Section("1. Adapter");
        var legacy735 = new LegacySmokeDetector(735);
        var adapter = new SmokeSensorAdapter(legacy735);
        Test("1a ReadValue(735) == 73.5", adapter.ReadValue() == 73.5, "73.5", adapter.ReadValue().ToString());
        Test("1b GetUnit == \"% smoke\"", adapter.GetUnit() == "% smoke", "% smoke", adapter.GetUnit());
        legacy735.SetSmokeLevel(100);
        Test("1c sau SetSmokeLevel(100) ReadValue == 10.0 (khong copy state)", adapter.ReadValue() == 10.0, "10.0", adapter.ReadValue().ToString());
        try
        {
            new SmokeSensorAdapter(null!);
            Test("1d constructor null nem ArgumentNullException", false, "nem ArgumentNullException", "khong nem");
        }
        catch (ArgumentNullException)
        {
            Test("1d constructor null nem ArgumentNullException", true, "nem ArgumentNullException", "da nem");
        }

        // ================= 2. Ngưỡng =================
        Section("2. Nguong canh bao");
        var mon2 = new SensorMonitor();
        var audit2 = new AuditLogObserver();
        mon2.Attach(audit2);
        int baseCount = audit2.Logs.Count;

        mon2.CheckSensor("KHOI", new SmokeSensorAdapter(new LegacySmokeDetector(0)));   // 0.0  -> khong canh bao
        mon2.CheckSensor("KHOI", new SmokeSensorAdapter(new LegacySmokeDetector(499))); // 49.9 -> khong canh bao
        Test("2a 0 va 499 khong canh bao", audit2.Logs.Count == baseCount, "khong thay doi", audit2.Logs.Count.ToString());

        mon2.CheckSensor("KHOI", new SmokeSensorAdapter(new LegacySmokeDetector(500))); // 50.0 -> WARNING
        Test("2b 500 -> WARNING", audit2.Logs.Count == baseCount + 1 && audit2.Logs[^1].Contains("WARNING"), "co WARNING", audit2.Logs.Count.ToString());

        mon2.CheckSensor("KHOI", new SmokeSensorAdapter(new LegacySmokeDetector(699))); // 69.9 -> WARNING
        Test("2c 699 -> WARNING", audit2.Logs[^1].Contains("WARNING"), "WARNING", audit2.Logs[^1]);

        mon2.CheckSensor("KHOI", new SmokeSensorAdapter(new LegacySmokeDetector(700))); // 70.0 -> CRITICAL
        Test("2d 700 -> CRITICAL", audit2.Logs[^1].Contains("CRITICAL"), "CRITICAL", audit2.Logs[^1]);

        mon2.CheckSensor("KHOI", new SmokeSensorAdapter(new LegacySmokeDetector(1000))); // 100.0 -> CRITICAL
        Test("2e 1000 -> CRITICAL", audit2.Logs[^1].Contains("CRITICAL"), "CRITICAL", audit2.Logs[^1]);

        // ================= 3. Attach trung =================
        Section("3. Attach trung");
        var mon3 = new SensorMonitor();
        var dash3 = new DashboardObserver();
        mon3.Attach(dash3);
        mon3.Attach(dash3); // dang ky trung
        string out3 = Capture(() => mon3.CheckSensor("S3", new SmokeSensorAdapter(new LegacySmokeDetector(800))));
        int n3 = CountLines(out3, "[DASHBOARD]");
        Test("3a dashboard chi nhan 1 lan moi canh bao", n3 == 1, "1", n3.ToString());

        // ================= 4. Detach =================
        Section("4. Detach");
        var mon4 = new SensorMonitor();
        var dash4 = new DashboardObserver();
        mon4.Attach(dash4);
        mon4.Detach(dash4);
        string out4 = Capture(() => mon4.CheckSensor("S4", new SmokeSensorAdapter(new LegacySmokeDetector(800))));
        Test("4a sau Detach dashboard khong con nhan", !out4.Contains("[DASHBOARD]"), "khong co", out4);

        // ================= 5. Tu Detach =================
        Section("5. Observer tu Detach trong Update");
        var mon5 = new SensorMonitor();
        var self5 = new SelfDetachingObserver(mon5);
        var other5 = new DashboardObserver();
        mon5.Attach(self5);
        mon5.Attach(other5);
        string out5 = Capture(() => mon5.CheckSensor("S5", new SmokeSensorAdapter(new LegacySmokeDetector(800))));
        Test("5a tu detach khong lam loi vong lap (selfUpdate==1)", self5.TimesUpdated == 1, "1", self5.TimesUpdated.ToString());
        Test("5b observer khac van nhan", out5.Contains("[DASHBOARD]"), "co", out5);
        string out5b = Capture(() => mon5.CheckSensor("S5", new SmokeSensorAdapter(new LegacySmokeDetector(800))));
        Test("5c sau khi detach self khong nhan nua (selfUpdate van 1)", self5.TimesUpdated == 1, "van 1", self5.TimesUpdated.ToString());

        // ================= 6. Mot observer nem exception =================
        Section("6. Mot observer nem exception");
        var mon6 = new SensorMonitor();
        var throw6 = new ThrowingObserver();
        var dash6 = new DashboardObserver();
        var audit6 = new AuditLogObserver();
        mon6.Attach(throw6);
        mon6.Attach(dash6);
        mon6.Attach(audit6);
        string out6 = Capture(() => mon6.CheckSensor("S6", new SmokeSensorAdapter(new LegacySmokeDetector(800))));
        Test("6a loi duoc log ra Console", out6.Contains("[MONITOR][ERROR]"), "co", out6);
        Test("6b dashboard van nhan dung du lieu", out6.Contains("[DASHBOARD]"), "co", out6);
        Test("6c audit van nhan dung du lieu", audit6.Logs.Count > 0, "co", audit6.Logs.Count.ToString());
        Test("6d observer nem bi goi 1 lan", throw6.TimesUpdated == 1, "1", throw6.TimesUpdated.ToString());

        // ================= 7. WARNING khong kich hoat mediator =================
        Section("7. WARNING khong kich hoat ung pho khan cap");
        var alarm7 = new Alarm();
        var door7 = new DoorController();
        var vent7 = new VentilationController();
        var med7 = new EmergencyMediator(alarm7, door7, vent7);
        var bridge7 = new EmergencyBridgeObserver(med7);
        var mon7 = new SensorMonitor();
        mon7.Attach(new DashboardObserver());
        mon7.Attach(new EmailAlertObserver("an@truong.edu"));
        mon7.Attach(new AuditLogObserver());
        mon7.Attach(bridge7);
        mon7.CheckSensor("S7", new SmokeSensorAdapter(new LegacySmokeDetector(500))); // WARNING
        Test("7a alarm chua bat", !alarm7.IsActive, "false", alarm7.IsActive.ToString());
        Test("7b cua chua mo", !door7.IsUnlocked, "false", door7.IsUnlocked.ToString());
        Test("7c quat chua chay", !vent7.IsExhaustRunning, "false", vent7.IsExhaustRunning.ToString());

        // ================= 8. CRITICAL kich hoat toan bo =================
        Section("8. CRITICAL kich hoat toan bo thiet bi");
        mon7.CheckSensor("S8", new SmokeSensorAdapter(new LegacySmokeDetector(800))); // CRITICAL
        Test("8a alarm bat", alarm7.IsActive, "true", alarm7.IsActive.ToString());
        Test("8b cua mo", door7.IsUnlocked, "true", door7.IsUnlocked.ToString());
        Test("8c quat chay", vent7.IsExhaustRunning, "true", vent7.IsExhaustRunning.ToString());

        // ================= 9. CRITICAL lien tiep khong lap =================
        Section("9. CRITICAL lien tiep 2 lan khong kich hoat lap");
        string out9 = Capture(() => mon7.CheckSensor("S9", new SmokeSensorAdapter(new LegacySmokeDetector(900))));
        int fireCount = CountLines(out9, "\u0110\u00e3 k\u00edch ho\u1ea1t \u1ef9ng ph\u00f3 CH\u00c1Y");
        Test("9a mediator chi kich hoat ung pho 1 lan (lan 2 khong in them)", fireCount == 0, "0", fireCount.ToString());
        Test("9b thiet bi van o trang thai CHAY", alarm7.IsActive && door7.IsUnlocked && vent7.IsExhaustRunning, "true", $"{alarm7.IsActive}/{door7.IsUnlocked}/{vent7.IsExhaustRunning}");

        // ================= 10. ALL_CLEAR =================
        Section("10. ALL_CLEAR khoi phuc trang thai ban dau");
        med7.Notify(null!, "ALL_CLEAR");
        Test("10a alarm tat", !alarm7.IsActive, "false", alarm7.IsActive.ToString());
        Test("10b cua khoa lai", !door7.IsUnlocked, "false", door7.IsUnlocked.ToString());
        Test("10c quat tat", !vent7.IsExhaustRunning, "false", vent7.IsExhaustRunning.ToString());

        // ================= 11. Su kiien la =================
        Section("11. Su kiien khong xac dinh bo qua");
        string out11 = Capture(() => med7.Notify(null!, "STRANGE_EVENT"));
        Test("11a chuong trinh khong crash", true, "", "");
        Test("11b su kiien la bo qua va ghi log", out11.Contains("STRANGE_EVENT"), "co", out11);

        // ================= Kich ban tich hop =================
        Section("TICH HOP: smoke tang dan 200 -> 520 -> 750 -> 100");
        var alarmI = new Alarm();
        var doorI = new DoorController();
        var ventI = new VentilationController();
        var medI = new EmergencyMediator(alarmI, doorI, ventI);
        var monI = new SensorMonitor();
        monI.Attach(new DashboardObserver());
        monI.Attach(new EmailAlertObserver("khoa@truong.edu"));
        monI.Attach(new AuditLogObserver());
        monI.Attach(new EmergencyBridgeObserver(medI));

        foreach (int level in new[] { 200, 520, 750, 100 })
        {
            Console.WriteLine($"-- Smoke = {level} ({level / 10.0:F1}%) --");
            monI.CheckSensor("PHONG_MAY", new SmokeSensorAdapter(new LegacySmokeDetector(level)));
            Console.WriteLine($"   Thiet bi: Alarm={alarmI.IsActive}, Cua={(doorI.IsUnlocked ? "MO" : "KHOA")}, Quat={(ventI.IsExhaustRunning ? "CHAY" : "TAT")}");
        }
    }
}
