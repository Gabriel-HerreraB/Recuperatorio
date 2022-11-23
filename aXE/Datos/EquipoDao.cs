using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EquipoQ22.Domino;

namespace EquipoQ22.Datos
{
    class EquipoDao : IEquipoDao
    {
        public bool CrearEquipo(Equipo equipo)
        {
            return HelperDB.ObtenerInstancia().InsertarMaestroDetalle(equipo,"SP_INSERTAR_EQUIPO","@id","SP_INSERTAR_DETALLES_EQUIPO");
        }

        public List<Persona> ObtenerPersonas()
        {
            DataTable dt = HelperDB.ObtenerInstancia().ConsultarSp("SP_CONSULTAR_PERSONAS",null);
            List<Persona> personas = new List<Persona>();
            foreach (DataRow row in dt.Rows) {
                Persona p = new Persona();
                p.IdPersona = Convert.ToInt32(row["id_persona"]);
                p.NombreCompleto = row["nombre_completo"].ToString();
                p.Clase = Convert.ToInt32(row["clase"]);
                personas.Add(p);
            }
            return personas;
        }
    }
}
