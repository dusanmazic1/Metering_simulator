using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekat3.Model
{
    public class Tip:BindableBase
    {
        private string naziv;

        public string Naziv
        {
            get { return naziv; }
            set { naziv = value;  OnPropertyChanged("Naziv"); }
        }

        private string imgSrc;

        public string ImgSrc
        {
            get { return imgSrc; }
            set { imgSrc = value; OnPropertyChanged("ImgSrc"); }
        }

        public Tip() { }

        public Tip(string uri)
        {
            ImgSrc = uri;
        }

        public Tip(string a, string b)
        {
            naziv = a;
            imgSrc = b;
        }

    }
}
