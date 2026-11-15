namespace SC_HW_1_HSEkat.Services
{
    public record CheckResult(bool Accepted, string Reason)
    {
        public static CheckResult Accept(string reason = "Vehicle inspection complete successfully") => new(true, reason);
        public static CheckResult Reject(string reason) => new(false, reason);
    }
}
