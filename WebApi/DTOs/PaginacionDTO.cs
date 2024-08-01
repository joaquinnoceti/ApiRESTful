namespace WebApi.DTOs
{
    public class PaginacionDTO
    {
        public int Pagina { get; set; } = 1;
        private int cantidadxPag = 10;
        private readonly int MaxReg  = 20;

        public int CantidadxPag
        {
            get { return cantidadxPag; }
            set { cantidadxPag = ( value > MaxReg) ? MaxReg : value; }
        }
    }
}
