namespace WebApiMensajeria.Entity
{
    public class Result<Tipo>
    {
        public Tipo value { get; set; }
        public string errorCodigo { get; set; } = "OK";
        public string errorMensaje { get; set; } = "";
    }
}
