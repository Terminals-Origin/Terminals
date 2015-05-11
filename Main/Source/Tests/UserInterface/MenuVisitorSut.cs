using System.Windows.Forms;
using Moq;
using Terminals.Connections;

namespace Tests.UserInterface
{
    internal class MenuVisitorSut
    {
        private readonly Mock<IConnection> mockConnection;

        internal Mock<ICurrenctConnectionProvider> MockProvider { get; set; }

        internal ToolStrip Toolbar { get; set; }

        internal MenuVisitorSut()
        {
            this.mockConnection = new Mock<IConnection>();
            this.MockProvider = new Mock<ICurrenctConnectionProvider>();
            this.MockProvider.SetupGet(p => p.CurrentConnection)
                .Returns(() => this.mockConnection.Object); // other connection, than null
            this.Toolbar = new ToolStrip();
        }
    }
}