using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using MysticChronicles.Engine.DB;
using MysticChronicles.Engine.DB.Tables;

namespace MysticChronicles.Engine.Managers
{
    public class GameManager
    {
        private Games _game;
        private List<PartyMembers> _partyMembers;
        private List<GameVariables> _variables;

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
                _variables = dbManager.SelectMany<GameVariables>(a => a.GameID == gameID);

                return true;
            }
        }

        public GameVariables GetGameVariable(string name) =>_variables.FirstOrDefault(a => a.VarName == name);

        public List<PartyMembers> GetPartyMembers() => _partyMembers;
    }
}