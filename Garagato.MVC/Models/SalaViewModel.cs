namespace Garagato.MVC.Models;

public class SalaViewModel
{
    public List<DataJugador> InformacionSala {  get; set; }
    public string nombreSala { get; set; }
    public int idSala { get; set; }
    public List<string> dibujosPrevios { get; set; }
    public int idUsuarioLogueado { get; set; }
}
