using System;

namespace Terminals.Data
{
    [Serializable]
    public class Favorite
    {
        public string Name { get; set; }
        public Guid Id { get; set; }
        public string Groups { get; set; }

        public Favorite()
        {
            this.Id = Guid.Empty;
        }
    }
}
