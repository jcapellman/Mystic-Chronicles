using System.Threading.Tasks;

using MysticChronicles.Engine.DB;
using MysticChronicles.Engine.DB.Tables;

namespace MysticChronicles.Engine.Managers
{
    public class GameManager
    {
        private Games _game;
        
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

                return true;
            }
        }
    }
}