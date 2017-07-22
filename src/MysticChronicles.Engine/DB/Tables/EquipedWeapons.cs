namespace MysticChronicles.Engine.DB.Tables
{
    public class EquipedWeapons : BaseTable
    {
        public int PartyMemberID { get; set; }

        public int WeaponID { get; set; }

        public int SlotNumber { get; set; }
    }
}