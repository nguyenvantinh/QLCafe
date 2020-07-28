using QuanLyQuanCafe.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCafe.DAO
{
    public class MenuDAO
    {
        private static MenuDAO instance;

        public static MenuDAO Instance
        {
            get
            {
                if (instance == null) instance = new MenuDAO();
                return instance;
            }

            private set
            {
                instance = value;
            }
        }

        private MenuDAO() { }

        public List<Menu> GetListMenuByTable(int idTable)
        {
            List<Menu> lstMenu = new List<Menu>();
            string query = "SELECT f.name,bi.count,f.price,f.price*bi.count AS totalPrice FROM dbo.Bill AS b, dbo.BillInfo AS bi,dbo.Food AS f WHERE b.id = bi.idBill AND bi.idFood = f.id AND b.status='0' AND b.idTable = '" + idTable +"'";
            DataTable data = DataProvider.Instance.ExecuteQuery(query);

            foreach(DataRow item in data.Rows)
            {
                Menu menu = new Menu(item);
                lstMenu.Add(menu);
            }

            return lstMenu; 
        }
    }
}
