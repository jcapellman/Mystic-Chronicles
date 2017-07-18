using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace MysticChronicles.Engine.DB.Tables
{
    public class BaseTable
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public DateTime Modified { get; set; }

        public DateTime Created { get; set; }

        public bool Active { get; set; }
    }
}