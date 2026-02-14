# WinUI 3 Migration Tasks

**Repository**: Mystic-Chronicles  
**Source Branch**: master  
**Target Branch**: winui3-migration  
**Target Framework**: net8.0-windows10.0.19041.0  
**Deployment Model**: Packaged (MSIX)

---

## Overview

This task list guides the migration from UWP to WinUI 3. Complete tasks sequentially within each phase. Each phase has validation steps to confirm success before proceeding.

**Total Phases**: 7  
**Estimated Total Effort**: 3-5 days

---

## Phase 0: Preparation

**Objective**: Set up migration environment and finalize decisions  
**Estimated Time**: 1 hour

### Prerequisites Verification

- [ ] **Task 0.1**: Verify Visual Studio 2022 version ≥ 17.8
  - Open Visual Studio → Help → About
  - Confirm version is 17.8 or higher
  - If not, update Visual Studio

- [ ] **Task 0.2**: Verify .NET 8 SDK installed
  - Open terminal
  - Run: `dotnet --list-sdks`
  - Confirm .NET 8.0.x appears in list
  - If not, download from https://dotnet.microsoft.com/download/dotnet/8.0

- [ ] **Task 0.3**: Verify Windows App SDK workload installed
  - Open Visual Studio Installer
  - Modify installation
  - Under "Desktop & Mobile" → Check "Windows application development"
  - Verify "Windows App SDK C# Templates" is installed

### Git Setup

- [ ] **Task 0.4**: Check current branch
  - Run: `git branch --show-current`
  - Confirm on `master` branch
  - If not, checkout master: `git checkout master`

- [ ] **Task 0.5**: Ensure working directory is clean
  - Run: `git status`
  - If uncommitted changes exist, commit or stash them
  - Commit message: "chore: Checkpoint before WinUI 3 migration"

- [ ] **Task 0.6**: Create migration branch
  - Run: `git checkout -b winui3-migration`
  - Verify new branch created: `git branch --show-current`

- [ ] **Task 0.7**: Tag baseline state
  - Run: `git tag pre-winui3-migration master`
  - Verify tag created: `git tag -l`

### Decision Documentation

- [ ] **Task 0.8**: Confirm deployment model decision
  - Decision: **Packaged (MSIX)** deployment
  - Rationale: Maintains file access patterns, minimal code changes
  - Document in notes if different choice made

- [ ] **Task 0.9**: Confirm target framework decision
  - Decision: **net8.0-windows10.0.19041.0**
  - Rationale: .NET 8 LTS (supported until Nov 2026)
  - Document in notes if different choice made

- [ ] **Task 0.10**: Confirm input architecture decision
  - Decision: **Window-level PreviewKeyDown** with IKeyboardHandler
  - Rationale: Similar to current CoreWindow pattern, maintainable
  - Document in notes if different choice made

### Phase 0 Validation

- [ ] **Validation 0.1**: Confirm on `winui3-migration` branch
- [ ] **Validation 0.2**: Confirm tag `pre-winui3-migration` exists
- [ ] **Validation 0.3**: Confirm all prerequisites installed
- [ ] **Validation 0.4**: Confirm all decisions documented

**Phase 0 Complete** ✅ Proceed to Phase 1

---

## Phase 1: Project Infrastructure

**Objective**: Convert project files from UWP to WinUI 3 structure  
**Estimated Time**: 2-3 hours

### Update GORE.csproj

- [ ] **Task 1.1**: Open `GORE-Engine\src\GORE.csproj` in text editor

- [ ] **Task 1.2**: Remove UWP-specific properties
  - Remove: `<TargetPlatformIdentifier>UAP</TargetPlatformIdentifier>`
  - Remove: `<TargetPlatformVersion>10.0.26100.0</TargetPlatformVersion>`
  - Remove: `<TargetPlatformMinVersion>10.0.26100.0</TargetPlatformMinVersion>`
  - Remove: `<MinimumVisualStudioVersion>14</MinimumVisualStudioVersion>`
  - Remove: `<ProjectTypeGuids>` (if present)

- [ ] **Task 1.3**: Add WinUI 3 target framework to GORE.csproj
  - Add inside `<PropertyGroup>`:
    ```xml
    <TargetFramework>net8.0-windows10.0.19041.0</TargetFramework>
    <TargetPlatformMinVersion>10.0.17763.0</TargetPlatformMinVersion>
    <UseWinUI>true</UseWinUI>
    <RuntimeIdentifiers>win10-x64</RuntimeIdentifiers>
    ```

- [ ] **Task 1.4**: Update package references in GORE.csproj
  - Remove: `<PackageReference Include="Microsoft.NETCore.UniversalWindowsPlatform" Version="6.2.14" />`
  - Update: Change `Win2D.uwp` to `Microsoft.Graphics.Win2D`:
    ```xml
    <PackageReference Include="Microsoft.Graphics.Win2D" Version="1.2.0" />
    ```
  - Keep: `<PackageReference Include="System.Text.Json" Version="8.0.6" />`
  - Add: 
    ```xml
    <PackageReference Include="Microsoft.WindowsAppSDK" Version="1.5.240802000" />
    <PackageReference Include="Microsoft.Windows.SDK.BuildTools" Version="10.0.22621.3233" />
    ```

- [ ] **Task 1.5**: Save GORE.csproj

- [ ] **Task 1.6**: Restore packages for GORE project
  - Run: `dotnet restore GORE-Engine\src\GORE.csproj`
  - Expect warnings about namespaces (OK for now)
  - Confirm packages restored successfully

### Update MysticChronicles.csproj

- [ ] **Task 1.7**: Open `src\MysticChronicles.csproj` in text editor

