using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuzeumInz
{
    public class History
    {
        private int historyId;
        public string tableName { get; set; }
        public int recordId { get; set; }
        public string operation { get; set; }
        public string changed_by { get; set; }
        public DateTime changed_at  { get; set; }
        public string old_values { get; set; }
        public string new_values { get; set; }

        public History(int historyId, string tableName, int recordId, string operation, string changed_by, DateTime changed_at, string old_values, string new_values)
    {
        this.historyId = historyId;
        this.tableName = tableName;
        this.recordId = recordId;
        this.operation = operation;
        this.changed_by = changed_by;
        this.changed_at = changed_at;
        this.old_values = old_values;
        this.new_values = new_values;
    }
}
}
