namespace VSW.Lib.Global
{
    public class Permissions
    {
        public Permissions()
        {
        }

        public Permissions(int access)
        {
            Access = access;
        }

        internal void OrAccess(int access)
        {
            var newPermissions = new Permissions(access);

            View = View || newPermissions.View;
            Add = Add || newPermissions.Add;
            Edit = Edit || newPermissions.Edit;
            Delete = Delete || newPermissions.Delete;
            Approve = Approve || newPermissions.Approve;
        }

        public int Access { get; internal set; }

        public bool Control
        {
            get => (Access & 16) == 16;
            internal set
            {
                if (Approve && !value) Access -= 16;
                if (!Approve && value) Access += 16;
            }
        }

        public bool Approve
        {
            get => (Access & 16) == 16;
            internal set
            {
                if (Approve && !value) Access -= 16;
                if (!Approve && value) Access += 16;
            }
        }

        public bool Delete
        {
            get => (Access & 8) == 8;
            internal set
            {
                if (Delete && !value) Access -= 8;
                if (!Delete && value) Access += 8;
            }
        }

        public bool Edit
        {
            get => (Access & 4) == 4;
            internal set
            {
                if (Edit && !value) Access -= 4;
                if (!Edit && value) Access += 4;
            }
        }

        public bool Add
        {
            get => (Access & 2) == 2;
            internal set
            {
                if (Add && !value) Access -= 2;
                if (!Add && value) Access += 2;
            }
        }

        public bool View
        {
            get => (Access & 1) == 1;
            internal set
            {
                if (View && !value) Access--;
                if (!View && value) Access++;
            }
        }

        public bool Full
        {
            get => View && Add && Edit && Delete && Approve;
            internal set => View = Add = Edit = Delete = Approve = value;
        }

        public bool Any => Access > 0;
    }
}