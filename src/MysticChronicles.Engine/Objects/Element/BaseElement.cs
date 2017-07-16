namespace MysticChronicles.Engine.Objects.Element
{
    public class BaseElement
    {
        public bool IsVisible { get; protected set; }

        public void Hide()
        {
            IsVisible = false;
        }

        public void Show()
        {
            IsVisible = true;
        }
    }
}