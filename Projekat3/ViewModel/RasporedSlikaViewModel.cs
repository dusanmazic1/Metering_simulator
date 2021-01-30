using Projekat3.Model;
using Projekat3.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Projekat3.ViewModel
{
    public class RasporedSlikaViewModel : BindableBase
    {

        public MyICommand<ListView> LeftMouseButtonDownCommand { get; set; }

        public MyICommand<Canvas> DragOver { get; set; }
        public MyICommand<Canvas> Drop { get; set; }

        public MyICommand<Canvas> Oslobodi { get; set; }
        public static ObservableCollection<Merac> Brojila { get; set; } = new ObservableCollection<Merac>();
        private Merac izabrani_za_prevlacenje;

        public Merac Izabrani_Za_Prevlacenje
        {
            get { return izabrani_za_prevlacenje; }
            set { izabrani_za_prevlacenje = value; }
        }

        public RasporedSlikaViewModel()
        {
            Brojila = MainWindowViewModel.Meraci;
            LeftMouseButtonDownCommand = new MyICommand<ListView>(OnDrop);
            DragOver = new MyICommand<Canvas>(OnDrag);
            Drop = new MyICommand<Canvas>(OnDroping);
            Oslobodi = new MyICommand<Canvas>(OnFree);
        }

        private void OnFree(Canvas b)
        {

            if (b.Resources["taken"] != null)
            {
                b.Background = Brushes.GhostWhite;
                ((TextBlock)b.Children[0]).Foreground = Brushes.Black;
                ((TextBlock)b.Children[0]).Text = "Slobodno mesto";
                b.Resources.Remove("taken");
            }

        }
        private void OnDrag(Canvas c)
        {
            if(c.Resources["taken"] != null)
            {
                c.AllowDrop = false;
            }
            else
            {
                c.AllowDrop = true;
            }
        }

        private void OnDroping(Canvas c)
        {
            if (c.Resources["taken"] == null)
            {
                BitmapImage logo = new BitmapImage();
                logo.BeginInit();
                logo.UriSource = new Uri(Izabrani_Za_Prevlacenje.Tip.ImgSrc);
                logo.EndInit();
                c.Background = new ImageBrush(logo);
                ((TextBlock)(c).Children[0]).Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFDAFF00"));
                ((TextBlock)(c).Children[0]).Text = "ID:"+izabrani_za_prevlacenje.ID+ "\nName:" + izabrani_za_prevlacenje.Name;
                c.Resources.Add("taken", true);
            }

            Izabrani_Za_Prevlacenje = null;

        }

        private void OnDrop(ListView lv)
        {
            DragDrop.DoDragDrop(lv, Izabrani_Za_Prevlacenje, DragDropEffects.Copy | DragDropEffects.Move);
        }


    }
}
