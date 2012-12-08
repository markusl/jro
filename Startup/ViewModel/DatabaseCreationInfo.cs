using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.IO;
using System.Diagnostics;

namespace Startup
{
    /// <summary>
    /// The data model used in new database creation.
    /// </summary>
    class DatabaseCreationInfo : IDataErrorInfo
    {
        public string DatabaseName { get; set; }
        public string DatabasePassword { get; set; }
        public string DatabasePath { get; set; }

        public string FullPathToDatabase
        {
            get { return Path.Combine(DatabasePath, DatabaseName + ".db"); }
        }

        public string Error
        {
            get { return null; }
        }

        /// <summary>
        /// Gets the error message for the property with the given name.
        /// </summary>
        public string this[string propertyName]
        {
            get { return this.GetValidationError(propertyName); }
        }

        private string GetValidationError(string propertyName)
        {
            switch (propertyName)
            {
                case "DatabaseName":
                    return ValidateDatabaseName();
                case "DatabasePassword":
                    return this.ValidatePassword();
                case "DatabasePath":
                    return this.ValidatePath();
                default:
                    Debug.Assert(false, "Should not be reached");
                    break;
            }

            return null;
        }

        private string ValidatePath()
        {
            if (String.IsNullOrEmpty(DatabasePath))
            {
                return "Valitse hakemisto";
            }

            if (!Directory.Exists(DatabasePath))
            {
                return "Hakemisto ei ole olemassa";
            }

            if (File.Exists(FullPathToDatabase))
            {
                return "Tietokanta on jo olemassa tällä nimellä.";
            }
            return null;
        }

        private string ValidateDatabaseName()
        {
            if (String.IsNullOrEmpty(DatabaseName) || DatabaseName.Length < 4)
            {
                return "Anna yhdistyksen nimi";
            }
            if (DatabaseName.Contains(' '))
            {
                return "Yhdistyksen nimi ei saa sisältää välilyöntejä";
            }
            return null;
        }

        private string ValidatePassword()
        {
            if (String.IsNullOrEmpty(DatabasePassword) || DatabasePassword.Length < 6)
            {
                return "Anna vähintään 6-merkkinen salasana.";
            }
            return null;
        }
    }
}
