# 🗡️ Mystic Chronicles

A classic **Final Fantasy**-inspired RPG built with UWP (Universal Windows Platform) and C#. Experience the nostalgic 16-bit era RPG gameplay with modern Windows development practices.

![Platform](https://img.shields.io/badge/platform-UWP-blue)
![.NET](https://img.shields.io/badge/.NET-UWP%2010.0.26100-purple)
![C%23](https://img.shields.io/badge/C%23-11.0-green)
![License](https://img.shields.io/badge/license-MIT-orange)

## ✨ Features

### 🎮 Classic RPG Gameplay
- **Tile-based exploration** - Navigate procedurally generated maps
- **Random encounters** - Face enemies as you explore the world
- **Turn-based battles** - Strategic Final Fantasy-style combat system
- **Character progression** - Level up your hero and gain experience
- **Full screen mode** - Immersive gameplay with F11 toggle
- **Hidden cursor** - No mouse cursor from start to finish (keyboard/gamepad only)

### 🎨 Final Fantasy VI-Inspired UI
- **Classic FF blue background** with white borders (#0000AA)
- **Character status window** with portrait, HP/MP bars, and stats
- **Color-coded HP bars** (Green → Yellow → Red based on health)
- **Battle command menu** with Fight, Magic, Item, and Defend options
- **Message boxes** for battle narration and story events

### 🎵 Dynamic Music System
- **Main Menu Theme** - Welcoming soundtrack
- **Exploration Theme** - Plays while wandering the world
- **Battle Music** - Intense combat soundtrack
- **Victory Fanfare** - Celebration after winning battles
- **Game Over Theme** - Plays on defeat
- Supports **MP3 and OGG** formats with auto-looping

### 💾 Save System
- **Save/Load functionality** using local storage
- Persistent character stats and progress
- In-game menu with save option

### 🎯 Battle System
- **FF6-style positioning** - Heroes on right, enemies on left
- **Image backgrounds** - Support for custom battle backdrops
- **Four battle commands**: Fight, Magic, Item, Defend
- **Enemy HP bars** displayed above enemies
- **Turn-based combat** with player and enemy actions

## 🕹️ Controls

### Main Menu
- **Arrow Keys / W/S** - Navigate menu options
- **Enter / Space** - Confirm selection

### Exploration
- **W/A/S/D or Arrow Keys** - Move character
- **ESC** - Open in-game menu
- **F11** - Toggle full screen mode

### Battle
- **W/S or Up/Down** - Select command
- **Enter / Space** - Execute command
- **F11** - Toggle full screen mode
- Commands: Fight, Magic (10 MP), Item (restore 30 HP), Defend

### Character Creation
- **Left/Right or A/D** - Switch between Confirm/Cancel
- **Enter / Space** - Confirm selection

## 🚀 Getting Started

### Prerequisites
- **Windows 10** or later
- **Visual Studio 2019/2022** with UWP development workload
- **.NET Core 5.0 SDK**
- **Win2D.uwp** NuGet package (for 2D graphics)

### Installation

1. **Clone the repository**
   ```bash
   git clone https://github.com/jcapellman/Mystic-Chronicles.git
   cd Mystic-Chronicles
   ```

2. **Open in Visual Studio**
   - Open `MysticChronicles.sln`
   - Restore NuGet packages

3. **Build and Run**
   - Set build configuration to **x64** or **x86**
   - Press **F5** to build and run

## 🎨 Adding Custom Assets

### Battle Backgrounds
Place PNG images in:
```
Assets/BattleBackgrounds/
├── City.png
├── Forest.png
├── Cave.png
└── ...
```

### Music Files
Add MP3 or OGG files to:
```
Assets/Music/
├── MainMenu.mp3
├── Exploration.mp3
├── Battle.mp3
├── Victory.mp3
└── GameOver.mp3
```

**Note**: Music files are optional. The game will gracefully continue without them.

### UI Cursors
```
Assets/
└── Cursor.png (24x24 recommended)
```

## 📁 Project Structure

```
MysticChronicles/
├── Models/                    # Data models
│   ├── Character.cs          # Hero/Enemy stats
│   ├── SaveData.cs           # Save game structure
│   └── Tile.cs               # Map tile data
├── GameEngine/               # Core game logic
│   ├── BattleSystem.cs       # Combat mechanics
│   ├── Map.cs                # World generation
│   └── InputManager.cs       # Input handling
├── Services/                 # Game services
│   ├── MusicManager.cs       # Background music
│   └── SaveGameManager.cs    # Save/Load logic
├── Pages/                    # UI Pages
│   ├── MainMenuPage.xaml     # Main menu
│   ├── CharacterCreationPage.xaml
│   ├── GamePage.xaml         # Main gameplay
│   └── *.xaml.cs             # Code-behind files
└── Assets/                   # Game assets
    ├── BattleBackgrounds/
    ├── Music/
    └── Cursor.png
```

## 🛠️ Technologies Used

- **C# 11.0** - Latest language features
- **Windows SDK 10.0.26100** - Latest UWP platform
- **UWP (Universal Windows Platform)** - Cross-device Windows app platform
- **Win2D 1.28.0** - Hardware-accelerated 2D graphics
- **XAML** - Declarative UI markup
- **MediaPlayer API** - Background music playback
- **Local Storage API** - Save game persistence
- **Nullable reference types** - Enhanced null-safety

## 🎯 Gameplay Features

### Character Stats
- **HP (Health Points)** - Character's vitality
- **MP (Magic Points)** - Used for spells
- **Attack** - Physical damage dealt
- **Defense** - Damage reduction
- **Magic** - Magical damage/healing power
- **Speed** - Turn order in battle
- **Level** - Character progression

### Battle Commands
1. **Fight** - Standard physical attack
2. **Magic** - Cast Fire spell (10 MP, 1.5x Magic damage)
3. **Item** - Use Potion (restore 30 HP)
4. **Defend** - Increase defense by 50% for one turn

### Map Tiles
- 🟩 **Grass** - Walkable terrain
- 🔵 **Water** - Non-walkable
- ⬜ **Mountain** - Non-walkable
- 🌲 **Forest** - Walkable terrain

## 🔮 Future Enhancements

- [ ] Multiple playable characters
- [ ] Equipment system (weapons, armor)
- [ ] Spell variety and skill trees
- [ ] Larger world maps with towns/NPCs
- [ ] Quest system
- [ ] Inventory management
- [ ] Multiple enemy types with unique abilities
- [ ] Boss battles
- [ ] Story mode
- [ ] Achievements

## 🤝 Contributing

Contributions are welcome! Here's how you can help:

1. **Fork** the repository
2. **Create** a feature branch (`git checkout -b feature/AmazingFeature`)
3. **Commit** your changes (`git commit -m 'Add some AmazingFeature'`)
4. **Push** to the branch (`git push origin feature/AmazingFeature`)
5. **Open** a Pull Request

### Code Style
- Follow C# naming conventions
- Use meaningful variable names
- Comment complex logic
- Maintain FF6 aesthetic for UI elements

## 📝 License

This project is licensed under the **MIT License** - see the [LICENSE](LICENSE) file for details.

## 🙏 Acknowledgments

- Inspired by **Final Fantasy IV & VI** (SNES) by Square Enix
- Classic JRPG mechanics and UI design
- 16-bit era RPG aesthetics

## 📧 Contact

**Jarred Capellman** - [@jcapellman](https://github.com/jcapellman)

Project Link: [https://github.com/jcapellman/Mystic-Chronicles](https://github.com/jcapellman/Mystic-Chronicles)
