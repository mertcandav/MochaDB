using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace MochaDB.Connection {
    /// <summary>
    /// Provider for MochaDB connections.
    /// </summary>
    public class MochaProvider:IMochaProvider {
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
            string attributeValue = sresult.Substring(sresult.IndexOf('=')+1);
            MochaProviderAttribute attribute = new MochaProviderAttribute();
            attribute.name=name.TrimStart().TrimEnd();
            attribute.Value= attributeValue==null ? string.Empty :
                string.Equals(attribute.Name,"password") ?
                attributeValue : attributeValue.TrimStart().TrimEnd();
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
                cstring+=attribtue.GetProviderString();
            }
            return cstring;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Enable constant property of provider. It cannot be undone!
        /// </summary>
        public void EnableConstant() {
            Constant=true;
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
                if(Constant)
                    throw new Exception("This provider is constant, can only be read!");

                if(string.IsNullOrWhiteSpace(value))
                    throw new Exception("Connection string is can not empty or white space!");

                if(value==connectionString)
                    return;

                MochaProviderAttribute pathAttribute = GetAttribute("Path",value);

                Path=pathAttribute !=null ? pathAttribute.Value : throw new Exception("'Path' attribute is not defined!");

                MochaProviderAttribute
                    passwordAttribute = GetAttribute("Password",value),
                    readonlyAttribute = GetAttribute("Readonly",value);

                //Defaults.
                Password=passwordAttribute!=null ? passwordAttribute.Value : string.Empty;
                Readonly=readonlyAttribute!=null ? bool.Parse(readonlyAttribute.Value) : false;

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
        /// Readonly state of connection.
        /// </summary>
        public bool Readonly { get; internal set; }

        /// <summary>
        /// Is readonly provider.
        /// </summary>
        public bool Constant { get; internal set; }

        #endregion
    }

    /// <summary>
    /// Connection states for MochaDB.
    /// </summary>
    public enum MochaConnectionState {
        Connected,
        Disconnected
    }
}