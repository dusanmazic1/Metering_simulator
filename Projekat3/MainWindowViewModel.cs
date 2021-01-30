using Projekat3.Model;
using Projekat3.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Projekat3
{
    public class MainWindowViewModel : BindableBase
    {
        public MyICommand<string> NavCommand { get; private set; }
        private TabelaViewModel tabelaviewmodel = new TabelaViewModel();
        private RasporedSlikaViewModel slikeviewmodel = new RasporedSlikaViewModel();
        private HelpViewModel helpviewmodel = new HelpViewModel();
        private BindableBase currentViewModel;

        public static ObservableCollection<Merac> Meraci { get; set; } = new ObservableCollection<Merac>();


        public MainWindowViewModel()
        {
            NavCommand = new MyICommand<string>(OnNav);
            CurrentViewModel = tabelaviewmodel;
            createListener();
           // createListener(); //Povezivanje sa serverskom aplikacijom

        }
        public BindableBase CurrentViewModel
        {
            get { return currentViewModel; }
            set { SetProperty(ref currentViewModel, value); }
        }

        private void OnNav(string destination)
        {
            switch (destination)
            {
                case "tabela":
                    CurrentViewModel = tabelaviewmodel;
                    break;
                case "slike":
                    CurrentViewModel = slikeviewmodel;
                    break;
                case "help":
                    CurrentViewModel = helpviewmodel;
                    break;
            }
        }


        private void createListener()
        {
            var tcp = new TcpListener(IPAddress.Any, 25565);
            tcp.Start();

            var listeningThread = new Thread(() =>
            {
                while (true)
                {
                    var tcpClient = tcp.AcceptTcpClient();
                    ThreadPool.QueueUserWorkItem(param =>
                    {
                        //Prijem poruke
                        NetworkStream stream = tcpClient.GetStream();
                        string incomming;
                        byte[] bytes = new byte[1024];
                        int i = stream.Read(bytes, 0, bytes.Length);
                        //Primljena poruka je sacuvana u incomming stringu
                        incomming = System.Text.Encoding.ASCII.GetString(bytes, 0, i);

                        //Ukoliko je primljena poruka pitanje koliko objekata ima u sistemu -> odgovor
                        if (incomming.Equals("Need object count"))
                        {
                            //Response
                            /* Umesto sto se ovde salje count.ToString(), potrebno je poslati 
                             * duzinu liste koja sadrzi sve objekte pod monitoringom, odnosno
                             * njihov ukupan broj (NE BROJATI OD NULE, VEC POSLATI UKUPAN BROJ)
                             * */
                            Byte[] data = System.Text.Encoding.ASCII.GetBytes(Meraci.Count.ToString());
                            stream.Write(data, 0, data.Length);
                        }
                        else
                        {
                            //U suprotnom, server je poslao promenu stanja nekog objekta u sistemu
                            Console.WriteLine(incomming); //Na primer: "Objekat_1:272"

                            

                            int index=Int32.Parse(incomming.Split(':', '_')[1]);

                            double value = Double.Parse(incomming.Split(':', '_')[2]);

                            Meraci[index].Vrednost = value;
                            TabelaViewModel.Lokalni_Meraci[index].Vrednost = value;
                            RasporedSlikaViewModel.Brojila[index].Vrednost = value;

                        }
                    }, null);
                }
            });

            listeningThread.IsBackground = true;
            listeningThread.Start();
        }

    }
}
