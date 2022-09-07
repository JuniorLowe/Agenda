using System.Data.SqlClient;

namespace Agenda.UIDesktop
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnSalvar_Click(object sender, EventArgs e)
        {
            string nome = txtContatoNovo.Text;
            


            string strCon = @"Data Source=DESKTOP-C1V2GFD\SQLEXPRESS;
                            Initial Catalog=Agenda;Integrated Security=True;Connect Timeout=30;Encrypt=False;
                            TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
                       
            SqlConnection conn = new SqlConnection(strCon);

            conn.Open();

            string id = Guid.NewGuid().ToString();

            string sql = String.Format("INSERT INTO Contato (Id, Nome) VALUES ('{0}', '{1}');", id, nome);

            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.ExecuteNonQuery();

            sql = String.Format("select nome from contato where Id='{0}'", id);
            cmd = new SqlCommand(sql, conn);
            

            txtContatoSalvo.Text = cmd.ExecuteScalar().ToString();

            conn.Close();
        }
    }
}