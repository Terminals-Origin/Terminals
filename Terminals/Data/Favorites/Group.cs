using System;

namespace Terminals.Data
{
    [Serializable]
    public class Group
    {
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of parent group. 
        /// This allwes deep tree structure of favorite groups.
        /// Empty Guid by default.
        /// </summary>
        public Guid Parent { get; set; }

        public string Name { get; set; }

        public Group()
        {
            this.Parent = Guid.Empty;
            this.Id = Guid.NewGuid();
        }
    }
}
