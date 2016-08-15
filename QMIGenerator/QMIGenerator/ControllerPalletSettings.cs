using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;

namespace QMIGenerator
{
    public class ControllerPalletSettings : Controller
    {
        public MainPage CorePage;
        public PalletSettings Content;
        public Core Core;

        public ContentControl ContentField;
        public TextBlock ContentTitle;

        public List<Pallet> Pallets = new List<Pallet>();
        public List<Item> Items = new List<Item>();

        public ControllerPalletSettings(Core Core)
        {
            this.Core = Core;
            this.CorePage = Core.CorePage;
            this.Content = new PalletSettings(this);

            //Check Files Init

            //Contentfield of the Page
            this.ContentField = ((ContentControl)(CorePage.FindName("PageContent")));
            this.ContentTitle = ((TextBlock)(CorePage.FindName("ContentTitle")));

            //Load Data into Forms
            loadForms();

        }
        public async Task loadForms()
        {
            await InitItems();
            await InitPallets();
            UpdateData();
        }
        public override void ChangeContent()
        {
            ContentField.Content = Content;
            //ContentTitle.Text = Content.Title;

            Content.EnableDisableNextBtn();
        }

        public async void UpdateData()
        {
            int temp;
            //PALLETS
            Pallets.Clear();
            Windows.Storage.StorageFolder storageFolder = await Windows.Storage.ApplicationData.Current.LocalFolder.GetFolderAsync("Settings");
            Windows.Storage.StorageFile storageFile = await storageFolder.GetFileAsync("Pallets.xml");

            //LOAD SETTINGS?
            Windows.Data.Xml.Dom.XmlLoadSettings loadSettings = new Windows.Data.Xml.Dom.XmlLoadSettings();
            loadSettings.ProhibitDtd = false;
            loadSettings.ResolveExternals = false;

            //ACTUAL LOADING FILE??
            Windows.Data.Xml.Dom.XmlDocument test = await Windows.Data.Xml.Dom.XmlDocument.LoadFromFileAsync(storageFile, loadSettings);

            //SELECTING DATA
            Windows.Data.Xml.Dom.XmlNodeList list = test.SelectNodes("/PALLETS/PALLET");
            foreach(Windows.Data.Xml.Dom.IXmlNode node in list)
            {
                Pallets.Add(new Pallet
                {
                    name = node.Attributes.GetNamedItem("name").NodeValue.ToString(),
                    widthInCm = int.Parse(node.Attributes.GetNamedItem("width").NodeValue.ToString()),
                    lengthInCm = int.Parse(node.Attributes.GetNamedItem("length").NodeValue.ToString()),
                    maxHeightInCm = int.Parse(node.Attributes.GetNamedItem("maxheight").NodeValue.ToString()),
                    maxWeightInKg = double.Parse(node.Attributes.GetNamedItem("maxweight").NodeValue.ToString())
                });
            }

            //Update data in Forms
            temp = ((ComboBox)(Content.FindName("ComboBoxPallet"))).SelectedIndex;
            ((ComboBox)(Content.FindName("ComboBoxPallet"))).Items.Clear();
            foreach (Pallet Pallet in Pallets)
            {
                ((ComboBox)(Content.FindName("ComboBoxPallet"))).Items.Add(Pallet.name);
            }
            ((ComboBox)(Content.FindName("ComboBoxPallet"))).SelectedIndex = (Pallets.Count <= temp ? -1 : temp);
            
            // ITEMS
            Items.Clear();
            storageFolder = await Windows.Storage.ApplicationData.Current.LocalFolder.GetFolderAsync("Settings");
            storageFile = await storageFolder.GetFileAsync("Items.xml");


            //ACTUAL LOADING FILE??
            test = await Windows.Data.Xml.Dom.XmlDocument.LoadFromFileAsync(storageFile, loadSettings);

            //SELECTING DATA
            list = test.SelectNodes("/ITEMS/ITEM");
            foreach (Windows.Data.Xml.Dom.IXmlNode node in list)
            {
                Items.Add(new Item
                {
                    name = node.Attributes.GetNamedItem("name").NodeValue.ToString(),
                    widthInCm = int.Parse(node.Attributes.GetNamedItem("width").NodeValue.ToString()),
                    lengthInCm = int.Parse(node.Attributes.GetNamedItem("length").NodeValue.ToString()),
                    heightInCm = int.Parse(node.Attributes.GetNamedItem("height").NodeValue.ToString()),
                    weightInKg = double.Parse(node.Attributes.GetNamedItem("weight").NodeValue.ToString())
                });
            }

            //Update data in Forms
            temp = ((ComboBox)(Content.FindName("ComboBoxItem"))).SelectedIndex;
            ((ComboBox)(Content.FindName("ComboBoxItem"))).Items.Clear();
            foreach (Item Pallet in Items)
            {
                ((ComboBox)(Content.FindName("ComboBoxItem"))).Items.Add(Pallet.name);
            }
            ((ComboBox)(Content.FindName("ComboBoxItem"))).SelectedIndex = (Pallets.Count <= temp ? -1 : temp);

        }

