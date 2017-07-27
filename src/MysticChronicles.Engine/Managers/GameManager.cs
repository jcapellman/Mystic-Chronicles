using System.Threading.Tasks;

using MysticChronicles.Engine.DB;
using MysticChronicles.Engine.DB.Tables;
using MysticChronicles.Engine.Objects.Common;

namespace MysticChronicles.Engine.Managers
{
    public class GameManager
    {
        public static async Task<GameContainer> LoadGame(int gameID)
        {
            using (var dbManager = new DBManager())
            {
                var game = await dbManager.SelectOneAsync<Games>(a => a.ID == gameID);

                if (game == null)
                {
                    return null;
                }
                
                var partyMembers = dbManager.SelectMany<PartyMembers>(a => a.GameID == gameID);
                var variables = dbManager.SelectMany<GameVariables>(a => a.GameID == gameID);

                return new GameContainer(game, partyMembers, variables);
            }
        }

    }
}