using System;
using System.ComponentModel.DataAnnotations;

namespace Todo.Data
{
    public class Transaction
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required(ErrorMessage = "Please provide the transaction name.")]
        public string TaskName { get; set; }

        public string Amount { get; set; }

        public string Notes { get; set; }

        public string Tag { get; set; }

        [Required(ErrorMessage = "Please provide a due date.")]
        public DateTime DueDate { get; set; } = DateTime.Today;

        public Guid CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public bool IsDone { get; set; }
    }

    public class Debt
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string TaskName { get; set; }

        [Required(ErrorMessage = "Please provide the debt source.")]
        public string SourceofDebt { get; set; }

        public string DebtAmount { get; set; }



        [Required(ErrorMessage = "Please provide a due date.")]
        public DateTime DueDate { get; set; } = DateTime.Today;

        public Guid CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public bool IsPaid { get; set; }

        public bool IsDone { get; set; }
    }
}
