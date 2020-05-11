using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Data.SqlClient;

namespace Semana02
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["Leon"].ConnectionString);
        public void ListaAnios()
        {
            using (SqlCommand cmd = new SqlCommand("Usp_ListaAnios", cn))
            {
                using(SqlDataAdapter da = new SqlDataAdapter())
                {
                    da.SelectCommand = cmd;
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    using(DataSet df=new DataSet())
                    {
                        da.Fill(df, "ListaAnios");

                        CboAnios.DataSource = df.Tables["ListaAnios"];
                        CboAnios.DisplayMember = "Anios";
                        CboAnios.ValueMember = "Anios";
                    }
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ListaAnios();
        }

        private void CboAnios_SelectionChangeCommitted(Object sender,EventArgs e)
        {
            using(SqlCommand cmd = new SqlCommand("Usp_Lista_Pedidos_Anios", cn))
            {
                using(SqlDataAdapter Da = new SqlDataAdapter())
                {
                    Da.SelectCommand = cmd;
                    Da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    Da.SelectCommand.Parameters.AddWithValue("@anio", CboAnios.SelectedValue);
                    using(DataSet df = new DataSet())
                    {
                        Da.Fill(df,"Pedidos"); 

                        DgPedidos.DataSource = df.Tables["Pedidos"];
                        lblNumero.Text = df.Tables["Pedidos"].Rows.Count.ToString();
                    }
                }
            }
        }

        private void DgPedidos_DoubleClick(Object sender, EventArgs e)
        {
            int Codigo;
            Codigo = Convert.ToInt32(DgPedidos.CurrentRow.Cells[0].Value);
            using(SqlCommand cmd = new SqlCommand("Usp_Detalle_Pedidos",cn))
            {
                using(SqlDataAdapter Da=new SqlDataAdapter())
                {
                    Da.SelectCommand = cmd;
                    Da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    Da.SelectCommand.Parameters.AddWithValue("@idpedido", Codigo);
                    using(DataSet df = new DataSet())
                    {
                        Da.Fill(df, "detallesdepedidos");
                        DgDetalle.DataSource = df.Tables["detallesdepedidos"];
                        lblMonto.Text = df.Tables["detallesdepedidos"].Compute("Sum(Monto)", "").ToString();
                    }
                }
            }
        }
    }
}
