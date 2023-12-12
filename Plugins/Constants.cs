using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStore.SQLite
{
    public class Constants
    {
        // Config for Term DB
        public const string DatabaseFileName = "TermSQLite.db3";
        public static string DataPath => Path.Combine(FileSystem.Current.AppDataDirectory, DatabaseFileName);
    }
}
