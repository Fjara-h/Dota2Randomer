using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Threading;
using System.Windows;
using System.Windows.Input;

using WindowsInput;

using Dota2Randomer.Properties;

namespace Dota2Randomer {
	public partial class MainWindow : Window {

		// Total: 124;
		private readonly string[] Heroes = {
			// Strength
			"Alchemist","Axe","Bristleback","Centaur Warrunner","Chaos Knight","Dawnbreaker","Doom","Dragon Knight",
			"Earth Spirit","Earthshaker","Elder Titan","Huskar","Kunkka","Legion Commander","Lifestealer","Mars",
			"Night Stalker","Ogre Magi","Omniknight","Primal Beast","Pudge","Slardar","Spirit Breaker","Sven",
			"Tidehunter","Tiny","Treant Protector","Tusk","Underlord","Undying","Wraith King",
			// Agility
			"Anti-Mage","Arc Warden","Bloodseeker","Bounty Hunter","Clinkz","Drow Ranger","Ember Spirit","Faceless Void",
			"Gyrocopter","Hoodwink","Juggernaut","Luna","Medusa","Meepo","Monkey King","Morphling","Naga Siren",
			"Phantom Assassin","Phantom Lancer","Razor","Riki","Shadow Fiend","Slark","Sniper","Spectre",
			"Templar Assassin","Terrorblade","Troll Warlord","Ursa","Viper","Weaver",
			// Intelligence
			"Ancient Apparition","Crystal Maiden","Death Prophet","Disruptor","Enchantress","Grimstroke","Jakiro",
			"Keeper of the Light","Leshrac","Lich","Lina","Lion","Muerta","Nature's Prophet","Necrophos","Oracle",
			"Outworld Destroyer","Puck","Pugna","Queen of Pain","Rubick","Shadow Demon","Shadow Shaman","Silencer",
			"Skywrath Mage","Storm Spirit","Tinker","Warlock","Witch Doctor","Zeus",
			// Universal
			"Abaddon","Bane","Batrider","Beastmaster","Brewmaster","Broodmother","Chen","Clockwerk","Dark Seer",
			"Dark Willow","Dazzle","Enigma","Invoker","Io","Lone Druid","Lycan","Magnus","Marci","Mirana",
			"Nyx Assassin","Pangolier","Phoenix","Sand King","Snapfire","Techies","Timbersaw","Vengeful Spirit",
			"Venomancer","Visage","Void Spirit","Windranger","Winter Wyvern",
		};

	public MainWindow() {
			InitializeComponent();
			this.Top = Settings.Default.WindowLocation.Y;
			this.Left = Settings.Default.WindowLocation.X;
		}

		private void randomButton_Click(object sender, RoutedEventArgs e) {
			Process[] process = Process.GetProcessesByName("dota2");
			if(process.Length > 0 && process[0].ToString().Equals("System.Diagnostics.Process (dota2)", StringComparison.Ordinal)) {
				int heroNum = RandomNumberGenerator.GetInt32(0, Heroes.Length);
				string heroName = Heroes[heroNum];
				hoverHero(heroName, process[0]);
				RandomHero_Button.ToolTip = "Run Successful! Picked: " + heroName;
			} else {
				RandomHero_Button.ToolTip = "Dota 2 is not running.";
			}
		}

		// Bring dota to foreground and focus, potentially clear keyboard focus from chatbox, type heroName from heroList, select but don't pick
		private static void hoverHero(string heroName, Process process) {
			WindowHelper.bringProcessToFront(process);
			Thread.Sleep(Settings.Default.WaitTime);
			InputSimulator keySim = new();
			keySim.Keyboard.KeyPress(VirtualKeyCode.ESCAPE);
			keySim.Keyboard.KeyPress(VirtualKeyCode.BACK);
			keySim.Keyboard.TextEntry(heroName);
			keySim.Keyboard.KeyPress(VirtualKeyCode.RETURN);
		}

		// Event for minimizeButton - Minimizes window to taskbar
		private void minimizeButton_Click(object sender, RoutedEventArgs e) {
			this.WindowState = WindowState.Minimized;
		}

		// Event for closeButton - Reset randomButton tooltip, save settings, shutdown program
		private void closeButton_Click(object sender, RoutedEventArgs e) {
			RandomHero_Button.ToolTip = "";
			Settings.Default.WindowLocation = new System.Drawing.Point((int)Top, (int)Left);
			Settings.Default.Save();
			Application.Current.Shutdown();
		}

		// If closed from a different source, this still shuts down the program properly
		protected override void OnClosed(EventArgs e) {
			base.OnClosed(e);
			Application.Current.Shutdown();
		}

		// Click and dragging on any non-active control drags window
		protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e) {
			base.OnMouseLeftButtonDown(e);
			this.DragMove();
		}
	}
	// Utilities for window controls
	public static class WindowHelper {

		private const int SW_RESTORE = 9;

		public static void bringProcessToFront(Process process) {
			if(IsIconic(process.MainWindowHandle)) {
				ShowWindow(process.MainWindowHandle, SW_RESTORE);
			}
			SetForegroundWindow(process.MainWindowHandle);
		}

		[DllImport("User32.dll")]
		private static extern bool SetForegroundWindow(IntPtr handle);
		[DllImport("User32.dll")]
		private static extern bool ShowWindow(IntPtr handle, int nCmdShow);
		[DllImport("User32.dll")]
		private static extern bool IsIconic(IntPtr handle);
	}
}