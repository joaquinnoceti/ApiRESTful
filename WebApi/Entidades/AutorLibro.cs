namespace WebApi.Entidades
{
    public class AutorLibro
    {
        public int LibroID { get; set; }
        public int AutorID { get; set; }
        public int Orden { get; set; }
        public Libro Libro{ get; set; }
        public Autor Autor { get; set; }

    }
}
