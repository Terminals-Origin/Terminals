using Terminals.Integration.Export;

namespace Terminals.Connections
{
    public interface IOptionsExporterFactory
    {
        ITerminalsOptionsExport CreateOptionsExporter();
    }
}