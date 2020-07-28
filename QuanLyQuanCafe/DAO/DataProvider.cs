using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCafe.DAO
{
    public class DataProvider
    {
        private static DataProvider instance;

        string connectionSTR = "Data Source=TINH-PC\\SQLEXPRESS_Tinh;Initial Catalog=QuanLyQuanCafe;Integrated Security=True";
        // Khởi tạo tính đóng gói : Ctrl + R + E
        public static DataProvider Instance
        {
            get
            {
                if(instance == null) { instance = new DataProvider(); };
                return instance;
            }

            private set
            {
                instance = value;
            }
        }
        // Tạo hàm dựng contructor
        private DataProvider()
        {

        }
        public DataTable ExecuteQuery(string query,object[] parameter = null)
        {
            DataTable data = new DataTable();

            using (SqlConnection connection = new SqlConnection(connectionSTR)) {
                connection.Open();
                SqlCommand command = new SqlCommand(query, connection);
                if(parameter != null)
                {
                    string[] listPara = query.Split(' ');
                    int i = 0;
                    foreach(string item in listPara)
                    {
                        if (item.Contains('@'))
                        {//parameter[0] được gán cho ts đầu tiên, parameter[1] gán cho ts thứ 2....
                            command.Parameters.AddWithValue(item, parameter[i]); // truyền giá trị(parameter) cho tham số(item) trong câu query ( item là tham số cgl biến, parameter[i] là gtri của ts tương ứng thứ i
                            i++;// vd .executequery("k9","123") query là ...@username , @password thì k9 được gán cho biến @username, 123 được gán cho biến @password để truyền gt của hai biến này xuống thực thi procedure
                        }
                    }
                }
                
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(data);
                connection.Close();

                
            }

            return data;

        }

        // Đếm số dòng thành công
        public int ExecuteNonQuery(string query, object[] parameter = null)
        {
            int data = 0;

            using (SqlConnection connection = new SqlConnection(connectionSTR))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(query, connection);
                if (parameter != null)
                {
                    string[] listPara = query.Split(' ');
                    int i = 0;
                    foreach (string item in listPara)
                    {
                        if (item.Contains('@'))
                        {
                            command.Parameters.AddWithValue(item, parameter[i]);
                            i++;
                        }
                    }
                }
                data = command.ExecuteNonQuery();
                connection.Close();


            }

            return data;

        }
        // Trả về đối tượng cột đầu tiên của dòng
        public object ExecuteScalar(string query, object[] parameter = null)
        {
            object data = 0;

            using (SqlConnection connection = new SqlConnection(connectionSTR))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(query, connection);
                if (parameter != null)
                {
                    string[] listPara = query.Split(' ');
                    int i = 0;
                    foreach (string item in listPara)
                    {
                        if (item.Contains('@'))
                        {
                            command.Parameters.AddWithValue(item, parameter[i]);
                            i++;
                        }
                    }
                }
                data = command.ExecuteScalar();
                connection.Close();


            }

            return data;

        }
    }
}