- [ ] **Task 1.8**: Remove UWP-specific properties from MysticChronicles.csproj
  - Remove: `<TargetPlatformIdentifier>UAP</TargetPlatformIdentifier>`
  - Remove: `<TargetPlatformVersion>10.0.26100.0</TargetPlatformVersion>`
  - Remove: `<TargetPlatformMinVersion>10.0.26100.0</TargetPlatformMinVersion>`
  - Remove: `<ProjectTypeGuids>` (if present)
  - Change: `<OutputType>AppContainerExe</OutputType>` to `<OutputType>WinExe</OutputType>`

- [ ] **Task 1.9**: Add WinUI 3 configuration to MysticChronicles.csproj
  - Add inside `<PropertyGroup>`:
    ```xml
    <TargetFramework>net8.0-windows10.0.19041.0</TargetFramework>
    <TargetPlatformMinVersion>10.0.17763.0</TargetPlatformMinVersion>
    <UseWinUI>true</UseWinUI>
    <EnableMsixTooling>true</EnableMsixTooling>
    <RuntimeIdentifiers>win10-x64</RuntimeIdentifiers>
    ```

- [ ] **Task 1.10**: Update package references in MysticChronicles.csproj
  - Remove: `<PackageReference Include="Microsoft.NETCore.UniversalWindowsPlatform" Version="6.2.14" />`
  - Update: Change `Win2D.uwp` to `Microsoft.Graphics.Win2D`:
    ```xml
    <PackageReference Include="Microsoft.Graphics.Win2D" Version="1.2.0" />
    ```
  - Add:
    ```xml
    <PackageReference Include="Microsoft.WindowsAppSDK" Version="1.5.240802000" />
    <PackageReference Include="Microsoft.Windows.SDK.BuildTools" Version="10.0.22621.3233" />
    ```

- [ ] **Task 1.11**: Save MysticChronicles.csproj

- [ ] **Task 1.12**: Restore packages for MysticChronicles project
  - Run: `dotnet restore src\MysticChronicles.csproj`
  - Confirm packages restored successfully

### Update Package.appxmanifest

- [ ] **Task 1.13**: Open `src\Package.appxmanifest` in text editor

- [ ] **Task 1.14**: Update manifest schema for Windows App SDK
  - Update `<Package>` element to include:
    ```xml
    xmlns:rescap="http://schemas.microsoft.com/appx/manifest/foundation/windows10/restrictedcapabilities"
    IgnorableNamespaces="uap rescap"
    ```

- [ ] **Task 1.15**: Update TargetDeviceFamily
  - Change from:
    ```xml
    <TargetDeviceFamily Name="Windows.Universal" MinVersion="10.0.0.0" MaxVersionTested="10.0.0.0" />
    ```
  - To:
    ```xml
    <TargetDeviceFamily Name="Windows.Desktop" MinVersion="10.0.17763.0" MaxVersionTested="10.0.22621.0" />
    ```

- [ ] **Task 1.16**: Save Package.appxmanifest

### Phase 1 Validation

- [ ] **Validation 1.1**: GORE.csproj package restore succeeds
- [ ] **Validation 1.2**: MysticChronicles.csproj package restore succeeds
- [ ] **Validation 1.3**: Microsoft.WindowsAppSDK appears in Solution Explorer under both projects
- [ ] **Validation 1.4**: Microsoft.Graphics.Win2D appears under both projects
- [ ] **Validation 1.5**: Attempt build (will fail with namespace errors - EXPECTED)
- [ ] **Validation 1.6**: Confirm build fails with "Windows.UI.Xaml not found" (not package restore errors)

- [ ] **Task 1.17**: Commit Phase 1 changes
  - Run: `git add .`
  - Run: `git commit -m "refactor: Convert project files to WinUI 3 structure"`

**Phase 1 Complete** ✅ Proceed to Phase 2

---

## Phase 2: Namespace Updates

**Objective**: Replace all UWP namespaces with WinUI 3 equivalents  
**Estimated Time**: 1-2 hours

### Namespace Replacement Strategy

- [ ] **Task 2.1**: Choose replacement method
  - Recommended: Visual Studio Find & Replace (Ctrl+Shift+H)
  - Alternative: PowerShell script (more error-prone)

### Update GORE Engine Namespaces

- [ ] **Task 2.2**: Replace Windows.UI.Xaml → Microsoft.UI.Xaml
  - Find: `using Windows.UI.Xaml;`
  - Replace: `using Microsoft.UI.Xaml;`
  - Scope: Entire Solution
  - Match case: Yes
  - Click "Replace All"
  - Verify replacements in: BasePage.cs, BaseMainMenuPage.cs, BaseGamePage.cs, BaseCharacterCreationPage.cs, MainMenuPage.xaml.cs, GoreEngine.cs

- [ ] **Task 2.3**: Replace Windows.UI.Xaml.Controls
  - Find: `using Windows.UI.Xaml.Controls;`
  - Replace: `using Microsoft.UI.Xaml.Controls;`
  - Click "Replace All"

- [ ] **Task 2.4**: Replace Windows.UI.Xaml.Navigation
  - Find: `using Windows.UI.Xaml.Navigation;`
  - Replace: `using Microsoft.UI.Xaml.Navigation;`
  - Click "Replace All"

- [ ] **Task 2.5**: Replace Windows.UI namespace (for Colors)
  - Find: `using Windows.UI;`
  - Replace: `using Microsoft.UI;`
  - Click "Replace All"

- [ ] **Task 2.6**: Update Windows.System to Microsoft.UI.Input
  - Find: `using Windows.System;`
  - Replace: `using Microsoft.UI.Input;`
  - Click "Replace All"
  - Note: VirtualKey moved to Microsoft.UI.Input

