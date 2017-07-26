namespace MysticChronicles.Engine.DB.Tables
{
    public class GameVariables : BaseTable
    {
        public int GameID { get; set; }

        public string VarName { get; set; }

        public string VarValue { get; set; }
    }
}