using Terminals.Data;

namespace Tests.FilePersisted
{
    internal class GroupsTestLab
    {
        private const string GROUP_NAME = "TestGroup";

        private const string GROUP_NAME2 = GROUP_NAME + "2";

        internal IGroup Parent { get; set; }

        internal IGroup Child { get; set; }

        internal int AddedReported { get; private set; }

        internal int RemovedReported { get; private set; }

        internal int UpdateReported { get; private set; }

        private readonly IPersistence persistence;

        public GroupsTestLab(IPersistence persistence)
        {
            this.persistence = persistence;
        }

        internal void RegisterDispatcherEvent()
        {
            this.persistence.Dispatcher.GroupsChanged += new GroupsChangedEventHandler(this.Dispatcher_GroupsChanged);
        }

        private void Dispatcher_GroupsChanged(GroupsChangedArgs args)
        {
            if (args.Added.Count == 1)
                this.AddedReported++;
            if (args.Removed.Count == 1)
                this.RemovedReported++;
            if (args.Updated.Count == 1)
                this.UpdateReported++;
        }

        internal void DeleteParent()
        {
            this.CreateChildWithParent();
            this.RegisterDispatcherEvent();
            this.persistence.Groups.Delete(this.Parent);
        }

        internal void CreateChildWithParent()
        {
            this.Parent = this.AddNewGroup(GROUP_NAME);
            this.Child = this.AddNewGroup(GROUP_NAME2);
            this.Child.Parent = this.Parent;
            this.persistence.Groups.Update(this.Child);
        }

        private IGroup AddNewGroup(string groupName)
        {
            IGroup newGroup = this.persistence.Factory.CreateGroup(groupName);
            this.persistence.Groups.Add(newGroup);
            return newGroup;
        }
    }
}