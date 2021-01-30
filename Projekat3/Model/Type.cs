using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekat3.Model
{
    public class Type
    {
        private string naziv;

        public string Naziv
        {
            get { return naziv; }
            set { naziv = value; }
        }

        private string imgSrc;

        public string ImgSrc
        {
            get { return imgSrc; }
            set { imgSrc = value; }
        }

        public Type() { }

        public Type(string a, string b)
        {
            naziv = a;
            imgSrc = b;
        }

    }
}
