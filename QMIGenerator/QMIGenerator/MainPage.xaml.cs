using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace QMIGenerator
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public partial class MainPage : Page
    {
        //CoreController
        public Core Core;

        public MainPage()
        {
            this.InitializeComponent();

            //Create the CoreController for this page.
            Core = new Core(this);
        }

        private void NavLeft_Click(object sender, RoutedEventArgs e)
        {
            Core.SlideLeft();
        }

        private void NavRight_Click(object sender, RoutedEventArgs e)
        {
            Core.SlideRight();
        }

        private void button_Copy_Click(object sender, RoutedEventArgs e)
        {
            Core.navTo(typeof(ControllerPalletDesigner));
        }

        private void button_Copy1_Click(object sender, RoutedEventArgs e)
        {
            Core.navTo(typeof(ControllerPalletSettings));
        }
    }
}
