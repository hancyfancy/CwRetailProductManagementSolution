using CwRetail.Data.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CwRetail.Data.Models
{
    public class ProductAudit
    {
        public long ProductAuditId { get; set; }

        public ProductAuditEventTypeEnum EventType { get; set; }

        public string ObjJson { get; set; }

        public DateTime AuditDateTime { get; set; }
    }
}
