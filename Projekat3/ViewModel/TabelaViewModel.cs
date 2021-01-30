using Projekat3.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Projekat3.Views;
using System.Windows.Media.Imaging;
using System.Windows;

namespace Projekat3.ViewModel
{
    public class TabelaViewModel:BindableBase
    {


        public static ObservableCollection<Merac> Lokalni_Meraci { get; set; } = new ObservableCollection<Merac>();
        private ObservableCollection<Merac> FilterMeraca = new ObservableCollection<Merac>();
        //public static ObservableCollection<Merac> Meraci { get; set; } = new ObservableCollection<Merac>();

        public MyICommand DeleteCommand { get; set; }
        public MyICommand AddCommand { get; set; }
        public MyICommand FilterCommand { get; set; }
        public MyICommand CancelFilterCommand { get; set; }
        public MyICommand OutOfRangeCommand { get; set; }
        public MyICommand ExpectedValuesCommand { get; set; }

        public Dictionary<string,string> Imena_Tipova { get; set; }

        private int inOrOutValues = -1;
        private bool filtercan = false;
        private Merac selektovani_merac;
        public Merac Selektovani_Merac
        {
            get { return selektovani_merac; }
            set
            {
                selektovani_merac = value;
                DeleteCommand.RaiseCanExecuteChanged();
            }
        }
       

        public int InOrOutValues
        {
            get { return inOrOutValues; }
            set { inOrOutValues = value; }
        }

        private string TBid;
        public string TBID
        {
            get { return TBid; }
            set
            {
                if(TBid != value) { TBid = value; OnPropertyChanged("TBID"); }
                
            }
        }

        private string TBname;
        public string TBNAME
        {
            get { return TBname; }
            set
            {
                if(TBname != value) { TBname = value; OnPropertyChanged("TBNAME"); }
                
            }
        }

      
        private int tbFILTERid;
        public int TBFILTERID
        {
            get { return tbFILTERid; }
            set { tbFILTERid = value; }
        }

        private string izabrani_tip;
        public string Izabrani_Tip
        {
            get { return izabrani_tip; }
            set {
                izabrani_tip = value;
                Izabrani_Tip_Slika = new BitmapImage(new Uri(pronadji_sliku_za_tip(izabrani_tip)));
                OnPropertyChanged("Izabrani_Tip_Slika");
                OnPropertyChanged("Izabrani_Tip");
                FilterCommand.RaiseCanExecuteChanged();
            }
        }

        private BitmapImage izabrani_tip_slika;
        public BitmapImage Izabrani_Tip_Slika
        {
            get { return izabrani_tip_slika; }
            set { izabrani_tip_slika = value; }
        }
        

        public TabelaViewModel()
        {

            Lokalni_Meraci = MainWindowViewModel.Meraci;

            Imena_Tipova = new Dictionary<string, string>();
            dodaj_imena_tipova();

            AddCommand = new MyICommand(OnAdd);
            DeleteCommand = new MyICommand(OnDelete, CanDelete);
            FilterCommand = new MyICommand(OnFilter, CanFilter);
            OutOfRangeCommand = new MyICommand(OnOut);
            ExpectedValuesCommand = new MyICommand(OnExpected);
            CancelFilterCommand = new MyICommand(OnCancel, CanCancel);

        }

