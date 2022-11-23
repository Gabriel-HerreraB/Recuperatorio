using EquipoQ22.Domino;
using EquipoQ22.Negocios;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

//COMPLETAR --> Curso:1W3      Legajo:114255         Apellido y Nombre:Bordon Alvarez Axel

//CadenaDeConexion: "Data Source=sqlgabineteinformatico.frc.utn.edu.ar;Initial Catalog=Qatar2022;User ID=alumnolab22;Password=SQL-Alu22"

namespace EquipoQ22
{
    public partial class FrmAlta : Form
    {
        private GestorEquipos gestor;
        private Equipo equipo;
        public FrmAlta()
        {
            InitializeComponent();
            gestor = new GestorEquipos();
            equipo = new Equipo();
        }
        private void FrmAlta_Load(object sender, EventArgs e)
        {
            CargarPersonas();
            cboPosicion.Text = "Arquero";
        }
        #region METODOS PRIVADOS
        private void CargarPersonas() {
            List<Persona> list = gestor.ObtenerPersonas();
            cboPersona.DataSource = list;
            cboPersona.DisplayMember = "NombreCompleto";
            cboPersona.ValueMember = "IdPersona";
        }
        private void CalcularTotalJugadores() {
            lblTotal.Text = "Total de Jugadores: " + dgvDetalles.Rows.Count;
        }
        private void GuardarEquipo() {
            equipo.Pais = txtPais.Text;
            equipo.DirectorTecnico = txtDT.Text;
            if (gestor.ConfirmarEquipo(equipo))
            {
                MessageBox.Show("Ha registrado el equipo con exito", "Registro", MessageBoxButtons.OK);
                //new FrmAlta().ShowDialog();
                //this.Dispose();
                LimpiarCampos();
            }
            else {
                MessageBox.Show("Error al registrar equipo.Intente de nuevo", "Registro", MessageBoxButtons.OK);

            }
        }
        private void LimpiarCampos()
        {
            txtDT.Text = "";
            txtPais.Text = "";
            cboPersona.SelectedValue = 1;
            cboPosicion.Text = "Arquero";
            nudCamiseta.Value = 1;
            dgvDetalles.Rows.Clear();
        }
        #endregion
        private void btnAgregar_Click(object sender, EventArgs e)
        {
            if (cboPersona.Text.Equals(String.Empty)) {
                MessageBox.Show("Debe seleccionar una persona","Advertencia",MessageBoxButtons.OK);
                return;
            }
            if (nudCamiseta.Value < 1 || nudCamiseta.Value > 23)
            {
                MessageBox.Show("Debe ingresar un numero de camiseta entre 1 y 23", "Advertencia", MessageBoxButtons.OK);
                return;
            }
           
            foreach (DataGridViewRow row in dgvDetalles.Rows) {
                if (row.Cells["jugador"].Value.ToString().Equals(cboPersona.Text)) {
                MessageBox.Show("Esa persona ya fue ingresada", "Advertencia", MessageBoxButtons.OK);
                    return;
                }
            }
            foreach (DataGridViewRow row in dgvDetalles.Rows)
            {
                if (row.Cells["camiseta"].Value.ToString().Equals(nudCamiseta.Value.ToString()))
                {
                    MessageBox.Show("Esa camiseta ya fue ingresada.Pruebe con otra", "Advertencia", MessageBoxButtons.OK);
                    return;
                }
            }
            
            if (cboPosicion.Text.Equals(String.Empty))
            {

                MessageBox.Show("Debe seleccionar una posicion", "Advertencia", MessageBoxButtons.OK);
                return;
            }
            Persona p = (Persona)cboPersona.SelectedItem;

            Jugador j = new Jugador();
            j.Persona = p;
            j.Camiseta = Convert.ToInt32(nudCamiseta.Value);
            j.Posicion = cboPosicion.Text;

            equipo.AgregarJugador(j);
            dgvDetalles.Rows.Add(new object[] { j.Persona.IdPersona, j.Persona.NombreCompleto,j.Camiseta,j.Posicion});
            CalcularTotalJugadores();

        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            if (txtPais.Text.Equals(String.Empty)) {
                MessageBox.Show("Debe ingresar un pais", "Advertencia", MessageBoxButtons.OK);
                return;
            }
            if (txtDT.Text.Equals(String.Empty)) {
                MessageBox.Show("Debe ingresar un direcctor tecnico", "Advertencia", MessageBoxButtons.OK);
                return;
            }
            if (dgvDetalles.Rows.Count < 1) {
                MessageBox.Show("Debe ingresar al menos un jugador", "Advertencia", MessageBoxButtons.OK);
                return;
            }
            GuardarEquipo();
        }

        

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("¿Está seguro que desea cancelar?", "Salir", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                this.Dispose();
            }
        }

        private void dgvDetalles_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvDetalles.CurrentCell.ColumnIndex == 4) {
                DialogResult msg = MessageBox.Show("Desea eliminar este jugador","Registro",MessageBoxButtons.YesNo);
                if (msg == DialogResult.Yes) {
                    equipo.QuitarJugador(dgvDetalles.CurrentRow.Index);
                    dgvDetalles.Rows.Remove(dgvDetalles.CurrentRow);
                }
            }
        }
    }
}
