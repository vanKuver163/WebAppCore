using Core.ValidationAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Core.Models
{
    public class Ticket
    {
        public int? TicketId { get; set; }
        [Required]
        public int? ProjectId { get; set; }
        [Required]
        [StringLength(100)]
        public string Title { get; set; }
        public string Description { get; set; }

        [StringLength(50)]
        public string Owner { get; set; }

        [Ticket_EnsureReportDatePresent]
        public DateTime? ReportDate { get; set; }

        [Ticket_EnsureDueDatePresent]
        [Ticket_EnsureFutureDueDateOnCreationAttribite]
        [Ticket_EnsureDueDateAfterReportDate]
        public DateTime? DueDate { get; set; }

        public Project Project { get; set; }

        public bool ValidateFutureDueDate()
        {
            if (TicketId.HasValue) return true;
            if (!DueDate.HasValue) return true;
            return (DueDate.Value > DateTime.Now);
        }

        public bool ValidateReportDatePresence()
        {
            if (string.IsNullOrWhiteSpace(Owner)) return true;
            return ReportDate.HasValue;
        }

        public bool ValidateDuetDatePresence()
        {
            if (string.IsNullOrWhiteSpace(Owner)) return true;
            return DueDate.HasValue;
        }
        public bool ValidateDuetDateAfterReportDate()
        {
            if (!DueDate.HasValue || !ReportDate.HasValue) return true;
            return DueDate.Value.Date >= ReportDate.Value.Date;
        }
    }
}
