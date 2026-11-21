using System.Text;
using WSLCC_App;

namespace WSLCC.Core.Services;

public sealed record IssueReportContext(
    string AppVersion,
    string? WslcVersion,
    IReadOnlyList<string> TunInterfaces,
    IReadOnlyList<string> RecentAudit,
    string? AppLogTail);

public static class IssueReportBuilder
{
    public const string RepoUrl = "https://github.com/mauriciomenon/wslcc";

    private const int MaxTitleLength = 80;
    private const int MaxContextLength = 400;
    private const int MaxDetailLength = 1500;
    private const int MaxBodyLength = 4000;

    public static string BuildUrl(string context, string? detail, IssueReportContext info)
    {
        var title = $"[Bug] {Shorten(context, MaxTitleLength)}";
        var body = BuildIssueBody(context, detail, info);
        return $"{RepoUrl}/issues/new?title={Uri.EscapeDataString(title)}&body={Uri.EscapeDataString(body)}";
    }

    public static string BuildIssueBody(string context, string? detail, IssueReportContext info)
    {
        var sb = new StringBuilder();
        sb.AppendLine("## " + L.Get("Core.Report.Description"));
        sb.AppendLine();
        sb.AppendLine(Shorten(context, MaxContextLength));
        sb.AppendLine();
        sb.AppendLine("## " + L.Get("Core.Report.Steps"));
        sb.AppendLine();
        sb.AppendLine("1. ");
        sb.AppendLine("2. ");
        sb.AppendLine();
        sb.AppendLine("## " + L.Get("Core.Report.Error"));
        sb.AppendLine();
        sb.AppendLine("```");
        sb.AppendLine(string.IsNullOrWhiteSpace(detail) ? L.Get("Feedback.NoDetail") : Shorten(detail, MaxDetailLength));
        sb.AppendLine("```");
        sb.AppendLine();
        sb.AppendLine("## " + L.Get("Core.Report.Environment"));
        sb.AppendLine();
        sb.AppendLine("```");
        sb.AppendLine(BuildEnvironment(info));
        sb.AppendLine("```");
        sb.AppendLine();
        sb.AppendLine("<!-- " + L.Get("Core.Report.PasteDiagnostics") + " -->");
        var body = sb.ToString();
        if (body.Length > MaxBodyLength)
            body = body[..MaxBodyLength] + "\n\n" + L.Get("Core.Report.Truncated");
        return body;
    }

    public static string BuildDiagnostics(string context, string? detail, IssueReportContext info)
    {
        var sb = new StringBuilder();
        sb.AppendLine("### " + L.Get("Core.Report.Description"));
        sb.AppendLine(Shorten(context, MaxContextLength));
        sb.AppendLine();
        sb.AppendLine("### " + L.Get("Core.Report.Error"));
        sb.AppendLine("```");
        sb.AppendLine(string.IsNullOrWhiteSpace(detail) ? L.Get("Feedback.NoDetail") : detail);
        sb.AppendLine("```");
        sb.AppendLine();
        sb.AppendLine("### " + L.Get("Core.Report.Environment"));
        sb.AppendLine("```");
        sb.AppendLine(BuildEnvironment(info));
        sb.AppendLine("```");
        if (info.RecentAudit.Count > 0)
        {
            sb.AppendLine();
            sb.AppendLine("### " + L.Get("Core.Report.RecentActions"));
            sb.AppendLine("```");
            foreach (var line in info.RecentAudit) sb.AppendLine(line);
            sb.AppendLine("```");
        }
        if (!string.IsNullOrWhiteSpace(info.AppLogTail))
        {
            sb.AppendLine();
            sb.AppendLine("### " + L.Get("Core.Report.LogTail"));
            sb.AppendLine("```");
            sb.AppendLine(info.AppLogTail);
            sb.AppendLine("```");
        }
        return sb.ToString().TrimEnd();
    }

    private static string BuildEnvironment(IssueReportContext info)
    {
        var sb = new StringBuilder();
        sb.AppendLine($"WSLCC: {info.AppVersion}");
        sb.AppendLine($"OS: {Environment.OSVersion.VersionString} ({Environment.OSVersion.Platform})");
        sb.AppendLine($".NET: {Environment.Version}");
        sb.AppendLine($"wslc: {info.WslcVersion ?? L.Get("Settings.NotDetected")}");
        sb.AppendLine(info.TunInterfaces.Count > 0
            ? L.GetFormat("Core.Report.Tun", string.Join(", ", info.TunInterfaces))
            : L.Get("Core.Report.NoTun"));
        sb.AppendLine(L.GetFormat("Core.Report.Time", DateTimeOffset.Now));
        return sb.ToString().TrimEnd();
    }

    private static string Shorten(string text, int max)
        => string.IsNullOrEmpty(text) || text.Length <= max ? text : text[..max] + "…";
}
