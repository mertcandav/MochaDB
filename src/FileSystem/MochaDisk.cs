using System;
using MochaDB.engine;

namespace MochaDB.FileSystem {
    /// <summary>
    /// Disk for MochaDB file system.
    /// </summary>
    public class MochaDisk:IMochaDisk {
        #region Fields

        private string name;
        private string root;

        #endregion

        #region Constructors

        /// <summary>
        /// Create new MochaDisk.
        /// </summary>
        /// <param name="root">Root of disk.</param>
        /// <param name="name">Name of disk.</param>
        public MochaDisk(string root,string name) {
            Files= new MochaFileCollection();
            Directories=new MochaDirectoryCollection();
            Root=root;
            Name=name;
            Description=string.Empty;
        }

        #endregion

        #region Operators

        public static explicit operator string(MochaDisk value) =>
            value.ToString();

        #endregion

        #region Events

        /// <summary>
        /// This happens after name changed.
        /// </summary>
        public event EventHandler<EventArgs> NameChanged;
        private void OnNameChanged(object sender,EventArgs e) {
            //Invoke.
            NameChanged?.Invoke(this,new EventArgs());
        }

        /// <summary>
        /// This happens after root changed.
        /// </summary>
        public event EventHandler<EventArgs> RootChanged;
        private void OnRootChanged(object sender,EventArgs e) {
            //Invoke.
            RootChanged?.Invoke(this,new EventArgs());
        }

        #endregion

        #region Overrides

        /// <summary>
        /// Returns <see cref="Name"/>.
        /// </summary>
        public override string ToString() {
            return Name;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Root name.
        /// </summary>
        public string Root {
            get =>
                root;
            set {
                value.Trim();

                if(value==root)
                    return;

                if(string.IsNullOrWhiteSpace(value))
                    throw new MochaException("Root is cannot null or whitespace!");

                Engine_NAMES.DiskRootCheckThrow(value);

                root=value;
                OnRootChanged(this,new EventArgs());
            }
        }

        /// <summary>
        /// Disk name.
        /// </summary>
        public string Name {
            get =>
                name;
            set {
                value=value.Trim();
                if(string.IsNullOrWhiteSpace(value))
                    throw new MochaException("Name is cannot null or whitespace!");

                Engine_NAMES.DiskCheckThrow(value);

                if(value==name)
                    return;

                name=value;
                OnNameChanged(this,new EventArgs());
            }
        }

        /// <summary>
        /// Disk description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Directories of disk.
        /// </summary>
        public MochaDirectoryCollection Directories { get; }

        /// <summary>
        /// Files of disk.
        /// </summary>
        public MochaFileCollection Files { get; }

        #endregion
    }
}
