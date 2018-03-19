using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace OrdVenta01
{
    public class OrdenVentaItem : INotifyPropertyChanged
    {
        //private int id;
        //private DateTime fechaEmision;
        //private EstadoOrden estadoOrden;
        //private int recibida;
        //private DateTime fechaRecepcion;
        //private int lista;
        //private DateTime fechaLista;
        //private int entregada;
        //private DateTime fehaEntrega;
        //private String observacion;
        //private TipoDespacho tipoDespacho;
        //private String usuario;
        private int nvNumero;
        private DateTime dateCreacion;
        private DateTime dateRecepcion;
        private DateTime dateLista;
        private DateTime dateEntrega;
        private String estado1;
        private int estado2;
        private String estado3;
        private String estado4;
        private String observ1;
        private String observ2;
        private String observ3;
        private String observ4;
        private DateTime dateAux;
        private String codCliente;



        public event PropertyChangedEventHandler PropertyChanged;

        #region Properties Setters y Getters
        public int NvNumero
        {
            get { return this.nvNumero; }
            set
            {
                this.nvNumero = value;
                OnPropertyChanged("NvNumero");
            }

        }

        public DateTime DateCreacion
        {
            get { return this.dateCreacion; }
            set
            {
                this.dateCreacion = value;
                OnPropertyChanged("DateCreacion");
            }
        }

        public DateTime DateLista
        {
            get { return this.dateLista; }
            set
            {
                this.dateLista = value;
                OnPropertyChanged("DateLista");
            }
        }

        public DateTime DateRecepcion
        {
            get { return this.dateRecepcion; }
            set
            {
                this.dateRecepcion = value;
                OnPropertyChanged("DateRecepcion");
            }
        }

        public DateTime DateEntrega
        {
            get { return this.dateEntrega; }
            set
            {
                this.dateEntrega = value;
                OnPropertyChanged("DateEntrega");
            }
        }

        public String Estado1
        {
            get { return this.estado1; }
            set
            {
                this.estado1 = value;
                OnPropertyChanged("Estado1");
            }
        }

        public int Estado2
        {
            get { return this.estado2; }
            set
            {
                this.estado2 = value;
                OnPropertyChanged("Estado2");
            }
        }

        public String Estado3
        {
            get { return this.estado3; }
            set
            {
                this.estado3 = value;
                OnPropertyChanged("Estado3");
            }
        }

        public String Estado4
        {
            get { return this.estado4; }
            set
            {
                this.estado4 = value;
                OnPropertyChanged("Estado4");
            }
        }

        public String Observ1
        {
            get { return this.observ1; }
            set
            {
                this.observ1 = value;
                OnPropertyChanged("Observ1");
            }
        }

        public String Observ2
        {
            get { return this.observ2; }
            set
            {
                this.observ2 = value;
                OnPropertyChanged("Observ2");
            }
        }

        public String Observ3
        {
            get { return this.observ3; }
            set
            {
                this.observ3 = value;
                OnPropertyChanged("Observ3");
            }
        }

        public String Observ4
        {
            get { return this.observ4; }
            set
            {
                this.observ4 = value;
                OnPropertyChanged("Observ4");
            }
        }

        public DateTime DateAux
        {
            get { return this.dateAux; }
            set
            {
                this.dateAux = value;
                OnPropertyChanged("DateAux");
            }
        }

        public String CodCliente
        {
            get { return this.codCliente; }
            set
            {
                this.codCliente = value;
                OnPropertyChanged("CodCliente");
            }
        }

        #endregion

        //public OrdenVentaItem(int nvNumero, DateTime dateCreacion, DateTime dateRecepcion, DateTime dateLista, DateTime dateEntrega, String estado1, String estado2, String estado3, String estado4, String observ1, String observ2, String observ3, String observ4, DateTime dateAux, String codCliente)
        //{
        //    this.nvNumero = nvNumero;
        //    this.dateCreacion = dateCreacion;
        //    this.dateRecepcion = dateRecepcion;
        //    this.dateLista = dateLista;
        //    this.dateEntrega = dateEntrega;
        //    this.estado1 = estado1;
        //    this.estado2 = estado2;
        //    this.estado3 = estado3;
        //    this.estado4 = estado4;
        //    this.observ1 = observ1;
        //    this.observ2 = observ2;
        //    this.observ3 = observ3;
        //    this.observ4 = observ4;
        //    this.dateAux = dateAux;
        //    this.codCliente = codCliente;
            
        //     //private DateTime fechaRecepcion;
        //    //private int lista;
        //    //private DateTime fechaLista;
        //    //private int entregada;
        //    //private DateTime fehaEntrega;
            
        //}
        protected void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }
    }
    
   
}
    

