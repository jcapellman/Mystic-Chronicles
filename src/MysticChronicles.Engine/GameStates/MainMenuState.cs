using MysticChronicles.Engine.Objects.Common;
using MysticChronicles.Engine.Objects.Element.Static;

namespace MysticChronicles.Engine.GameStates
{
    public class MainMenuState : BaseGameState
    {
        public MainMenuState(GameStateContainer container) : base(container) { }

        public override void LoadContent()
        {
            AddGraphicElement(new BackgroundImage(EContainer, "UI/MainMenu"));
        }
    }
}