        private void OnAdd() {
             Merac temp = new Merac();
            Model.Tip tip = new Model.Tip();
            
            temp.Tip = tip;
        
            try
            {
                temp.ID = Int32.Parse(TBID);
            }
            catch
            {
                System.Windows.MessageBox.Show("ID mora biti broj!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            temp.Name = TBNAME;
            if (string.IsNullOrWhiteSpace(temp.Name))
            {
                System.Windows.MessageBox.Show("Mora biti uneseno nesto u naziv!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            bool exists = false;
            foreach (Merac v in Lokalni_Meraci)
            {
                if (v.ID == temp.ID)
                {
                    exists = true;
                }
            }
            if (exists)
            {
                System.Windows.MessageBox.Show("ID mora da bude jedinstven!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            temp.Vrednost = 0;
            temp.Tip.Naziv = Izabrani_Tip;
            temp.Tip.ImgSrc = pronadji_sliku_za_tip(Izabrani_Tip);
            
            Lokalni_Meraci.Add(temp);

            MainWindowViewModel.Meraci = Lokalni_Meraci;
        }
       

        private string pronadji_sliku_za_tip(string tip)
        {
            string temp = "";
            foreach (string s in Imena_Tipova.Keys)
            {
                if (s.Equals(tip)) { temp = Imena_Tipova[s]; break; }
            }

            return temp;
        }

        private void OnDelete() 
        { 
            Lokalni_Meraci.Remove(Selektovani_Merac);
            MainWindowViewModel.Meraci = Lokalni_Meraci; 
        }

        private bool CanDelete() 
        { 
            return Selektovani_Merac != null;
        }


        private string izabrani_tip_filter;

        public string Izabrani_Tip_Filter
        {
            get { return izabrani_tip_filter; }
            set { izabrani_tip_filter = value; }
        }

        private bool isCheckedGreater;

        public bool ISCHECKEDGREATER
        {
            get { return isCheckedGreater; }
            set 
            {
                isCheckedGreater = value; 
                FilterCommand.RaiseCanExecuteChanged();
            }
        }

        private bool isCheckedLower;

        public bool ISCHECKEDLOWER
        {
            get { return isCheckedLower; }
            set { isCheckedLower = value; FilterCommand.RaiseCanExecuteChanged(); }
        }


        private bool CanFilter()
        {
            bool filter = false;
            if (((isCheckedGreater != false) || (isCheckedLower != false) || (izabrani_tip_filter != string.Empty) || (inOrOutValues != -1)))
                filter = true;
            else
                filter = false;

            return filter;
        }


        private void OnFilter()
        {

            int val;
            //FilterMeraca.Clear();
            if(TBID == "")
            {
                val = -1;
                System.Windows.MessageBox.Show("Unesite ID!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                try
                {
                    val = Int32.Parse(TBID);
                }
                catch
                {
                    System.Windows.MessageBox.Show("ID mora biti broj!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }

            //Lokalni_Meraci.Clear();
            foreach(Merac m in Lokalni_Meraci)
            {
                FilterMeraca.Add(m);//kopija
            }
            //Lokalni_Meraci.Clear();
            if (isCheckedGreater != false || isCheckedLower != false)
            {
                foreach(Merac m in FilterMeraca)
                {
                    if(isCheckedLower == true)
                    {
                        if(val >= m.ID)
                        {
                            Lokalni_Meraci.Remove(m);
                        }
                        
                    }
                    else if(isCheckedGreater == true)
                    {
                        if (val <= m.ID)
                        {
                            Lokalni_Meraci.Remove(m);
                        }
                    }
                } 
               //FilterInMeraci();
            }
           
            if(Izabrani_Tip_Filter != string.Empty)
            {
                foreach(Merac m in FilterMeraca)
                {
                    if (Izabrani_Tip_Filter.Equals(m.Tip.Naziv))
                    {
                        Lokalni_Meraci.Remove(m);
                    }
                }
              // FilterInMeraci();
            }
            TBID = "";
            isCheckedGreater = false;
            isCheckedLower = false;
            inOrOutValues = -1;
            Izabrani_Tip_Filter = string.Empty;
            OnPropertyChanged("Izabrani_Tip_Filter");
            filtercan = true;
            OnPropertyChanged("Lokalni_Meraci");
            CancelFilterCommand.RaiseCanExecuteChanged();
        }
        /*private void FilterInMeraci()
        {
            Meraci.Clear();
             foreach (Merac m in FilterMeraca)
             {
                Meraci.Add(m);
             }
           // Lokalni_Meraci.Clear();
        }*/

        private bool CanCancel()
        {
            return filtercan;

        }

        private void OnCancel()
        {
            Lokalni_Meraci.Clear();
            foreach (Merac m in FilterMeraca)
            {
                Lokalni_Meraci.Add(m);
            }
            //OnPropertyChanged("Lokalni_Meraci");
            FilterMeraca.Clear();
            filtercan = false;
            CancelFilterCommand.RaiseCanExecuteChanged();

        }



        private void OnOut()
        {
            InOrOutValues = 1;
            FilterCommand.RaiseCanExecuteChanged();
        }
        private void OnExpected()
        {
            InOrOutValues = 2;
            FilterCommand.RaiseCanExecuteChanged();
        }

        private void dodaj_imena_tipova()
        {
            
            Imena_Tipova.Add("Tecni", "D:\\Documents\\Desktop\\Fax\\3 Godina\\HCI\\Projekat3\\Projekat3\\bin\\Debug\\Nuklearni_reaktor_6.jpg");
            Imena_Tipova.Add("Plinski", "D:\\Documents\\Desktop\\Fax\\3 Godina\\HCI\\Projekat3\\Projekat3\\bin\\Debug\\Reaktor_12.jpg");
            Imena_Tipova.Add("Kljucajuci", "D:\\Documents\\Desktop\\Fax\\3 Godina\\HCI\\Projekat3\\Projekat3\\bin\\Debug\\Nuklearni_reaktor_5.jpg");
        }

    }
}