- [ ] **Task 2.7**: Update Windows.UI.ViewManagement
  - Find: `using Windows.UI.ViewManagement;`
  - Replace: `using Microsoft.UI.Windowing;`
  - Click "Replace All" (in GoreEngine.cs)

- [ ] **Task 2.8**: Remove Windows.UI.Core references
  - Find: `using Windows.UI.Core;`
  - **DELETE** this line (no replacement - CoreWindow doesn't exist in WinUI 3)
  - Affected files: BaseMainMenuPage.cs, BaseGamePage.cs, BaseCharacterCreationPage.cs
  - Will cause errors - to be fixed in Phase 4

### Update App.xaml.cs Namespaces

- [ ] **Task 2.9**: Open `src\App.xaml.cs`

- [ ] **Task 2.10**: Update App.xaml.cs namespaces
  - Replace: `using Windows.UI.Xaml;` → `using Microsoft.UI.Xaml;`
  - Replace: `using Windows.UI.Xaml.Controls;` → `using Microsoft.UI.Xaml.Controls;`
  - Replace: `using Windows.UI.Xaml.Navigation;` → `using Microsoft.UI.Xaml.Navigation;`
  - Remove: `using Windows.ApplicationModel;`
  - Remove: `using Windows.ApplicationModel.Activation;`
  - These will be handled differently in Phase 3

### Verify XAML Files (No Changes Needed)

- [ ] **Task 2.11**: Verify App.xaml namespace
  - Open `src\App.xaml`
  - Confirm xmlns remains: `xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"`
  - **NO CHANGE NEEDED** - XAML namespace URL is same in WinUI 3 ✅

- [ ] **Task 2.12**: Verify MainMenuPage.xaml namespace (if exists)
  - Confirm xmlns unchanged
  - **NO CHANGE NEEDED** ✅

### Phase 2 Validation

- [ ] **Validation 2.1**: Build solution
- [ ] **Validation 2.2**: Confirm no "Windows.UI.Xaml not found" errors
- [ ] **Validation 2.3**: Confirm Microsoft.UI.Xaml namespaces resolve
- [ ] **Validation 2.4**: Expected errors: LaunchActivatedEventArgs, CoreWindow references (Phase 3 & 4 will fix)
- [ ] **Validation 2.5**: XAML files compile without errors

- [ ] **Task 2.13**: Commit Phase 2 changes
  - Run: `git add .`
  - Run: `git commit -m "refactor: Update namespaces from UWP to WinUI 3"`

**Phase 2 Complete** ✅ Proceed to Phase 3

---

## Phase 3: Window and Lifecycle Migration

**Objective**: Refactor App.xaml.cs to use WinUI 3 window model  
**Estimated Time**: 2-3 hours

### Create MainWindow XAML

- [ ] **Task 3.1**: Create new file `src\MainWindow.xaml`

- [ ] **Task 3.2**: Add MainWindow XAML content
  ```xml
  <Window
      x:Class="MysticChronicles.MainWindow"
      xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
      xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
      xmlns:local="using:MysticChronicles">

      <Frame x:Name="rootFrame"/>
  </Window>
  ```

- [ ] **Task 3.3**: Save MainWindow.xaml

### Create MainWindow Code-Behind

- [ ] **Task 3.4**: Create new file `src\MainWindow.xaml.cs`

- [ ] **Task 3.5**: Add MainWindow.xaml.cs content
  ```csharp
  using Microsoft.UI.Xaml;
  using GORE.Pages;
  using GORE.Services;

  namespace MysticChronicles
  {
      public sealed partial class MainWindow : Window
      {
          public MainWindow()
          {
              this.InitializeComponent();
              
              // Navigate to main menu on window load
              rootFrame.Navigate(typeof(MainMenuPage));
              
              // Apply game mode after window is activated
              this.Activated += MainWindow_Activated;
          }

          private void MainWindow_Activated(object sender, WindowActivatedEventArgs args)
          {
              if (args.WindowActivationState != WindowActivationState.Deactivated)
              {
                  // Only apply game mode once
                  this.Activated -= MainWindow_Activated;
                  
                  // TEMPORARILY DISABLED until Phase 5
                  // GoreEngine.ApplyGameMode(this);
              }
          }
      }
  }
  ```

- [ ] **Task 3.6**: Save MainWindow.xaml.cs

### Update App.xaml.cs

- [ ] **Task 3.7**: Open `src\App.xaml.cs`

- [ ] **Task 3.8**: Replace entire App.xaml.cs content
  ```csharp
  using Microsoft.UI.Xaml;
  using GORE.Pages;
  using GORE.Services;

  namespace MysticChronicles
  {
      public partial class App : Application
      {
          private Window m_window;

          // Expose window for keyboard handler registration
          public static MainWindow MainWindow { get; private set; }

          public App()
          {
              this.InitializeComponent();
          }

          protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
          {
              m_window = new MainWindow();
              MainWindow = (MainWindow)m_window;
              m_window.Activate();
          }
      }
  }
  ```

- [ ] **Task 3.9**: Save App.xaml.cs

### Update Project File References

- [ ] **Task 3.10**: Verify MainWindow files included in project
  - If using SDK-style project, files should auto-include
  - Build solution to verify files are recognized

### Phase 3 Validation

- [ ] **Validation 3.1**: Build solution
- [ ] **Validation 3.2**: Confirm App.xaml.cs compiles with no errors
- [ ] **Validation 3.3**: Confirm MainWindow.xaml compiles
- [ ] **Validation 3.4**: Confirm MainWindow.xaml.cs compiles
- [ ] **Validation 3.5**: GORE project still has CoreWindow errors (expected - Phase 4)

### Runtime Test (Partial)

- [ ] **Validation 3.6**: Run application
- [ ] **Validation 3.7**: Confirm window appears (even if navigation fails)
- [ ] **Validation 3.8**: Expected: Navigation to MainMenuPage fails with error (CoreWindow not found)
  - This is EXPECTED - Phase 4 will fix

- [ ] **Task 3.11**: Commit Phase 3 changes
  - Run: `git add .`
  - Run: `git commit -m "refactor: Migrate window lifecycle to WinUI 3 model"`

**Phase 3 Complete** ✅ Proceed to Phase 4

---

## Phase 4: Input System Rewrite

**Objective**: Replace CoreWindow input with WinUI 3 window-level events  
**Estimated Time**: 4-6 hours ⚠️ **CRITICAL PHASE**

### Create IKeyboardHandler Interface

- [ ] **Task 4.1**: Open `GORE-Engine\src\Pages\BasePage.cs`

- [ ] **Task 4.2**: Add IKeyboardHandler interface
  - Add at top of file, before BasePage class:
    ```csharp
    using Microsoft.UI.Input;
    
    /// <summary>
    /// Interface for keyboard handling in pages
    /// </summary>
    public interface IKeyboardHandler
    {
        void HandleKeyDown(VirtualKey key, bool isRepeat);
    }
    ```

- [ ] **Task 4.3**: Update BasePage to implement IKeyboardHandler
  - Change class declaration to: `public abstract class BasePage : Page, IKeyboardHandler`
  
- [ ] **Task 4.4**: Add keyboard handler registration to BasePage
  - Update OnNavigatedTo:
    ```csharp
    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        base.OnNavigatedTo(e);
        RegisterKeyboardHandler();
    }

    protected override void OnNavigatedFrom(NavigationEventArgs e)
    {
        base.OnNavigatedFrom(e);
        UnregisterKeyboardHandler();
    }

    private void RegisterKeyboardHandler()
    {
        if (App.MainWindow != null)
        {
            App.MainWindow.SetKeyboardHandler(this);
        }
    }

    private void UnregisterKeyboardHandler()
    {
        if (App.MainWindow != null)
        {
            App.MainWindow.SetKeyboardHandler(null);
        }
    }

    public virtual void HandleKeyDown(VirtualKey key, bool isRepeat)
    {
        // Base implementation does nothing
    }
    ```

- [ ] **Task 4.5**: Save BasePage.cs

### Update MainWindow for Input Routing

- [ ] **Task 4.6**: Open `src\MainWindow.xaml.cs`

- [ ] **Task 4.7**: Add input routing to MainWindow
  - Add using: `using Microsoft.UI.Xaml.Input;`
  - Add using: `using Microsoft.UI.Input;`
  - Add field: `private IKeyboardHandler currentKeyboardHandler;`
  - Add method:
    ```csharp
    public void SetKeyboardHandler(IKeyboardHandler handler)
    {
        currentKeyboardHandler = handler;
    }
    ```
  - In constructor, after InitializeComponent():
    ```csharp
    this.Content.PreviewKeyDown += Content_PreviewKeyDown;
    ```
  - Add event handler:
    ```csharp
    private void Content_PreviewKeyDown(object sender, KeyRoutedEventArgs e)
    {
        if (currentKeyboardHandler != null)
        {
            var key = (VirtualKey)e.Key;
            bool isRepeat = e.KeyStatus.RepeatCount > 1;
            currentKeyboardHandler.HandleKeyDown(key, isRepeat);
            e.Handled = true;
        }
    }
    ```

- [ ] **Task 4.8**: Save MainWindow.xaml.cs

### Update BaseMainMenuPage

- [ ] **Task 4.9**: Open `GORE-Engine\src\Pages\BaseMainMenuPage.cs`

- [ ] **Task 4.10**: Remove CoreWindow references from BaseMainMenuPage
  - Remove all `Window.Current.CoreWindow.KeyDown` subscriptions in OnNavigatedTo
  - Remove all `Window.Current.CoreWindow.KeyDown` unsubscriptions in OnNavigatedFrom
  - Delete `CoreWindow_KeyDown` method entirely

- [ ] **Task 4.11**: Implement HandleKeyDown in BaseMainMenuPage
  - Add method (replaces CoreWindow_KeyDown):
    ```csharp
    public override void HandleKeyDown(VirtualKey key, bool isRepeat)
    {
        if (isDialogOpen) return;

        if (key == VirtualKey.Up || key == VirtualKey.W)
        {
            menuSelection--;
            if (menuSelection < 0)
                menuSelection = MenuItemCount - 1;
            UpdateMenuCursor();
        }
        else if (key == VirtualKey.Down || key == VirtualKey.S)
        {
            menuSelection++;
            if (menuSelection >= MenuItemCount)
                menuSelection = 0;
            UpdateMenuCursor();
        }
        else if (key == VirtualKey.Enter || key == VirtualKey.Space)
        {
            ExecuteMenuSelection();
        }
    }
    ```

- [ ] **Task 4.12**: Update OnNavigatedTo in BaseMainMenuPage
  - Ensure it calls `base.OnNavigatedTo(e);` first
  - Remove CoreWindow event subscriptions
  - Keep initialization logic

- [ ] **Task 4.13**: Update OnNavigatedFrom in BaseMainMenuPage
  - Ensure it calls `base.OnNavigatedFrom(e);` first
  - Remove CoreWindow event unsubscriptions
  - Keep cleanup logic

- [ ] **Task 4.14**: Save BaseMainMenuPage.cs

### Update BaseGamePage

- [ ] **Task 4.15**: Open `GORE-Engine\src\Pages\BaseGamePage.cs`

- [ ] **Task 4.16**: Remove CoreWindow references from BaseGamePage
  - Remove `Window.Current.CoreWindow.KeyDown` subscription in OnNavigatedTo
  - Remove `Window.Current.CoreWindow.KeyDown` unsubscription in OnNavigatedFrom
  - Delete `OnCoreWindowKeyDown` method entirely

- [ ] **Task 4.17**: Implement HandleKeyDown in BaseGamePage
  - Add method (replaces OnCoreWindowKeyDown):
    ```csharp
    public override void HandleKeyDown(VirtualKey key, bool isRepeat)
    {
        if (isDialogOpen) return;

        if (key == VirtualKey.Escape)
        {
            ToggleMenu();
            return;
        }

        if (isMenuOpen)
        {
            HandleMenuInput(key);
        }
        else
        {
            switch (gameState)
            {
                case GameState.Exploration:
                    HandleExplorationInput(key);
                    break;
                case GameState.Battle:
                    HandleBattleInput(key);
                    break;
            }
        }
    }

    private void HandleMenuInput(VirtualKey key)
    {
        // Copy existing menu input logic from OnCoreWindowKeyDown
    }

    private void HandleExplorationInput(VirtualKey key)
    {
        // Copy existing exploration input logic
    }

    private void HandleBattleInput(VirtualKey key)
    {
        // Copy existing battle input logic
    }
    ```

- [ ] **Task 4.18**: Update OnNavigatedTo/From in BaseGamePage
  - Ensure calls to base methods
  - Remove CoreWindow subscriptions

- [ ] **Task 4.19**: Save BaseGamePage.cs

### Update BaseCharacterCreationPage

- [ ] **Task 4.20**: Open `GORE-Engine\src\Pages\BaseCharacterCreationPage.cs`

- [ ] **Task 4.21**: Remove CoreWindow references
  - Remove `Window.Current.CoreWindow.KeyDown` subscription
  - Remove `Window.Current.CoreWindow.CharacterReceived` subscription
  - Delete `CoreWindow_KeyDown` method
  - Delete `CoreWindow_CharacterReceived` method

- [ ] **Task 4.22**: Implement simplified HandleKeyDown
  - Add method (navigation only, not character input):
    ```csharp
    public override void HandleKeyDown(VirtualKey key, bool isRepeat)
    {
        if (key == VirtualKey.Left || key == VirtualKey.A)
        {
            selection = 0; // Confirm
            UpdateSelectionCursor();
        }
        else if (key == VirtualKey.Right || key == VirtualKey.D)
        {
            selection = 1; // Cancel
            UpdateSelectionCursor();
        }
        else if (key == VirtualKey.Enter || key == VirtualKey.Space)
        {
            ExecuteSelection();
        }
        else if (key == VirtualKey.Escape)
        {
            OnCancel();
        }
    }
    ```

- [ ] **Task 4.23**: Add TextBox change handler method
  ```csharp
  protected void OnNameTextChanged(object sender, TextChangedEventArgs e)
  {
      if (sender is TextBox textBox)
      {
          characterName = textBox.Text;
          OnCharacterNameChanged(characterName);
      }
  }
  ```

- [ ] **Task 4.24**: Save BaseCharacterCreationPage.cs

### Update CharacterCreationPage XAML

- [ ] **Task 4.25**: Open `GORE-Engine\src\Pages\CharacterCreationPage.xaml`

- [ ] **Task 4.26**: Add TextBox for character name input
  - Add to Grid content (replace existing character input UI):
    ```xml
    <TextBox x:Name="txtCharacterName"
             Text="Hero"
             PlaceholderText="Enter character name"
             Width="300"
             MaxLength="20"
             TextChanged="TxtCharacterName_TextChanged"
             HorizontalAlignment="Center"/>
    ```

- [ ] **Task 4.27**: Save CharacterCreationPage.xaml

- [ ] **Task 4.28**: Update CharacterCreationPage.xaml.cs to wire up TextChanged event
  - Ensure TxtCharacterName_TextChanged calls base.OnNameTextChanged

### Update MainMenuPage.xaml.cs

- [ ] **Task 4.29**: Open `GORE-Engine\src\Pages\MainMenuPage.xaml.cs`

- [ ] **Task 4.30**: Remove CoreWindow references from OnLoadGame
  - Remove: `Window.Current.CoreWindow.KeyDown -= CoreWindow_KeyDown;` (before async work)
  - Remove: `Window.Current.CoreWindow.KeyDown += CoreWindow_KeyDown;` (after async work)
  - Keyboard handling now managed by base class automatically

- [ ] **Task 4.31**: Save MainMenuPage.xaml.cs

### Phase 4 Validation - Build

- [ ] **Validation 4.1**: Build solution
- [ ] **Validation 4.2**: Confirm ZERO CoreWindow-related errors
- [ ] **Validation 4.3**: Confirm solution builds successfully
- [ ] **Validation 4.4**: Check for warnings (address any critical ones)

### Phase 4 Validation - Runtime

- [ ] **Validation 4.5**: Run application
- [ ] **Validation 4.6**: Confirm application launches
- [ ] **Validation 4.7**: Confirm window appears
- [ ] **Validation 4.8**: Confirm MainMenuPage displays

### Phase 4 Validation - Input Testing

- [ ] **Validation 4.9**: **CRITICAL** - Test menu navigation
  - Press Up arrow → Menu selection moves up
  - Press Down arrow → Menu selection moves down
  - Press W → Menu selection moves up
  - Press S → Menu selection moves down
  - Menu wraps around (top → bottom, bottom → top)

- [ ] **Validation 4.10**: **CRITICAL** - Test menu activation
  - Press Enter → Activates selected menu item
  - Press Space → Activates selected menu item

- [ ] **Validation 4.11**: Test character creation (if New Game works)
  - Navigate to character creation page
  - Click in TextBox
  - Type character name
  - Backspace works to delete
  - Max 20 characters enforced
  - Left/Right navigate Confirm/Cancel
  - Enter activates selection

### Debug if Input Doesn't Work

If menu navigation fails:

- [ ] **Debug 4.1**: Add breakpoint in MainWindow.Content_PreviewKeyDown
  - Run in debugger
  - Press arrow key
  - Verify breakpoint hits
  - Check if currentKeyboardHandler is null

- [ ] **Debug 4.2**: Add breakpoint in BaseMainMenuPage.HandleKeyDown
  - Verify method is called
  - Check key parameter value

- [ ] **Debug 4.3**: Add debug output
  - In MainWindow.SetKeyboardHandler: Add `System.Diagnostics.Debug.WriteLine($"Handler set: {handler?.GetType().Name}");`
  - In HandleKeyDown implementations: Add `System.Diagnostics.Debug.WriteLine($"Key pressed: {key}");`

- [ ] **Task 4.32**: Commit Phase 4 changes
  - Run: `git add .`
  - Run: `git commit -m "refactor: Rewrite input system for WinUI 3"`

**Phase 4 Complete** ✅ Proceed to Phase 5

---

## Phase 5: API-Specific Updates

**Objective**: Update remaining UWP-specific APIs  
**Estimated Time**: 2-3 hours

### Update GoreEngine.cs - Fullscreen Mode

- [ ] **Task 5.1**: Open `GORE-Engine\src\Services\GoreEngine.cs`

- [ ] **Task 5.2**: Add required namespaces
  - Add: `using Microsoft.UI.Windowing;`
  - Add: `using WinRT.Interop;`

- [ ] **Task 5.3**: Update ApplyGameMode method signature
  - Change from: `public static void ApplyGameMode()`
  - To: `public static void ApplyGameMode(Window window)`

- [ ] **Task 5.4**: Replace fullscreen code
  - Remove: `var view = ApplicationView.GetForCurrentView();` and `view.TryEnterFullScreenMode();`
  - Add:
    ```csharp
    var hwnd = WindowNative.GetWindowHandle(window);
    var windowId = Win32Interop.GetWindowIdFromWindow(hwnd);
    var appWindow = AppWindow.GetFromWindowId(windowId);
    appWindow.SetPresenter(AppWindowPresenterKind.FullScreen);
    ```

- [ ] **Task 5.5**: Update cursor hiding code
  - Remove: `Window.Current.CoreWindow.PointerCursor = null;`
  - Add:
    ```csharp
    if (window.Content is FrameworkElement content)
    {
        content.Cursor = null;
    }
    ```

- [ ] **Task 5.6**: Update EnsureGameMode method signature
  - Change from: `public static void EnsureGameMode()`
  - To: `public static void EnsureGameMode(Window window)`

- [ ] **Task 5.7**: Update EnsureGameMode implementation
  - Replace body with:
    ```csharp
    if (!_isInitialized) return;

    var hwnd = WindowNative.GetWindowHandle(window);
    var windowId = Win32Interop.GetWindowIdFromWindow(hwnd);
    var appWindow = AppWindow.GetFromWindowId(windowId);

    if (appWindow.Presenter.Kind != AppWindowPresenterKind.FullScreen)
    {
        appWindow.SetPresenter(AppWindowPresenterKind.FullScreen);
    }
    ```

- [ ] **Task 5.8**: Save GoreEngine.cs

### Update BasePage to Pass Window

- [ ] **Task 5.9**: Open `GORE-Engine\src\Pages\BasePage.cs`

- [ ] **Task 5.10**: Update OnNavigatedTo to pass window to EnsureGameMode
  - Change call from: `GoreEngine.EnsureGameMode();`
  - To:
    ```csharp
    if (App.MainWindow != null)
    {
        GoreEngine.EnsureGameMode(App.MainWindow);
    }
    ```

- [ ] **Task 5.11**: Save BasePage.cs

### Enable Game Mode in MainWindow

- [ ] **Task 5.12**: Open `src\MainWindow.xaml.cs`

- [ ] **Task 5.13**: Uncomment game mode call
  - In MainWindow_Activated method, uncomment:
    ```csharp
    GoreEngine.ApplyGameMode(this);
    ```

- [ ] **Task 5.14**: Save MainWindow.xaml.cs

### Update MusicManager (Packaged Deployment)

- [ ] **Task 5.15**: Open `GORE-Engine\src\Services\MusicManager.cs`

- [ ] **Task 5.16**: Verify file access pattern
  - Current code uses: `Windows.ApplicationModel.Package.Current.InstalledLocation`
  - For **packaged deployment**, this still works in WinUI 3
  - **NO CHANGES NEEDED** ✅

- [ ] **Task 5.17**: Refactor switch to method (optional cleanup)
  - Extract filename logic to separate method:
    ```csharp
    private static string GetFilenameForTrack(MusicTrack track)
    {
        return track switch
        {
            MusicTrack.MainMenu => "MainMenu.mp3",
            MusicTrack.Exploration => "Exploration.mp3",
            MusicTrack.Battle => "Battle.mp3",
            MusicTrack.Victory => "Victory.mp3",
            MusicTrack.GameOver => "GameOver.mp3",
            _ => null
        };
    }
    ```

- [ ] **Task 5.18**: Save MusicManager.cs

### Enable Music in BaseMainMenuPage

- [ ] **Task 5.19**: Open `GORE-Engine\src\Pages\BaseMainMenuPage.cs`

- [ ] **Task 5.20**: Uncomment music playback
  - In OnNavigatedTo, uncomment:
    ```csharp
    MusicManager.PlayMusic(MusicTrack.MainMenu);
    ```

- [ ] **Task 5.21**: Save BaseMainMenuPage.cs

### Phase 5 Validation - Build

- [ ] **Validation 5.1**: Build solution
- [ ] **Validation 5.2**: Confirm zero errors
- [ ] **Validation 5.3**: Confirm zero warnings (or only minor ones)

### Phase 5 Validation - Runtime

- [ ] **Validation 5.4**: Run application
- [ ] **Validation 5.5**: Confirm application enters **fullscreen** mode
- [ ] **Validation 5.6**: Confirm cursor is **hidden**
- [ ] **Validation 5.7**: Confirm background music plays (if MP3 files exist in Assets/Music)
  - If no music files, music should fail silently (OK)
- [ ] **Validation 5.8**: Confirm menu navigation still works
- [ ] **Validation 5.9**: Confirm can exit fullscreen with Alt+Enter (Windows feature)

- [ ] **Task 5.22**: Commit Phase 5 changes
  - Run: `git add .`
  - Run: `git commit -m "refactor: Update remaining UWP APIs to WinUI 3"`

**Phase 5 Complete** ✅ Proceed to Phase 6

---

## Phase 6: Testing and Validation

**Objective**: Comprehensive validation of migration  
**Estimated Time**: 2-4 hours

### Functional Testing - Application Lifecycle

- [ ] **Test 6.1**: Application launches without errors
- [ ] **Test 6.2**: Window appears and activates
- [ ] **Test 6.3**: Application enters fullscreen correctly
- [ ] **Test 6.4**: Can exit fullscreen with Alt+Enter (test restore)
- [ ] **Test 6.5**: Application closes cleanly (Alt+F4 or X button)

### Functional Testing - Main Menu

- [ ] **Test 6.6**: Main menu displays correctly
- [ ] **Test 6.7**: Cursor is hidden
- [ ] **Test 6.8**: Background music plays (if assets present)
- [ ] **Test 6.9**: Arrow Up/Down navigate menu
- [ ] **Test 6.10**: W/S keys navigate menu
- [ ] **Test 6.11**: Enter key activates selection
- [ ] **Test 6.12**: Space key activates selection
- [ ] **Test 6.13**: Menu wraps around correctly (top ↔ bottom)
- [ ] **Test 6.14**: Visual cursor/highlight updates correctly

### Functional Testing - Character Creation

- [ ] **Test 6.15**: New Game navigates to character creation
- [ ] **Test 6.16**: Character creation page displays
- [ ] **Test 6.17**: TextBox accepts character name input
- [ ] **Test 6.18**: Can type letters, numbers, spaces
- [ ] **Test 6.19**: Backspace deletes characters
- [ ] **Test 6.20**: Max length (20 chars) enforced
- [ ] **Test 6.21**: Left/Right or A/D navigate Confirm/Cancel
- [ ] **Test 6.22**: Enter activates selected button
- [ ] **Test 6.23**: Confirm creates character and navigates to game
- [ ] **Test 6.24**: Cancel returns to main menu
- [ ] **Test 6.25**: Escape returns to main menu

### Functional Testing - Game Page (if implemented)

- [ ] **Test 6.26**: Game page loads with character data
- [ ] **Test 6.27**: Exploration controls work (WASD or arrows)
- [ ] **Test 6.28**: Battle system input works
- [ ] **Test 6.29**: Escape key opens menu
- [ ] **Test 6.30**: Menu navigation works during gameplay
- [ ] **Test 6.31**: Save functionality works (if implemented)
- [ ] **Test 6.32**: Load Game from main menu works

### Functional Testing - Graphics/Rendering

- [ ] **Test 6.33**: Win2D canvases render correctly (if used)
- [ ] **Test 6.34**: No graphics corruption or artifacts
- [ ] **Test 6.35**: Animations are smooth
- [ ] **Test 6.36**: Frame rate acceptable (target 60 FPS)

### Functional Testing - Audio

- [ ] **Test 6.37**: Main menu music plays and loops
- [ ] **Test 6.38**: Victory/GameOver music doesn't loop (if tested)
- [ ] **Test 6.39**: No audio crackling or distortion
- [ ] **Test 6.40**: Volume control works (if implemented)

### Edge Case Testing

- [ ] **Test 6.41**: Rapid key presses don't cause crashes
- [ ] **Test 6.42**: Navigate between pages multiple times
- [ ] **Test 6.43**: Multiple fullscreen enter/exit cycles
- [ ] **Test 6.44**: Music files missing handled gracefully (silent failure)
- [ ] **Test 6.45**: Invalid save data shows error dialog
- [ ] **Test 6.46**: Character name at max length (20 chars) works

### Critical Bug Verification

- [ ] **Test 6.47**: **MOST IMPORTANT** - Confirm original AccessViolationException is **FIXED**
  - Run application multiple times
  - Confirm consistent successful launch
  - No native crashes on startup
  - No AccessViolationException in debug output

### Performance Validation

- [ ] **Test 6.48**: Measure application startup time
  - Time from launch to visible window: ______ seconds
  - Should be < 3 seconds

- [ ] **Test 6.49**: Check memory usage
  - Open Task Manager
  - Find MysticChronicles.exe
  - Note memory usage: ______ MB
  - Should be reasonable (< 500 MB idle)

- [ ] **Test 6.50**: Check CPU usage during idle
  - CPU usage should be < 5% when idle on menu

### Deployment Testing (MSIX Package)

- [ ] **Test 6.51**: Build MSIX package
  - Right-click MysticChronicles project
  - Publish → Create App Packages
  - Choose "Sideloading"
  - Select x64 architecture
  - Build package successfully

- [ ] **Test 6.52**: Install MSIX package
  - Navigate to output folder
  - Right-click .msix file → Install
  - Package installs without errors

- [ ] **Test 6.53**: Run installed application
  - Launch from Start menu
  - Confirm all functionality works
  - Test music playback (file access from package)
  - Test save/load if implemented

- [ ] **Test 6.54**: Uninstall package
  - Settings → Apps → Find Mystic Chronicles → Uninstall
  - Package uninstalls cleanly

### Documentation Updates

- [ ] **Task 6.55**: Update README.md (if exists)
  - Update system requirements: Windows 10 1809 or higher
  - Update .NET version: .NET 8
  - Update build instructions for WinUI 3
  - Note Visual Studio 2022 requirement

- [ ] **Task 6.56**: Create or update CHANGELOG
  - Document WinUI 3 migration
  - Note AccessViolationException fix
  - List any breaking changes (for developers, not users)

- [ ] **Task 6.57**: Update developer documentation
  - Document new input architecture
  - Note IKeyboardHandler interface usage
  - Explain Window-level event routing

### Create Test Report

- [ ] **Task 6.58**: Document test results
  - Create file: `winui3-migration-test-report.md`
  - List all tests performed
  - Note any failures or issues
  - Document performance metrics
  - Include screenshots if helpful

### Phase 6 Validation

- [ ] **Validation 6.1**: All critical tests pass
- [ ] **Validation 6.2**: Zero crashes during testing
- [ ] **Validation 6.3**: AccessViolationException confirmed fixed
- [ ] **Validation 6.4**: Performance acceptable
- [ ] **Validation 6.5**: MSIX package builds and installs successfully
- [ ] **Validation 6.6**: Documentation updated

- [ ] **Task 6.59**: Commit Phase 6 changes (documentation, test report)
  - Run: `git add .`
  - Run: `git commit -m "test: Complete WinUI 3 migration validation"`

**Phase 6 Complete** ✅ Migration Ready for Finalization

---

## Finalization

**Objective**: Finalize migration and merge to main branch

### Pre-Merge Checklist

- [ ] **Final Check 1**: All 6 phases completed
- [ ] **Final Check 2**: All tests passed
- [ ] **Final Check 3**: Zero compiler warnings (or only acceptable warnings documented)
- [ ] **Final Check 4**: Zero runtime errors
- [ ] **Final Check 5**: Performance meets requirements
- [ ] **Final Check 6**: Deployment package builds
- [ ] **Final Check 7**: Documentation updated
- [ ] **Final Check 8**: Original AccessViolationException bug confirmed fixed

### Merge to Master

- [ ] **Task F.1**: Review all commits
  - Run: `git log --oneline`
  - Verify commit messages are clear
  - Confirm all phases committed

- [ ] **Task F.2**: Push migration branch (if using remote)
  - Run: `git push origin winui3-migration`

- [ ] **Task F.3**: Create tag for migration completion
  - Run: `git tag winui3-migration-complete`
  - Run: `git tag -a v2.0-winui3 -m "WinUI 3 migration complete"`

- [ ] **Task F.4**: Merge to master
  - Run: `git checkout master`
  - Run: `git merge winui3-migration`
  - Resolve any conflicts (should be none if working only on migration branch)

- [ ] **Task F.5**: Verify master branch
  - Build solution on master
  - Run application from master
  - Confirm everything works

- [ ] **Task F.6**: Push to remote (if applicable)
  - Run: `git push origin master`
  - Run: `git push origin --tags`

### Archive UWP Version (Optional)

- [ ] **Task F.7**: Create archive branch of UWP version
  - Run: `git branch archive/uwp-version pre-winui3-migration`
  - Preserves original UWP code if ever needed

### Cleanup

- [ ] **Task F.8**: Delete migration branch (optional, after merge)
  - Run: `git branch -d winui3-migration`
  - Or keep for historical reference

### Final Deliverables Checklist

- [ ] **Deliverable 1**: WinUI 3 application running successfully
- [ ] **Deliverable 2**: All features migrated with feature parity
- [ ] **Deliverable 3**: Original bug (AccessViolationException) fixed
- [ ] **Deliverable 4**: Performance equal to or better than UWP
- [ ] **Deliverable 5**: Deployable MSIX package
- [ ] **Deliverable 6**: Updated documentation
- [ ] **Deliverable 7**: Test report
- [ ] **Deliverable 8**: Clean Git history with tagged releases

---

## 🎉 **WinUI 3 Migration Complete!** 🎉

**Congratulations!** You've successfully migrated Mystic Chronicles from UWP to WinUI 3.

### What You've Achieved:

✅ Modernized to WinUI 3 platform  
✅ Upgraded to .NET 8  
✅ Fixed AccessViolationException bug  
✅ Implemented new input architecture  
✅ Maintained all game features  
✅ Created deployable MSIX package  

### Next Steps:

- Continue game development on modern platform
- Leverage WinUI 3 features and controls
- Enjoy better tooling and Visual Studio integration
- Distribute via Microsoft Store or direct deployment

---

**Total Tasks**: 150+  
**Completion Date**: _______________  
**Migration Duration**: _______________  
**Notes**: _______________
