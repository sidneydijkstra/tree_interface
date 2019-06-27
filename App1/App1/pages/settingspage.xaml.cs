using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App1.pages{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class settingspage : ContentPage{
		public settingspage (){
			InitializeComponent ();
            Setting_Tree tree = SettingsController.tree;
            entryNormalR.Text = tree.normal.r.ToString();
            entryNormalG.Text = tree.normal.g.ToString();
            entryNormalB.Text = tree.normal.b.ToString();

            entryBlinkR.Text = tree.blink.r.ToString();
            entryBlinkG.Text = tree.blink.g.ToString();
            entryBlinkB.Text = tree.blink.b.ToString();
        }

        private void clickResyncSettingsTree(object sender, EventArgs e){
            Setting_RGB _normal = new Setting_RGB() { r = 255, g = 255, b = 255 };
            int.TryParse(entryNormalR.Text, out _normal.r);
            int.TryParse(entryNormalG.Text, out _normal.g);
            int.TryParse(entryNormalB.Text, out _normal.b);
            Setting_RGB _blink = new Setting_RGB() { r = 255, g = 255, b = 255 };
            int.TryParse(entryBlinkR.Text, out _blink.r);
            int.TryParse(entryBlinkG.Text, out _blink.g);
            int.TryParse(entryBlinkB.Text, out _blink.b);
            
            SettingsController.resyncTree(new Setting_Tree() { normal = _normal, blink = _blink });
        }

        private void clickNavigateAdvancedSettings(object sender, EventArgs e) {
            this.Navigation.PushAsync(new advancedsettingspage());
        }
	}
}