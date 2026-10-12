using System;
using System.Data;
using System.Globalization;
using System.Text;

namespace DVLDBusinessLogic2
{
    public static class TableFilter
    {
        public static string EscapeLike(string value)
        {
            var result = new StringBuilder();
            foreach (char c in value ?? string.Empty)
            {
                switch (c)
                {
                    case '\'': result.Append("''"); break;
                    case '[': result.Append("[[]"); break;
                    case ']': result.Append("[]]"); break;
                    case '%': result.Append("[%]"); break;
                    case '*': result.Append("[*]"); break;
                    default: result.Append(c); break;
                }
            }
            return result.ToString();
        }

        public static DataView Text(DataTable table, string column, string value)
        {
            CheckColumn(table, column);
            return new DataView(table) { RowFilter = "[" + column + "] LIKE '%" + EscapeLike(value) + "%'" };
        }

        public static DataView Number(DataTable table, string column, string value)
        {
            CheckColumn(table, column);
            var view = new DataView(table);
            if (string.IsNullOrWhiteSpace(value)) return view;
            if (table.Columns[column].DataType == typeof(bool))
            {
                bool flag;
                view.RowFilter = bool.TryParse(value, out flag) ? "[" + column + "] = " + (flag ? "true" : "false") : "1 = 0";
                return view;
            }
            long number;
            view.RowFilter = long.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out number)
                ? "[" + column + "] = " + number.ToString(CultureInfo.InvariantCulture) : "1 = 0";
            return view;
        }

        private static void CheckColumn(DataTable table, string column)
        {
            if (table == null || string.IsNullOrEmpty(column) || !table.Columns.Contains(column) ||
                column.IndexOfAny(new[] { '[', ']', '\\' }) >= 0)
                throw new ArgumentException("Unknown filter column.", nameof(column));
        }
    }
}
