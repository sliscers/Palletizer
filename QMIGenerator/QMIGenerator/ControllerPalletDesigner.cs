using System;
using System.Collections.Generic;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Shapes;

namespace QMIGenerator
{
    public class ControllerPalletDesigner : Controller
    {
        public MainPage CorePage;
        public PalletDesigner Content;
        public Core Core;

        public ContentControl ContentField;
        public TextBlock ContentTitle;

        public ControllerPalletDesigner(Core Core)
        {
            this.Core = Core;
            this.CorePage = Core.CorePage;
            this.Content = new PalletDesigner(this);

            

            //Contentfield of the Page
            this.ContentField = ((ContentControl)(CorePage.FindName("PageContent")));
            this.ContentTitle = ((TextBlock)(CorePage.FindName("ContentTitle")));

        }
        public override void ChangeContent()
        {
            if (Core.SelectedItem != null && Core.SelectedPallet != null)
            {
                Content.Pallet.Children.Clear();
                ContentField.Content = Content;
                Content.CreateVisualPallet();

                //Setup first pallet layer
                Core.Layers.Clear();
                Core.Layers.Add(new List<Item>());

                ((ComboBox)(Content.FindName("ComboBoxSelectedLayer"))).Items.Clear();
                ((ComboBox)(Content.FindName("ComboBoxSelectedLayer"))).Items.Add("Layer 1");
                ((ComboBox)(Content.FindName("ComboBoxSelectedLayer"))).SelectedIndex = 0;
            }
        }
        public Item AddItem(int layer, double x, double y)
        {
            Core.Layers[layer].Add(new Item()
            {
                x = x,
                y = y,
                lengthInCm = Core.SelectedItem.lengthInCm,
                widthInCm = Core.SelectedItem.widthInCm,
                Rotation = 0
            });
            return Core.Layers[layer][Core.Layers[layer].Count - 1];
        }
        public void AddLayer()
        {
            Core.Layers.Add(new List<Item>());
            
            int tempCounter = ((ComboBox)(Content.FindName("ComboBoxSelectedLayer"))).Items.Count;
            Content.changeLayer(tempCounter);
            ((ComboBox)(Content.FindName("ComboBoxSelectedLayer"))).Items.Add("Layer " + (1 + tempCounter));
            ((ComboBox)(Content.FindName("ComboBoxSelectedLayer"))).SelectedIndex = tempCounter;
        }
    }
}
