using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MaterialDesignThemes.Wpf.Theme.ToolBar;

namespace Bookstore.WPF.Converters
{
    public class IdConverter
    {
        public static string ViewIDGenerator(string prefix, DateTime createdDate, int id)
        {
            return $"{prefix}{createdDate:ddMMyy}{id:D3}";
        }

        public static int ExtractIdFromViewID(string viewId, string prefix)
        {
            if (string.IsNullOrWhiteSpace(viewId) || string.IsNullOrWhiteSpace(prefix) || viewId.Length <= prefix.Length + 6)
            {
                return 0;
            }

            int startIndex = prefix.Length + 6;
            string idString = viewId.Substring(startIndex);

            if (int.TryParse(idString, out int id))
            {
                return id;
            }

            return 0;
        }
    }
}
