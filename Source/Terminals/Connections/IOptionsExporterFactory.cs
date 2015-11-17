using Terminals.Integration.Export;

namespace Terminals.Connections
{
    internal interface IOptionsExporterFactory
    {
        ITerminalsOptionsExport CreateOptionsExporter();
    }
}