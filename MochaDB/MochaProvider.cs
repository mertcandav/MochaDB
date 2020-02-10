using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;

namespace MochaDB {
    /// <summary>
    /// Provider for MochaDB connections.
    /// </summary>
    public sealed class MochaProvider {
        #region Fields

        private string connectionString;

        #endregion

        #region Constructors

        /// <summary>
        /// Create new MochaProvider.
        /// </summary>
        /// <param name="connectionString">Connection string for connect to MochaDb database.</param>
        public MochaProvider(string connectionString) {
            ConnectionString=connectionString;
        }

        #endregion

        #region Static Methods
        
        /// <summary>
        /// Return attribute by name.
        /// </summary>
        /// <param name="name">Name of attribute.</param>
        /// <param name="connectionString">Connection string for connect to MochaDb database.</param>
        internal static MochaProviderAttribute GetAttribute(string name,string connectionString) {
            if(string.IsNullOrWhiteSpace(name))
                throw new Exception("Attribute name is can not empty or white space!");

            if(string.IsNullOrWhiteSpace(connectionString))
                throw new Exception("Connection string is can not empty or white space!");

            Regex rgx = new Regex(name+".?=.+",RegexOptions.IgnoreCase|RegexOptions.CultureInvariant);

            IEnumerable<string> result =
                from rvalue in
                    from value in connectionString.Split(';')
                    select value.TrimStart().TrimEnd()
                where rgx.IsMatch(rvalue)
                select rvalue;

            int count = result.Count();
            if(count==0)
                return null;

            if(count>1)
                throw new Exception("An attribute can only be specified once!");

            string sresult = result.ElementAt(0);
            MochaProviderAttribute attribute = new MochaProviderAttribute();
            attribute.Name=name.TrimStart().TrimEnd();
            attribute.Value=sresult[(sresult.IndexOf('=')+1)..^0].TrimStart().TrimEnd();
            return attribute;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Enable readonly property. It cannot be undone!
        /// </summary>
        public void EnableReadonly() {
            Readonly=true;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Connection string of provider.
        /// </summary>
        public string ConnectionString {
            get =>
                connectionString;
            set {
                if(Readonly)
                    throw new Exception("This provider can only be read!");

                if(string.IsNullOrWhiteSpace(value))
                    throw new Exception("Connection string is can not empty or white space!");

                if(value==connectionString)
                    return;

                MochaProviderAttribute
                    pathAttribute = GetAttribute("Path",value),
                    passwordAttribute = GetAttribute("Password",value);

                Path=pathAttribute !=null ? pathAttribute.Value : throw new Exception("'Path' attribute is not defined!");

                if(!MochaDatabase.IsMochaDB(Path))
                    throw new Exception("The file shown is not a MochaDB database file!");

                Password=passwordAttribute!=null ? passwordAttribute.Value : string.Empty;

                connectionString=value;
            }
        }

        /// <summary>
        /// Path of database.
        /// </summary>
        public string Path { get; internal set; }

        /// <summary>
        /// Password of database.
        /// </summary>
        public string Password { get; internal set; }

        /// <summary>
        /// Is readonly.
        /// </summary>
        public bool Readonly { get; internal set; }

        #endregion
    }

    /// <summary>
    /// Attributes for MochaProviders.
    /// </summary>
    public sealed class MochaProviderAttribute {
        #region Constructors

        /// <summary>
        /// Create new MochaProviderAttribute.
        /// </summary>
        internal MochaProviderAttribute() {
            Name=string.Empty;
            Value=string.Empty;
        }

        /// <summary>
        /// Create new MochaProviderAttribute.
        /// </summary>
        /// <param name="name">Name of attribute.</param>
        /// <param name="value">Value of attribute.</param>
        public MochaProviderAttribute(string name,string value) {
            if(string.IsNullOrWhiteSpace(name))
                throw new Exception("Attribute name is can not empty or white space!");

            Name=name;
            Value=value;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Value.
        /// </summary>
        public string Value { get; set; }

        #endregion
    }
}