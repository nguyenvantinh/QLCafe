using QuanLyQuanCafe.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCafe.DAO
{
    public class BillDAO
    {
        private static BillDAO instance;

        public static BillDAO Instance
        {
            get
            {
                if (instance == null) instance = new BillDAO();
                return instance;
            }

            private set
            {
                instance = value;
            }
        }

        private BillDAO() { }

        public void CheckOut(int idBill,int discount,float totalPrice)
        {
            string query = "UPDATE dbo.Bill SET status= 1,DateCheckOut =GETDATE(),totalPrice = " +totalPrice + " ,discount=" + discount + "  WHERE id = '" + idBill + "'";
            DataProvider.Instance.ExecuteNonQuery(query);
        }

        public DataTable GetBillListByDate(DateTime checkFrom,DateTime checkTo)
        {
            return DataProvider.Instance.ExecuteQuery("EXEC USP_GetListBillByDate @checkFrom , @checkTo", new object[] { checkFrom,checkTo});
        }

         public int GetUnCheckBillGetByID(int idTable)
        {
            DataTable data = DataProvider.Instance.ExecuteQuery("SELECT * FROM dbo.Bill WHERE idTable = '"+idTable+"' AND status = '0'");
            if (data.Rows.Count > 0)
            {
                Bill bill = new Bill(data.Rows[0]);
                return bill.ID; 
            }
            return -1;
        }

        public void InsertBill(int idTable)
        {
            string query = "EXEC USP_InsertBill @idBill";
            DataProvider.Instance.ExecuteNonQuery(query, new object[] { idTable});
        }

        public int GetMaxIDBill()
        {
            string query = "SELECT MAX(id) FROM dbo.Bill";
            try
            {
                return (int)DataProvider.Instance.ExecuteScalar(query);
            }
            catch
            {
                // Nếu chưa có bill nào thì khởi tạo là 1
                return 1;
            }
        }
    }
}
