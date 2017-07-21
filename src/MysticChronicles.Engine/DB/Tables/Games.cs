using System;

namespace MysticChronicles.Engine.DB.Tables
{
    public class Games : BaseTable
    {
        public DateTime SaveDate { get; set; }

        public int NumberOfCredits { get; set; }

        public string Location { get; set; }

        public TimeSpan GameTime { get; set; }
    }
}