namespace Terminals.Network
{
    internal class Share
    {
        public string AccessMask { get; set; }

        public string MaximumAllowed { get; set; }

        public string InstallDate { get; set; }

        public string Description { get; set; }

        public string Caption { get; set; }

        public string AllowMaximum { get; set; }

        public string Name { get; set; }

        public string Path { get; set; }

        public string Status { get; set; }

        public string TypeId { get; set; }

        public string Type
        {
            get
            {
                switch (this.TypeId)
                {
                    case "0":
                        return "Disk Drive";
                    case "1":
                        return "Print Queue";
                    case "2":
                        return "Device";
                    case "3":
                        return "IPC";
                    case "2147483648":
                        return "Disk Drive Admin";
                    case "2147483649":
                        return "Print Queue Admin";
                    case "2147483650":
                        return "Device Admin";
                    case "2147483651":
                        return "IPC Admin";
                    default:
                        return this.TypeId;
                }
            }
        }
    }
}