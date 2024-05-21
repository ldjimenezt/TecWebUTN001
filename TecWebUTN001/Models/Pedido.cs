namespace TecWebUTN001.Models
{
    public class Pedido
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public double Cantidad { get; set; }

        public int ProductoID { get; set; } // clave foranea (producto)
        public int ClienteID { get; set; } // clave forane (cliente)

        public Producto? Producto { get; set; }
        public Cliente? Cliente { get; set; }

    }
}
