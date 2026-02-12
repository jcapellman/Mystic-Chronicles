using System.Collections.Generic;
using Windows.System;

namespace MysticChronicles.GameEngine
{
    public class InputManager
    {
        private HashSet<VirtualKey> pressedKeys;

        public InputManager()
        {
            pressedKeys = new HashSet<VirtualKey>();
        }

        public void KeyPressed(VirtualKey key)
        {
            pressedKeys.Add(key);
        }

        public void KeyReleased(VirtualKey key)
        {
            pressedKeys.Remove(key);
        }

        public bool IsKeyPressed(VirtualKey key)
        {
            return pressedKeys.Contains(key);
        }

        public void Clear()
        {
            pressedKeys.Clear();
        }
    }
}
