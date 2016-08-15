using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace QMIGenerator
{
    public sealed partial class PalletDesigner : UserControl
    {
        public String Title = "Pallet Designer";
        private ControllerPalletDesigner Controller;

        private Point FirstPoint;
        private Point FirstPointItem;
        private Rectangle drag = null;
        private Rectangle selected = null;

        public Double ScreenFactor = 1;

        public PalletDesigner(ControllerPalletDesigner Controller)
        {
            this.InitializeComponent();
            this.Controller = Controller;
        }

        public void AddItemToPallet(Item Item)
        {

            Rectangle temp = new Rectangle()
            {
                Width = Item.widthInCm * ScreenFactor,
                Height = Item.lengthInCm * ScreenFactor
            };
            LinearGradientBrush test = new LinearGradientBrush();
            test.StartPoint = new Point();
            test.EndPoint = new Point();
            test.ColorInterpolationMode = ColorInterpolationMode.
            test.GradientStops.Add(new GradientStop() { Offset = 0.1, Color = Colors.Red });
            test.GradientStops.Add(new GradientStop() { Offset = 0, Color = Colors.Black });
            temp.Fill = test;

            switch (Item.Rotation)
            {
                case 0:
                    ((LinearGradientBrush)(temp.Fill)).StartPoint = new Point(0, 0);
                    ((LinearGradientBrush)(temp.Fill)).EndPoint = new Point(0, 1);
                    break;
                case 1:
                    ((LinearGradientBrush)(temp.Fill)).StartPoint = new Point(1, 0);
                    ((LinearGradientBrush)(temp.Fill)).EndPoint = new Point(0, 0);
                    break;
                case 2:
                    ((LinearGradientBrush)(temp.Fill)).StartPoint = new Point(0, 1);
                    ((LinearGradientBrush)(temp.Fill)).EndPoint = new Point(0, 0);
                    break;
                case 3:
                    ((LinearGradientBrush)(temp.Fill)).StartPoint = new Point(0, 0);
                    ((LinearGradientBrush)(temp.Fill)).EndPoint = new Point(1, 0);
                    break;
            }
            temp.Stroke = new SolidColorBrush(Colors.Black);
            temp.PointerPressed += Rectangle_PointerPressed;
            Canvas.SetLeft(temp, Item.x * ScreenFactor);
            Canvas.SetTop(temp, Item.y * ScreenFactor);

            Pallet.Children.Add(temp);
        }

        private void CorePage_SizeChanged(object sender, Windows.UI.Xaml.SizeChangedEventArgs e)
        {
            CreateVisualPallet();
        }

        public void CreateVisualPallet()
        {
            if (Controller.Core.SelectedPallet != null)
            {
                double tempY, tempX;
                Pallet Pallet2 = ((Pallet)(Controller.Core.SelectedPallet));

                //Update text on the right for the position
                tempY = Controller.Content.test123.ActualHeight / Pallet2.lengthInCm;
                tempX = (Controller.ContentField.ActualWidth / 22 * 16) / Pallet2.widthInCm;

                foreach (Rectangle item in Pallet.Children)
                {
                    Canvas.SetLeft(item, Canvas.GetLeft(item) / ScreenFactor);
                    Canvas.SetTop(item, Canvas.GetTop(item) / ScreenFactor);
                }

                //Calculate 
                if (tempY <= tempX)
                {
                    ScreenFactor = tempY;
                }
                else if (tempY > tempX)
                {

                    ScreenFactor = tempX;
                }
                foreach (Rectangle item in Pallet.Children)
                {
                    Canvas.SetLeft(item, Canvas.GetLeft(item) * ScreenFactor);
                    Canvas.SetTop(item, Canvas.GetTop(item) * ScreenFactor);
                }

                foreach (Rectangle item in Pallet.Children)
                {
                    item.Width = ((Item)(Controller.Core.SelectedItem)).widthInCm * ScreenFactor;
                    item.Height = ((Item)(Controller.Core.SelectedItem)).lengthInCm * ScreenFactor;
                }

                Pallet.Width = Pallet2.widthInCm * ScreenFactor;
                Pallet.Height = Pallet2.lengthInCm * ScreenFactor;
            }
        }


        private void Rectangle_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            if (drag == null)
            {
                drag = ((Rectangle)(sender));
                FirstPointItem.X = Canvas.GetLeft(drag);
                FirstPointItem.Y = Canvas.GetTop(drag);
                FirstPoint = e.GetCurrentPoint(Pallet).Position;
                drag.PointerMoved += MoveItem;
                drag.PointerReleased += Rectangle_PointerCanceled;

                drag.PointerExited += Rectangle_PointerCanceled;
            }
            if (selected != null)
                ((LinearGradientBrush)(selected.Fill)).GradientStops[0].Color = Colors.Red;
            selected = ((Rectangle)(sender));
            ((LinearGradientBrush)(selected.Fill)).GradientStops[0].Color = Colors.Blue;
        }
        private void MoveItem(object sender, PointerRoutedEventArgs e)
        {
            Double TempHeight = Math.Floor(FirstPointItem.Y + e.GetCurrentPoint(Pallet).Position.Y - FirstPoint.Y);
            Double TempWidth = Math.Floor(FirstPointItem.X + e.GetCurrentPoint(Pallet).Position.X - FirstPoint.X);
            XPosItem.Text = Canvas.GetLeft(drag).ToString();
            YPosItem.Text = Canvas.GetTop(drag).ToString();

            Rect RectOrg = new Rect()
            {
                X = TempWidth,
                Y = TempHeight,
                Width = drag.Width,
                Height = drag.Height
            };

            Rect Border = new Rect()
            {
                X = 0,
                Y = 0,
                Width = Pallet.ActualWidth - drag.Width,
                Height = Pallet.ActualHeight - drag.Height
            };

            Point TempPoint2 = PointIntersectWithBorder(RectOrg, Border);
            RectOrg.Y = TempPoint2.Y;
            RectOrg.X = TempPoint2.X;
            foreach (Rectangle r in Pallet.Children)
            {
                Rect RectComp = new Rect()
                {
                    X = Canvas.GetLeft(r),
                    Y = Canvas.GetTop(r),
                    Width = r.Width,
                    Height = r.Height
                };
                if (r != drag)
                {
                    if (!RectHelper.Intersect(RectOrg, RectComp).IsEmpty)
                    {
                        Point TempPoint = FindPlacementPointOnIntersect(RectOrg, RectComp, Border, Pallet.Children);
                        RectOrg.Y = TempPoint.Y;
                        RectOrg.X = TempPoint.X;

                    }
                }
            }

            Canvas.SetTop(drag, RectOrg.Y);
            Canvas.SetLeft(drag, RectOrg.X);

            Controller.Core.Layers[ComboBoxSelectedLayer.SelectedIndex][Pallet.Children.IndexOf(drag)] = new Item()
            {
                x = Canvas.GetLeft(drag) / ScreenFactor,
                y = Canvas.GetTop(drag) / ScreenFactor,
                widthInCm = Controller.Core.Layers[ComboBoxSelectedLayer.SelectedIndex][Pallet.Children.IndexOf(selected)].widthInCm,
                lengthInCm = Controller.Core.Layers[ComboBoxSelectedLayer.SelectedIndex][Pallet.Children.IndexOf(selected)].lengthInCm,
                Rotation = Controller.Core.Layers[ComboBoxSelectedLayer.SelectedIndex][Pallet.Children.IndexOf(drag)].Rotation
            };
        }

        private Point PointIntersectWithBorder(Rect RectOrg, Rect Border)
        {

            if (RectOrg.X < 0)
                RectOrg.X = 0;
            else if (RectOrg.X > Border.Width)
                RectOrg.X = Border.Width;

            if (RectOrg.Y < 0)
                RectOrg.Y = 0;
            else if (RectOrg.Y > Border.Height)
                RectOrg.Y = Border.Height;

            return new Point() { X = RectOrg.X, Y = RectOrg.Y };

        }
        private Boolean IntersectWithBorder(Rect RectOrg, Rect Border)
        {

            if (RectOrg.X < 0)
                return true;
            else if (RectOrg.X > Border.Width)
                return true;

            if (RectOrg.Y < 0)
                return true;
            else if (RectOrg.Y > Border.Height)
                return true;
            return false;

        }
        private Point FindPlacementPointOnIntersect(Rect RectOrg, Rect RectComp, Rect Border, UIElementCollection list)
        {
            Rect tempRect = RectHelper.Intersect(RectOrg, RectComp);
            if (tempRect.Height <= tempRect.Width)
            {
                if (RectOrg.Y >= RectComp.Y)
                {
                    RectOrg.Y = RectOrg.Y + Math.Ceiling(tempRect.Height);
                }
                else if (RectOrg.Y < RectComp.Y)
                {
                    RectOrg.Y = RectOrg.Y - Math.Ceiling(tempRect.Height);
                }
            }
            if (tempRect.Height > tempRect.Width)
            {
                if (RectOrg.X >= RectComp.X)
                {
                    RectOrg.X = RectOrg.X + Math.Ceiling(tempRect.Width);
                }
                else if (RectOrg.X < RectComp.X)
                {
                    RectOrg.X = RectOrg.X - Math.Ceiling(tempRect.Width);
                }
            }

            if (ItemIntersect(RectOrg, list) || IntersectWithBorder(RectOrg, Border))
            {
                RectOrg.X = Canvas.GetLeft(selected);
                RectOrg.Y = Canvas.GetTop(selected);
            }
            
            return new Point() { X = RectOrg.X, Y = RectOrg.Y };
        }

        private Boolean ItemIntersect(Rect RectOrg, UIElementCollection list)
        {
            foreach (Rectangle r in list)
            {
                Rect RectComp = new Rect()
                {
                    X = Canvas.GetLeft(r),
                    Y = Canvas.GetTop(r),
                    Width = r.Width,
                    Height = r.Height
                };
                if (r != selected)
                {
                    Rect tempRect = RectHelper.Intersect(RectOrg, RectComp);
                    if (!tempRect.IsEmpty)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public void rotateSelectedItemRight(object sender, RoutedEventArgs e)
        {
            Rect TempRect = new Rect()
            {
                X = Canvas.GetLeft(selected) + selected.Width / 2 - selected.Height / 2,
                Y = Canvas.GetTop(selected)+ selected.Height / 2 - selected.Width / 2,
                Width = selected.Height,
                Height = selected.Width
            };

            Rect Border = new Rect()
            {
                X = 0,
                Y = 0,
                Width = Pallet.ActualWidth - selected.Width,
                Height = Pallet.ActualHeight - selected.Height
            };

            Point TempPoint = PointIntersectWithBorder(TempRect, Border);
            TempRect.Y = TempPoint.Y;
            TempRect.X = TempPoint.X;

            if (ItemIntersect(TempRect, Pallet.Children))
            {
                return;
            }

            Canvas.SetLeft(selected, TempRect.X);
            Canvas.SetTop(selected, TempRect.Y);
            selected.Width = TempRect.Width;
            selected.Height = TempRect.Height;

            Controller.Core.Layers[ComboBoxSelectedLayer.SelectedIndex][Pallet.Children.IndexOf(selected)] = new Item()
            {
                x = Canvas.GetLeft(selected) / ScreenFactor,
                y = Canvas.GetTop(selected) / ScreenFactor,
                Rotation = Controller.Core.Layers[ComboBoxSelectedLayer.SelectedIndex][Pallet.Children.IndexOf(selected)].lengthInCm + 1,
                widthInCm = Controller.Core.Layers[ComboBoxSelectedLayer.SelectedIndex][Pallet.Children.IndexOf(selected)].lengthInCm,
                lengthInCm = Controller.Core.Layers[ComboBoxSelectedLayer.SelectedIndex][Pallet.Children.IndexOf(selected)].widthInCm
            };
            if (Controller.Core.Layers[ComboBoxSelectedLayer.SelectedIndex][Pallet.Children.IndexOf(selected)].Rotation == 4)
                Controller.Core.Layers[ComboBoxSelectedLayer.SelectedIndex][Pallet.Children.IndexOf(selected)].Rotation = 0;
        }

        public void rotateSelectedItemLeft(object sender, RoutedEventArgs e)
        {
            Rect TempRect = new Rect()
            {
                X = Canvas.GetLeft(selected) + selected.Width / 2 - selected.Height / 2,
                Y = Canvas.GetTop(selected) + selected.Height / 2 - selected.Width / 2,
                Width = selected.Height,
                Height = selected.Width
            };

            Rect Border = new Rect()
            {
                X = 0,
                Y = 0,
                Width = Pallet.ActualWidth - selected.Width,
                Height = Pallet.ActualHeight - selected.Height
            };

            Point TempPoint = PointIntersectWithBorder(TempRect, Border);
            TempRect.Y = TempPoint.Y;
            TempRect.X = TempPoint.X;

            if (ItemIntersect(TempRect, Pallet.Children))
            {
                return;
            }

            Canvas.SetLeft(selected, TempRect.X);
            Canvas.SetTop(selected, TempRect.Y);
            selected.Width = TempRect.Width;
            selected.Height = TempRect.Height;

            Controller.Core.Layers[ComboBoxSelectedLayer.SelectedIndex][Pallet.Children.IndexOf(selected)] = new Item()
            {
                x = Canvas.GetLeft(selected) / ScreenFactor,
                y = Canvas.GetTop(selected) / ScreenFactor,
                Rotation = Controller.Core.Layers[ComboBoxSelectedLayer.SelectedIndex][Pallet.Children.IndexOf(selected)].Rotation - 1,
                widthInCm = Controller.Core.Layers[ComboBoxSelectedLayer.SelectedIndex][Pallet.Children.IndexOf(selected)].lengthInCm,
                lengthInCm = Controller.Core.Layers[ComboBoxSelectedLayer.SelectedIndex][Pallet.Children.IndexOf(selected)].widthInCm
            };
            if (Controller.Core.Layers[ComboBoxSelectedLayer.SelectedIndex][Pallet.Children.IndexOf(selected)].Rotation == -1)
                Controller.Core.Layers[ComboBoxSelectedLayer.SelectedIndex][Pallet.Children.IndexOf(selected)].Rotation = 3;
            switch (Controller.Core.Layers[ComboBoxSelectedLayer.SelectedIndex][Pallet.Children.IndexOf(selected)].Rotation)
            {
                case 0:
                    ((LinearGradientBrush)(selected.Fill)).StartPoint = new Point(0, 0);
                    ((LinearGradientBrush)(selected.Fill)).EndPoint = new Point(0, 1);
                    break;
                case 1:
                    ((LinearGradientBrush)(selected.Fill)).StartPoint = new Point(1, 0);
                    ((LinearGradientBrush)(selected.Fill)).EndPoint = new Point(0, 0);
                    break;
                case 2:
                    ((LinearGradientBrush)(selected.Fill)).StartPoint = new Point(0, 1);
                    ((LinearGradientBrush)(selected.Fill)).EndPoint = new Point(0, 0);
                    break;
                case 3:
                    ((LinearGradientBrush)(selected.Fill)).StartPoint = new Point(0, 0);
                    ((LinearGradientBrush)(selected.Fill)).EndPoint = new Point(1, 0);
                    break;
            }
            
        }

        private void CompressPallet()
        {

        }


        private void Rectangle_PointerCanceled(object sender, PointerRoutedEventArgs e)
        {
            drag.PointerReleased -= Rectangle_PointerCanceled;
            drag.PointerExited -= Rectangle_PointerCanceled;
            drag.PointerMoved -= MoveItem;
            drag = null;
        }

        private void BtnAddLayer_Click(object sender, RoutedEventArgs e)
        {
            Controller.AddLayer();
        }

        private void BtnAddItem_Click(object sender, RoutedEventArgs e)
        {
            AddItemToPallet(Controller.AddItem(ComboBoxSelectedLayer.SelectedIndex, 40, 40));
        }
        private void BtnDeleteItem_Click(object sender, RoutedEventArgs e)
        {
            Controller.Core.Layers[ComboBoxSelectedLayer.SelectedIndex].RemoveAt(Pallet.Children.IndexOf(selected));
            Pallet.Children.Remove(selected);
            selected = null;
        }

        public void changeLayer(int layer)
        {
            Pallet.Children.Clear();
            foreach (Item Item in Controller.Core.Layers[layer])
            {
                AddItemToPallet(Item);
            }
        }

        private void ComboBoxSelectedLayer_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(ComboBoxSelectedLayer.SelectedIndex >= 0)
                changeLayer(ComboBoxSelectedLayer.SelectedIndex);
        }

        private void BtnAddItem_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
        }
    }
}
