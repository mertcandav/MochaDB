using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;

namespace MochaDB {
    /// <summary>
    /// Provider for MochaDB connections.
    /// </summary>
    public sealed class MochaProvider:IMochaProvider {
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

        #region Static
        
        /// <summary>
        /// Return attribute by name.
        /// </summary>
        /// <param name="name">Name of attribute.</param>
        /// <param name="connectionString">Connection string for connect to MochaDb database.</param>
        public static MochaProviderAttribute GetAttribute(string name,string connectionString) {
            if(string.IsNullOrWhiteSpace(name))
                throw new Exception("Attribute name is can not empty or white space!");

            if(string.IsNullOrWhiteSpace(connectionString))
                throw new Exception("Connection string is can not empty or white space!");

            Regex rgx = new Regex(name+".?=.?",RegexOptions.IgnoreCase|RegexOptions.CultureInvariant);

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
            string attributeValue = sresult[(sresult.IndexOf('=')+1)..^0];
            MochaProviderAttribute attribute = new MochaProviderAttribute();
            attribute.Value= attributeValue==null ? "" :
                string.Equals(attribute.Name,"password",StringComparison.InvariantCultureIgnoreCase) ?
                attributeValue : attributeValue.TrimStart().TrimEnd();
            attribute.Name=name.TrimStart().TrimEnd();
            return attribute;
        }

        /// <summary>
        /// Return connection string by attributes.
        /// </summary>
        /// <param name="attributes">Attribtutes of connection string.</param>
        public static string GetConnectionString(IEnumerable<MochaProviderAttribute> attributes) {
            string cstring = string.Empty;
            for(int index = 0; index < attributes.Count(); index++) {
                MochaProviderAttribute attribtue = attributes.ElementAt(index);
                cstring+=attribtue.Name+"="+attribtue.Value+";";
            }
            return cstring;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Enable readonly property. It cannot be undone!
        /// </summary>
        public void EnableReadonly() {
            Readonly=true;
        }

        /// <summary>
        /// Return attribute by name.
        /// </summary>
        /// <param name="name">Name of attribute.</param>
        public MochaProviderAttribute GetAttribute(string name) =>
            GetAttribute(name,ConnectionString);

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
    public sealed class MochaProviderAttribute:IMochaProviderAttribute {
        #region Fields

        internal static Regex booleanAttributesRegex = new Regex(@"
AutoConnect",RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);

        private string
            name,
            value;

        #endregion

        #region Constructors

        /// <summary>
        /// Create new MochaProviderAttribute.
        /// </summary>
        internal MochaProviderAttribute() {
            name=string.Empty;
            value=string.Empty;
        }

        /// <summary>
        /// Create new MochaProviderAttribute.
        /// </summary>
        /// <param name="name">Name of attribute.</param>
        /// <param name="value">Value of attribute.</param>
        public MochaProviderAttribute(string name,string value) {
            this.value=value;
            Name=name;
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Check value by name.
        /// </summary>
        internal void CheckValue() {
            if(string.IsNullOrWhiteSpace(value)) {
                if(Name.Equals("Path",StringComparison.InvariantCultureIgnoreCase))
                    throw new Exception("File path cannot be empty!");
                if(booleanAttributesRegex.IsMatch(Name))
                    value="False";
            } else {
                if(booleanAttributesRegex.IsMatch(Name) && (
                    !value.Equals(bool.TrueString,StringComparison.InvariantCultureIgnoreCase) ||
                    !value.Equals(bool.FalseString,StringComparison.InvariantCultureIgnoreCase)))
                    value="False";
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Name.
        /// </summary>
        public string Name {
            get =>
                name;
            set {
                if(string.IsNullOrWhiteSpace(value))
                    throw new Exception("Attribute name is can not empty or white space!");

                if(value==name)
                    return;

                name=value;
                CheckValue();
            }
        }

        /// <summary>
        /// Value.
        /// </summary>
        public string Value {
            get =>
                value;
            set {
                this.value=value;
                CheckValue();
            }
        }

        #endregion
    }
}