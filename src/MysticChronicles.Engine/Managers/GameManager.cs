using System.Collections.Generic;
using System.Threading.Tasks;

using MysticChronicles.Engine.DB;
using MysticChronicles.Engine.DB.Tables;

namespace MysticChronicles.Engine.Managers
{
    public class GameManager
    {
        private Games _game;
        private List<PartyMembers> _partyMembers;

        public async Task<bool> LoadGame(int gameID)
        {
            using (var dbManager = new DBManager())
            {
                var game = await dbManager.SelectOneAsync<Games>(a => a.ID == gameID);

                if (game == null)
                {
                    return false;
                }

                _game = game;

                _partyMembers = dbManager.SelectMany<PartyMembers>(a => a.GameID == gameID);
                
                return true;
            }
        }
    }
}