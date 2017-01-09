using System;

namespace Changelogger.Shared.Export
{
    public static class LogExporterFactory
    {
        public static ILogExporter GetLogExporter(string identifier)
        {
            if (identifier.ToLowerInvariant() == "markdown")
                return new MarkdownExporter();

            throw new NotSupportedException();
        }
    }
}
