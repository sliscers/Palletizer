using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;

namespace QMIGenerator
{
    public class Core
    {
        public MainPage CorePage;

        public ContentControl ContentField;
        public TextBlock ContentTitle;

        public Item SelectedItem;
        public Pallet SelectedPallet;
        public List<List<Item>> Layers = new List<List<Item>>();

        public List<Controller> Controllers = new List<Controller>();
        private int ControllerIndex = 0;

        public Core(MainPage Page)
        {
            //Page to Control
            this.CorePage = Page;

            //Contentfield of the Page
            this.ContentField = ((ContentControl)(CorePage.FindName("PageContent")));
            this.ContentTitle = ((TextBlock)(CorePage.FindName("ContentTitle")));

            //Add Controllers to handle the UserControls
            this.Controllers.Add(new ControllerPalletSettings(this));
            this.Controllers.Add(new ControllerPalletDesigner(this));
            this.Controllers[0].ChangeContent();
        }
       public void SlideLeft()
        {
            if(ControllerIndex > 0)
            {
                ControllerIndex --;
                this.Controllers[ControllerIndex].ChangeContent();
            }
        }

        public void navTo(Type type)
        {
            foreach(Controller controller in Controllers)
            {
                if(controller.GetType() == type)
                {
                    controller.ChangeContent();
                }
            }
        }
        public void SlideRight()
        {
            if (ControllerIndex < (Controllers.Count - 1))
            {
                ControllerIndex++;
                this.Controllers[ControllerIndex].ChangeContent();
            }
        }
    }

    //Pallet Data
    public class Pallet
    {
        public String name;
        public int widthInCm;
        public int lengthInCm;
        public double maxWeightInKg;
        public int maxHeightInCm;
    }

    public class Item
    {
        public String name;
        public int widthInCm;
        public int lengthInCm;
        public double weightInKg;
        public int heightInCm;
        public double x, y;
        public int Rotation = 0;
    }
}
