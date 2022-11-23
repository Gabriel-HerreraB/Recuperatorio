using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EquipoQ22.Domino
{
    public class Equipo
    {
        public Equipo()
        {
            Pais = "";
            DirectorTecnico = "";
            Jugadores = new List<Jugador>();
        }
        public string Pais { get; set; }
        public string DirectorTecnico { get; set; }
        public List<Jugador>  Jugadores{ get; set; }
        public void AgregarJugador(Jugador j) {
            Jugadores.Add(j);
        }
        public void QuitarJugador(int indice) {
            Jugadores.RemoveAt(indice);
        }
    }
}
