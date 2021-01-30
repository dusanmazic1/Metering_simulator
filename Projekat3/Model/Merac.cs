using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekat3.Model
{
    public class Merac:BindableBase
    {

        private int id;

        public int ID
        {
            get { return id; }
            set { id = value; OnPropertyChanged("ID");  }
        }

        private string name;

        public string Name
        {
            get { return name; }
            set { name = value; OnPropertyChanged("Name"); }
        }

       
        private Tip tip;

        public Tip Tip
        {
            get { return tip; }
            set { tip = value; OnPropertyChanged("Tip"); }
        }

        private double vrednost;

        public double Vrednost
        {
            get { return vrednost; }
            set { vrednost = value; OnPropertyChanged("Vrednost");  }
        }

        public Merac()
        {
           
        }

        public Merac (int a, string b , Tip c ,double e)
        {
            name = b;
            id = a;
            Tip.Naziv = c.Naziv;
            Tip.ImgSrc = c.ImgSrc;
            vrednost = e;
        }


        public override string ToString()
        {
            return ID+" " + Name;
        }
    }
}
