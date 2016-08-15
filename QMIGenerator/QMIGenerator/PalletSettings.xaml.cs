using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace QMIGenerator
{
    public sealed partial class PalletSettings : UserControl
    {
        public String Title = "Pallet Settings";
        private ControllerPalletSettings Controller;
        public PalletSettings(ControllerPalletSettings Controller)
        {
            this.InitializeComponent();
            this.Controller = Controller;
        }
        //Update data in Forms
        private void ComboBoxPallet_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((ComboBox)(sender)).SelectedIndex != -1)
            {
                Pallet Pallet = Controller.Pallets[((ComboBox)(sender)).SelectedIndex];

                Controller.CorePage.Core.SelectedPallet = Pallet;

                TxtBoxPalletWidth.Text = Pallet.widthInCm.ToString();
                TxtBoxPalletLength.Text = Pallet.lengthInCm.ToString();
                TxtBoxPalletMaxWeight.Text = Pallet.maxWeightInKg.ToString();
                TxtBoxPalletMaxHeight.Text = Pallet.maxHeightInCm.ToString();
            }
            else
            {
                Controller.CorePage.Core.SelectedPallet = null;

                TxtBoxPalletWidth.Text = "";
                TxtBoxPalletLength.Text = "";
                TxtBoxPalletMaxWeight.Text = "";
                TxtBoxPalletMaxHeight.Text = "";
            }
            EnableDisableNextBtn();
        }

        public void EnableDisableNextBtn()
        {
            if(ComboBoxItem.SelectedIndex != -1 & ComboBoxPallet.SelectedIndex != -1)
            {
                //((Button)(Controller.CorePage.FindName("NavRight"))).IsEnabled = true;
            }
            else
            {
                //((Button)(Controller.CorePage.FindName("NavRight"))).IsEnabled = false;
            }
        }

        private void BtnPalletNew_Click(object sender, RoutedEventArgs e)
        {
            ContentDialogNewPallet dialog = new ContentDialogNewPallet();
            dialog.PrimaryButtonClick += Dialog_AddPallet;
            var result = dialog.ShowAsync();
        }

        private void Dialog_AddPallet(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            Controller.AddPallet(new Pallet {
                name = ((TextBox)(sender.FindName("textBoxName"))).Text,
                widthInCm = int.Parse(((TextBox)(sender.FindName("textBoxWidth"))).Text),
                lengthInCm = int.Parse(((TextBox)(sender.FindName("textBoxLength"))).Text),
                maxHeightInCm = int.Parse(((TextBox)(sender.FindName("textBoxHeight"))).Text),
                maxWeightInKg = double.Parse(((TextBox)(sender.FindName("textBoxWeight"))).Text)
            });
        }

        private void BtnPalletDelete_Click(object sender, RoutedEventArgs e)
        {
            Controller.DeletePallet(ComboBoxPallet.SelectedIndex);
        }

        private void textBlock8_SelectionChanged(object sender, RoutedEventArgs e)
        {

        }

        private void ComboBoxItem_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(((ComboBox)(sender)).SelectedIndex != -1)
            {
                Item Pallet = Controller.Items[((ComboBox)(sender)).SelectedIndex];

                Controller.CorePage.Core.SelectedItem = Pallet;

                TxtBoxItemWidth.Text = Pallet.widthInCm.ToString();
                TxtBoxItemLength.Text = Pallet.lengthInCm.ToString();
                TxtBoxItemWeight.Text = Pallet.weightInKg.ToString();
                TxtBoxItemHeight.Text = Pallet.heightInCm.ToString();

            }
            else
            {
                Controller.CorePage.Core.SelectedItem = null;
                TxtBoxItemWidth.Text = "";
                TxtBoxItemLength.Text = "";
                TxtBoxItemWeight.Text = "";
                TxtBoxItemHeight.Text = "";
            }
            EnableDisableNextBtn();
        }

        private void BtnPalletNew1_Click(object sender, RoutedEventArgs e)
        {
            ContentDialogNewItem dialog = new ContentDialogNewItem();
            dialog.PrimaryButtonClick += Dialog_AddItem;
            var result = dialog.ShowAsync();
        }
        private void Dialog_AddItem(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            Controller.AddItem(new Item
            {
                name = ((TextBox)(sender.FindName("textBoxName"))).Text,
                widthInCm = int.Parse(((TextBox)(sender.FindName("textBoxWidth"))).Text),
                lengthInCm = int.Parse(((TextBox)(sender.FindName("textBoxLength"))).Text),
                heightInCm = int.Parse(((TextBox)(sender.FindName("textBoxHeight"))).Text),
                weightInKg = double.Parse(((TextBox)(sender.FindName("textBoxWeight"))).Text)
            });
        }

        private void BtnItemDelete_Click(object sender, RoutedEventArgs e)
        {
            Controller.DeleteItem(ComboBoxItem.SelectedIndex);
        }

        private void BtnPalletChange_Click(object sender, RoutedEventArgs e)
        {
            if (ComboBoxPallet.SelectedIndex != -1)
            {
                ContentDialogChangePallet dialog = new ContentDialogChangePallet();
                ((TextBox)(dialog.FindName("textBoxName"))).Text = Controller.Pallets[ComboBoxPallet.SelectedIndex].name;
                ((TextBox)(dialog.FindName("textBoxWidth"))).Text = Controller.Pallets[ComboBoxPallet.SelectedIndex].widthInCm.ToString();
                ((TextBox)(dialog.FindName("textBoxLength"))).Text = Controller.Pallets[ComboBoxPallet.SelectedIndex].lengthInCm.ToString();
                ((TextBox)(dialog.FindName("textBoxHeight"))).Text = Controller.Pallets[ComboBoxPallet.SelectedIndex].maxHeightInCm.ToString();
                ((TextBox)(dialog.FindName("textBoxWeight"))).Text = Controller.Pallets[ComboBoxPallet.SelectedIndex].maxWeightInKg.ToString();

                dialog.PrimaryButtonClick += Dialog_ChangePallet;
                var result = dialog.ShowAsync();
                
            }
        }
        private void Dialog_ChangePallet(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            Controller.ChangePallet(new Pallet
            {
                name = ((TextBox)(sender.FindName("textBoxName"))).Text,
                widthInCm = int.Parse(((TextBox)(sender.FindName("textBoxWidth"))).Text),
                lengthInCm = int.Parse(((TextBox)(sender.FindName("textBoxLength"))).Text),
                maxHeightInCm = int.Parse(((TextBox)(sender.FindName("textBoxHeight"))).Text),
                maxWeightInKg = double.Parse(((TextBox)(sender.FindName("textBoxWeight"))).Text)
            });
        }
        private void BtnItemChange_Click(object sender, RoutedEventArgs e)
        {
            if (ComboBoxItem.SelectedIndex != -1)
            {
                ContentDialogChangeItem dialog = new ContentDialogChangeItem();
                ((TextBox)(dialog.FindName("textBoxName"))).Text = Controller.Items[ComboBoxItem.SelectedIndex].name;
                ((TextBox)(dialog.FindName("textBoxWidth"))).Text = Controller.Items[ComboBoxItem.SelectedIndex].widthInCm.ToString();
                ((TextBox)(dialog.FindName("textBoxLength"))).Text = Controller.Items[ComboBoxItem.SelectedIndex].lengthInCm.ToString();
                ((TextBox)(dialog.FindName("textBoxHeight"))).Text = Controller.Items[ComboBoxItem.SelectedIndex].heightInCm.ToString();
                ((TextBox)(dialog.FindName("textBoxWeight"))).Text = Controller.Items[ComboBoxItem.SelectedIndex].weightInKg.ToString();

                dialog.PrimaryButtonClick += Dialog_ChangeItem;
                var result = dialog.ShowAsync();
            }
        }
        private void Dialog_ChangeItem(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            Controller.ChangeItem(new Item
            {
                name = ((TextBox)(sender.FindName("textBoxName"))).Text,
                widthInCm = int.Parse(((TextBox)(sender.FindName("textBoxWidth"))).Text),
                lengthInCm = int.Parse(((TextBox)(sender.FindName("textBoxLength"))).Text),
                heightInCm = int.Parse(((TextBox)(sender.FindName("textBoxHeight"))).Text),
                weightInKg = double.Parse(((TextBox)(sender.FindName("textBoxWeight"))).Text)
            });
        }
    }
}
