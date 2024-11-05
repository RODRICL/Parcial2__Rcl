using CadParcial2Rcl;
using ClnParcial2Rcl;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CpParcial2Rcl
{
    public partial class FrmSerie : Form
    {
        private bool esNuevo = false;

        public FrmSerie()
        {
            InitializeComponent();
        }

        private void Listar()
        {
            var lista = SerieCln.listarPa(txtParametro.Text);
            var filteredData = lista.Select(s => new
            {
                s.id,
                s.titulo,
                s.sinopsis,
                s.director,
                s.tipoClasificacion,
                s.episodios,
                s.fechaEstreno
            }).ToList();
            dgvLista.DataSource = filteredData;
            dgvLista.Columns["id"].Visible = false;
            dgvLista.Columns["titulo"].HeaderText = "Título";
            dgvLista.Columns["sinopsis"].HeaderText = "Sinopsis";
            dgvLista.Columns["director"].HeaderText = "Director";
            dgvLista.Columns["episodios"].HeaderText = "Episodios";
            dgvLista.Columns["fechaEstreno"].HeaderText = "Fecha Estreno";
            btnEditar.Enabled = lista.Count() > 0;
            btnEliminar.Enabled = lista.Count() > 0;
            if (lista.Count > 0) dgvLista.CurrentCell = dgvLista.Rows[0].Cells["titulo"];
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            Listar();
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            esNuevo = true;
            Size = new Size(860, 504);
            txtTitulo.Focus();
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            esNuevo = false;
            Size = new Size(860, 504);

            int index = dgvLista.CurrentCell.RowIndex;
            int id = Convert.ToInt32(dgvLista.Rows[index].Cells["id"].Value);
            var serie = SerieCln.obtenerUno(id);
            txtTitulo.Text = serie.titulo;
            txtSinopsis.Text = serie.sinopsis;
            txtDirector.Text = serie.director;
            cbxtipoClasificacion.Text = serie.tipoClasificacion;
            nudEpisodio.Value = Math.Min(serie.episodios, nudEpisodio.Maximum);
            dtpFechaEstreno.Value = serie.fechaEstreno;
            txtTitulo.Focus();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Size = new Size(860, 348);
            LimpiarCampos();
        }

        private bool validar()
        {
            bool esValido = true;
            erpTitulo.SetError(txtTitulo, "");
            erpSinopsis.SetError(txtSinopsis, "");
            erpDirector.SetError(txtDirector, "");
            erpEpisodio.SetError(cbxtipoClasificacion, "");
            erpEpisodio.SetError(nudEpisodio, "");
            erpFechaEstreno.SetError(dtpFechaEstreno, "");

            if (string.IsNullOrWhiteSpace(txtTitulo.Text))
            {
                esValido = false;
                erpTitulo.SetError(txtTitulo, "El campo Título es obligatorio");
            }
            if (string.IsNullOrWhiteSpace(txtSinopsis.Text))
            {
                esValido = false;
                erpSinopsis.SetError(txtSinopsis, "El campo Sinopsis es obligatorio");
            }
            if (string.IsNullOrWhiteSpace(txtDirector.Text))
            {
                esValido = false;
                erpDirector.SetError(txtDirector, "El campo Director es obligatorio");
            }
            if (nudEpisodio.Value == nudEpisodio.Minimum)
            {
                esValido = false;
                erpEpisodio.SetError(nudEpisodio, "Por favor, seleccione un número de episodios válido");
            }

            return esValido;
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (validar())
            {
                var serie = new Serie();
                serie.titulo = txtTitulo.Text.Trim();
                serie.sinopsis = txtSinopsis.Text.Trim();
                serie.director = txtDirector.Text.Trim();
                serie.tipoClasificacion = cbxtipoClasificacion.Text.Trim();
                serie.episodios = (int)nudEpisodio.Value;
                serie.fechaEstreno = dtpFechaEstreno.Value;

                if (esNuevo)
                {
                    serie.estado = 1;
                    SerieCln.insertar(serie);
                }
                else
                {
                    int index = dgvLista.CurrentCell.RowIndex;
                    serie.id = Convert.ToInt32(dgvLista.Rows[index].Cells["id"].Value);
                    SerieCln.actualizar(serie);
                }
                Listar();
                btnCancelar.PerformClick();
                MessageBox.Show("Serie guardada correctamente", "::: Serie - Mensaje :::",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void LimpiarCampos()
        {
            txtTitulo.Clear();
            txtSinopsis.Clear();
            txtDirector.Clear();
            nudEpisodio.Value = nudEpisodio.Minimum;
            dtpFechaEstreno.Value = DateTime.Now;
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            int index = dgvLista.CurrentCell.RowIndex;
            int id = Convert.ToInt32(dgvLista.Rows[index].Cells["id"].Value);
            string titulo = dgvLista.Rows[index].Cells["titulo"].Value.ToString();
            DialogResult dialog =
                MessageBox.Show($"¿Está seguro que desea dar de baja el serie con titulo {titulo}?",
                "::: Serie - Mensaje :::", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);

            if (dialog == DialogResult.OK)
            {
                SerieCln.eliminar(id);
                Listar();
                MessageBox.Show("Serie eliminada correctamente", "::: Minerva - Mensaje :::",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void FrmSerie_Load(object sender, EventArgs e)
        {
            Size = new Size(860, 349);
            Listar();
        }
    }

}
