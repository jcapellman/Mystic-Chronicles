using System.Collections.Generic;
using System.Linq;

using MysticChronicles.Engine.DB.Tables;

namespace MysticChronicles.Engine.Objects.Common
{
    public class GameContainer
    {
        private Games _game;
        private readonly List<PartyMembers> _partyMembers;
        private readonly List<GameVariables> _variables;

        public GameContainer(Games game, List<PartyMembers> partyMembers, List<GameVariables> variables)
        {
            _game = game;
            _partyMembers = partyMembers;
            _variables = variables;
        }

        public GameVariables GetGameVariable(string name) => _variables.FirstOrDefault(a => a.VarName == name);

        public List<PartyMembers> GetPartyMembers() => _partyMembers;
    }
}