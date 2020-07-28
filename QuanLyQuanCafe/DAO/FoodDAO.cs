using QuanLyQuanCafe.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCafe.DAO
{
    public class FoodDAO
    {
        private static FoodDAO instance;

        public static FoodDAO Instance
        {
            get
            {
                if (instance == null) instance = new FoodDAO();
                return instance;
            }

            private set
            {
                instance = value;
            }
        }

        private FoodDAO() { }

        public List<Food> GetListFoodByIDCategory(int idCategory)
        {
            List<Food> lstFood = new List<Food>();

            string query = "SELECT * FROM dbo.Food WHERE idCategory = '" + idCategory + "'";

            DataTable data = DataProvider.Instance.ExecuteQuery(query);

            foreach(DataRow item in data.Rows)
            {
                Food food = new Food(item);
                lstFood.Add(food);
            }

            return lstFood;
        }

        public List<Food> GetListFood()
        {
            List<Food> lstFood = new List<Food>();

            string query = "SELECT * FROM dbo.Food";
            DataTable data = DataProvider.Instance.ExecuteQuery(query);

            foreach(DataRow item in data.Rows)
            {
                Food food = new Food(item);
                lstFood.Add(food);
            }
            return lstFood;
        }

        public bool InsertFood(string name, int idCategory, float price )
        {

            string query = string.Format("INSERT dbo.Food ( name, idCategory, price ) VALUES  ( N'{0}', {1}, {2})",name,idCategory,price);
            int result = DataProvider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }

        public bool UpdateFood(int id,string name,int idCategory,float price)
        {
            string query = string.Format("UPDATE dbo.Food SET name = N'{0}', idCategory = '{1}', price = '{2}' WHERE id = '{3}'",name,idCategory,price,id);
            int result = DataProvider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }

        public bool DeleteFood(int id)
        {
            // Trước khi xóa món thì phải xóa dữ liệu ở BillInfo
            BillInfoDAO.Instance.DeleteBillInfoByID(id);
            string query = string.Format("Delete dbo.Food WHERE id = '{0}'",id);
            int result = DataProvider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }

        public List<Food> SearchFoodByName(string name)
        {
            List<Food> lstFood = new List<Food>();

            string query = string.Format("SELECT * FROM dbo.Food WHERE dbo.fuConvertToUnsign1(name) LIKE N'%' + dbo.fuConvertToUnsign1(N'{0}') +'%'", name);
            DataTable data = DataProvider.Instance.ExecuteQuery(query);

            foreach (DataRow item in data.Rows)
            {
                Food food = new Food(item);
                lstFood.Add(food);
            }
            return lstFood;
        }

    }
}
