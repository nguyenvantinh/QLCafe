using QuanLyQuanCafe.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCafe.DAO
{
    public class BillInfoDAO
    {
        private static BillInfoDAO instance;

        public static BillInfoDAO Instance
        {
            get
            {
                if (instance == null) instance = new BillInfoDAO();
                return instance;
            }

            private set
            {
                instance = value;
            }
        }

        private BillInfoDAO() { }

        public void DeleteBillInfoByID(int idFood)
        {
            DataProvider.Instance.ExecuteQuery("DELETE dbo.BillInfo WHERE idFood = " + idFood);
        }

        public List<BillInfo> GetListBillInfo(int idBill)
        {
            List<BillInfo> listBillInfo = new List<BillInfo>();

            DataTable data = DataProvider.Instance.ExecuteQuery("SELECT * FROM dbo.BillInfo WHERE idBill = '" + idBill +"'");

            foreach(DataRow item in data.Rows)
            {
                BillInfo bInfo = new BillInfo(item);
                listBillInfo.Add(bInfo);
            }

            return listBillInfo;
        }

        public void InsertBillInfo(int _idBill, int _idFood, int _count)
        {
            string query = "EXEC USP_InsertBillInfo @idBill , @idFood , @count";
            DataProvider.Instance.ExecuteNonQuery(query, new object[] { _idBill, _idFood, _count });
        }
    }
}