        public async Task InitPallets()
        {

            //Get status of the files
            Windows.Storage.StorageFolder sf = await Windows.Storage.ApplicationData.Current.LocalFolder.CreateFolderAsync("Settings", Windows.Storage.CreationCollisionOption.OpenIfExists);
            var var = await sf.TryGetItemAsync("Pallets.xml");

            //If the status is "Does not exists." Generate Files and Folders
            if (var == null)
            {
                //Copy Files from Installation Folder
                Windows.Storage.StorageFolder storageFolder = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync("Settings");
                Windows.Storage.StorageFile storageFile = await storageFolder.GetFileAsync("Pallets.xml");
                Windows.Data.Xml.Dom.XmlLoadSettings loadSettings = new Windows.Data.Xml.Dom.XmlLoadSettings();
                loadSettings.ProhibitDtd = false;
                loadSettings.ResolveExternals = false;
                Windows.Data.Xml.Dom.XmlDocument doc = await Windows.Data.Xml.Dom.XmlDocument.LoadFromFileAsync(storageFile, loadSettings);

                //Copy to LocalStorage Folder "AppData"
                Windows.Storage.StorageFile st = await sf.CreateFileAsync("Pallets.xml",Windows.Storage.CreationCollisionOption.ReplaceExisting);
                await doc.SaveToFileAsync(st);
            }
        }
        public async Task InitItems()
        {

            //Get status of the files

            Windows.Storage.StorageFolder sf = await Windows.Storage.ApplicationData.Current.LocalFolder.CreateFolderAsync("Settings", Windows.Storage.CreationCollisionOption.OpenIfExists);
            var var = await sf.TryGetItemAsync("Items.xml");

            //If the status is "Does not exists." Generate Files and Folders
            if (var == null)
            {
                //Copy Files from Installation Folder
                Windows.Storage.StorageFolder storageFolder = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync("Settings");
                Windows.Storage.StorageFile storageFile = await storageFolder.GetFileAsync("Items.xml");
                Windows.Data.Xml.Dom.XmlLoadSettings loadSettings = new Windows.Data.Xml.Dom.XmlLoadSettings();
                loadSettings.ProhibitDtd = false;
                loadSettings.ResolveExternals = false;
                Windows.Data.Xml.Dom.XmlDocument doc = await Windows.Data.Xml.Dom.XmlDocument.LoadFromFileAsync(storageFile, loadSettings);

                //Copy to LocalStorage Folder "AppData"
                Windows.Storage.StorageFile st = await sf.CreateFileAsync("Items.xml", Windows.Storage.CreationCollisionOption.ReplaceExisting);
                await doc.SaveToFileAsync(st);
            }
        }
        public async void AddPallet(Pallet Pallet)
        {
            Windows.Storage.StorageFolder storageFolder = await Windows.Storage.ApplicationData.Current.LocalFolder.GetFolderAsync("Settings");
            Windows.Storage.StorageFile storageFile = await storageFolder.GetFileAsync("Pallets.xml");

            //LOAD SETTINGS?
            Windows.Data.Xml.Dom.XmlLoadSettings loadSettings = new Windows.Data.Xml.Dom.XmlLoadSettings();
            loadSettings.ProhibitDtd = false;
            loadSettings.ResolveExternals = false;

            //ACTUAL LOADING FILE??
            Windows.Data.Xml.Dom.XmlDocument test = await Windows.Data.Xml.Dom.XmlDocument.LoadFromFileAsync(storageFile, loadSettings);

            //SELECTING DATA
            
            Windows.Data.Xml.Dom.XmlElement node;
            Windows.Data.Xml.Dom.IXmlNode list = test.SelectSingleNode("/PALLETS");
            node = test.CreateElement("PALLET");
            node.SetAttribute("name", Pallet.name);
            node.SetAttribute("width", Pallet.widthInCm.ToString());
            node.SetAttribute("length", Pallet.lengthInCm.ToString());
            node.SetAttribute("maxweight", Pallet.maxWeightInKg.ToString());
            node.SetAttribute("maxheight", Pallet.maxHeightInCm.ToString());
            list.AppendChild(node);


            Windows.Storage.StorageFolder sf = await Windows.Storage.ApplicationData.Current.LocalFolder.GetFolderAsync("Settings");
            Windows.Storage.StorageFile st = await sf.CreateFileAsync("Pallets.xml", Windows.Storage.CreationCollisionOption.ReplaceExisting);
            await test.SaveToFileAsync(st);
            UpdateData();
        }
        public async void ChangePallet(Pallet Pallet)
        {
            Windows.Storage.StorageFolder storageFolder = await Windows.Storage.ApplicationData.Current.LocalFolder.GetFolderAsync("Settings");
            Windows.Storage.StorageFile storageFile = await storageFolder.GetFileAsync("Pallets.xml");

            //LOAD SETTINGS?
            Windows.Data.Xml.Dom.XmlLoadSettings loadSettings = new Windows.Data.Xml.Dom.XmlLoadSettings();
            loadSettings.ProhibitDtd = false;
            loadSettings.ResolveExternals = false;

            //ACTUAL LOADING FILE??
            Windows.Data.Xml.Dom.XmlDocument test = await Windows.Data.Xml.Dom.XmlDocument.LoadFromFileAsync(storageFile, loadSettings);

            //SELECTING DATA
            Windows.Data.Xml.Dom.XmlElement node;
            node = ((Windows.Data.Xml.Dom.XmlElement)(test.SelectNodes("/PALLETS/PALLET")[((ComboBox)(Content.FindName("ComboBoxPallet"))).SelectedIndex]));
            node.SetAttribute("name", Pallet.name);
            node.SetAttribute("width", Pallet.widthInCm.ToString());
            node.SetAttribute("length", Pallet.lengthInCm.ToString());
            node.SetAttribute("maxweight", Pallet.maxWeightInKg.ToString());
            node.SetAttribute("maxheight", Pallet.maxHeightInCm.ToString());


            Windows.Storage.StorageFolder sf = await Windows.Storage.ApplicationData.Current.LocalFolder.GetFolderAsync("Settings");
            Windows.Storage.StorageFile st = await sf.CreateFileAsync("Pallets.xml", Windows.Storage.CreationCollisionOption.ReplaceExisting);
            await test.SaveToFileAsync(st);
            UpdateData();
        }
        public async void DeletePallet(int index)
        {
            Windows.Storage.StorageFolder storageFolder = await Windows.Storage.ApplicationData.Current.LocalFolder.GetFolderAsync("Settings");
            Windows.Storage.StorageFile storageFile = await storageFolder.GetFileAsync("Pallets.xml");

            //LOAD SETTINGS?
            Windows.Data.Xml.Dom.XmlLoadSettings loadSettings = new Windows.Data.Xml.Dom.XmlLoadSettings();
            loadSettings.ProhibitDtd = false;
            loadSettings.ResolveExternals = false;

            //ACTUAL LOADING FILE??
            Windows.Data.Xml.Dom.XmlDocument test = await Windows.Data.Xml.Dom.XmlDocument.LoadFromFileAsync(storageFile, loadSettings);

            //SELECTING DATA
            test.SelectSingleNode("/PALLETS").RemoveChild(test.SelectNodes("/PALLETS/PALLET")[index]);
            Windows.Storage.StorageFolder sf = await Windows.Storage.ApplicationData.Current.LocalFolder.GetFolderAsync("Settings");
            Windows.Storage.StorageFile st = await sf.CreateFileAsync("Pallets.xml", Windows.Storage.CreationCollisionOption.ReplaceExisting);
            await test.SaveToFileAsync(st);
            UpdateData();
        }
        public async void AddItem(Item Pallet)
        {
            Windows.Storage.StorageFolder storageFolder = await Windows.Storage.ApplicationData.Current.LocalFolder.GetFolderAsync("Settings");
            Windows.Storage.StorageFile storageFile = await storageFolder.GetFileAsync("Items.xml");

            //LOAD SETTINGS?
            Windows.Data.Xml.Dom.XmlLoadSettings loadSettings = new Windows.Data.Xml.Dom.XmlLoadSettings();
            loadSettings.ProhibitDtd = false;
            loadSettings.ResolveExternals = false;

            //ACTUAL LOADING FILE??
            Windows.Data.Xml.Dom.XmlDocument test = await Windows.Data.Xml.Dom.XmlDocument.LoadFromFileAsync(storageFile, loadSettings);

            //SELECTING DATA
            Windows.Data.Xml.Dom.IXmlNode list = test.SelectSingleNode("/ITEMS");
            Windows.Data.Xml.Dom.XmlElement node = test.CreateElement("ITEM");
            node.SetAttribute("name", Pallet.name);
            node.SetAttribute("width", Pallet.widthInCm.ToString());
            node.SetAttribute("length", Pallet.lengthInCm.ToString());
            node.SetAttribute("weight", Pallet.weightInKg.ToString());
            node.SetAttribute("height", Pallet.heightInCm.ToString());

            list.AppendChild(node);
            Windows.Storage.StorageFolder sf = await Windows.Storage.ApplicationData.Current.LocalFolder.GetFolderAsync("Settings");
            Windows.Storage.StorageFile st = await sf.CreateFileAsync("Items.xml", Windows.Storage.CreationCollisionOption.ReplaceExisting);
            await test.SaveToFileAsync(st);
            UpdateData();
        }
        public async void DeleteItem(int index)
        {
            Windows.Storage.StorageFolder storageFolder = await Windows.Storage.ApplicationData.Current.LocalFolder.GetFolderAsync("Settings");
            Windows.Storage.StorageFile storageFile = await storageFolder.GetFileAsync("Items.xml");

            //LOAD SETTINGS?
            Windows.Data.Xml.Dom.XmlLoadSettings loadSettings = new Windows.Data.Xml.Dom.XmlLoadSettings();
            loadSettings.ProhibitDtd = false;
            loadSettings.ResolveExternals = false;

            //ACTUAL LOADING FILE??
            Windows.Data.Xml.Dom.XmlDocument test = await Windows.Data.Xml.Dom.XmlDocument.LoadFromFileAsync(storageFile, loadSettings);

            //SELECTING DATA
            test.SelectSingleNode("/ITEMS").RemoveChild(test.SelectNodes("/ITEMS/ITEM")[index]);
            Windows.Storage.StorageFolder sf = await Windows.Storage.ApplicationData.Current.LocalFolder.GetFolderAsync("Settings");
            Windows.Storage.StorageFile st = await sf.CreateFileAsync("Items.xml", Windows.Storage.CreationCollisionOption.ReplaceExisting);
            await test.SaveToFileAsync(st);
            UpdateData();
        }
        public async void ChangeItem(Item Pallet)
        {
            Windows.Storage.StorageFolder storageFolder = await Windows.Storage.ApplicationData.Current.LocalFolder.GetFolderAsync("Settings");
            Windows.Storage.StorageFile storageFile = await storageFolder.GetFileAsync("Items.xml");

            //LOAD SETTINGS?
            Windows.Data.Xml.Dom.XmlLoadSettings loadSettings = new Windows.Data.Xml.Dom.XmlLoadSettings();
            loadSettings.ProhibitDtd = false;
            loadSettings.ResolveExternals = false;

            //ACTUAL LOADING FILE??
            Windows.Data.Xml.Dom.XmlDocument test = await Windows.Data.Xml.Dom.XmlDocument.LoadFromFileAsync(storageFile, loadSettings);

            //SELECTING DATA

            Windows.Data.Xml.Dom.XmlElement node;
            node = ((Windows.Data.Xml.Dom.XmlElement)(test.SelectNodes("/ITEMS/ITEM")[((ComboBox)(Content.FindName("ComboBoxItem"))).SelectedIndex]));
            node.SetAttribute("name", Pallet.name);
            node.SetAttribute("width", Pallet.widthInCm.ToString());
            node.SetAttribute("length", Pallet.lengthInCm.ToString());
            node.SetAttribute("weight", Pallet.weightInKg.ToString());
            node.SetAttribute("height", Pallet.heightInCm.ToString());


            Windows.Storage.StorageFolder sf = await Windows.Storage.ApplicationData.Current.LocalFolder.GetFolderAsync("Settings");
            Windows.Storage.StorageFile st = await sf.CreateFileAsync("Items.xml", Windows.Storage.CreationCollisionOption.ReplaceExisting);
            await test.SaveToFileAsync(st);
            UpdateData();
        }
    }
}
