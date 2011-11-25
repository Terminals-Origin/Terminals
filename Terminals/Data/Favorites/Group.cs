using System;
using Terminals.Configuration;

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

        private string name;
        public string Name
        {
            get { return name; }
            set
            {
                if (Settings.AutoCaseTags)
                    name = Settings.ToTitleCase(value);
                name = value;
            }
        }

        public Group()
        {
            this.Parent = Guid.Empty;
            this.Id = Guid.NewGuid();
        }

        public override string ToString()
        {
            string parent = "Root";
            if (this.Parent != Guid.Empty)
                parent = this.Parent.ToString();

            return String.Format("Group:Name={0},Id={1},Parent={2}", this.name, this.Id, parent);
        }
    }
}
