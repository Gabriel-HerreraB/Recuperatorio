using EquipoQ22.Domino;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EquipoQ22.Datos
{
    class HelperDB
    {
        private static HelperDB instancia;
        private SqlConnection cnn;
        private HelperDB()
        {
            cnn = new SqlConnection(Properties.Resources.CadenaConexion);
        }
        public static HelperDB ObtenerInstancia()
        {
            if (instancia == null) {
                instancia = new HelperDB();
            }
            return instancia;
        }
        public DataTable ConsultarSp(string sp, List<Parametro> param){
            DataTable dt = new DataTable();
            try
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand(sp, cnn);
                cmd.CommandType = CommandType.StoredProcedure;
                if (param != null)
                {
                    foreach (Parametro p in param)
                    {
                        cmd.Parameters.AddWithValue(p.Nombre, p.Valor);
                    }

                }
                dt.Load(cmd.ExecuteReader());
            }
            catch (Exception)
            {

                dt = null;
            }
            finally {
                if (cnn != null && cnn.State == ConnectionState.Open) {
                    cnn.Close();
                }
            }
            return dt;
        }
        public bool InsertarMaestroDetalle(Equipo equipo,string spMaestro,string variableOutput,string spDetalle) {
            bool aux = true;
            SqlTransaction t = null;
            try
            {
                cnn.Open();
                t = cnn.BeginTransaction();
                SqlCommand cmdMaestro = new SqlCommand(spMaestro, cnn, t);
                cmdMaestro.CommandType = CommandType.StoredProcedure;
                cmdMaestro.Parameters.AddWithValue("@pais", equipo.Pais);
                cmdMaestro.Parameters.AddWithValue("@director_tecnico", equipo.DirectorTecnico);
                SqlParameter param = new SqlParameter(variableOutput, SqlDbType.Int);
                param.Direction = ParameterDirection.Output;
                cmdMaestro.Parameters.Add(param);
                cmdMaestro.ExecuteNonQuery();
                int id = Convert.ToInt32(param.Value);
                foreach (Jugador j in equipo.Jugadores)
                {
                    SqlCommand cmdDetalle = new SqlCommand(spDetalle, cnn, t);
                    cmdDetalle.CommandType = CommandType.StoredProcedure;
                    cmdDetalle.Parameters.AddWithValue("@id_equipo", id);
                    cmdDetalle.Parameters.AddWithValue("@id_persona", j.Persona.IdPersona);
                    cmdDetalle.Parameters.AddWithValue("@camiseta", j.Camiseta);
                    cmdDetalle.Parameters.AddWithValue("@posicion", j.Posicion);
                    cmdDetalle.ExecuteNonQuery();
                }
                t.Commit();
            }
            catch (Exception)
            {
                aux = false;
                t.Rollback();
            }
            finally {
                if (cnn != null && cnn.State == ConnectionState.Open) {
                    cnn.Close();
                }
            }
            return aux;
        }
    }
}
