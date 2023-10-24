using System.Threading;
using System.Windows;

namespace Dota2Randomer {
	public partial class App : Application {
		protected override void OnStartup(StartupEventArgs e) {
			Mutex _ = new Mutex(true, "DotaHeroGridGenerator", out bool createdNew);

			if(!createdNew) {
				Application.Current.Shutdown(); // Already running, exit.
			}

			base.OnStartup(e);
		}
	}
}
