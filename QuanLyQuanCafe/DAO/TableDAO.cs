using QuanLyQuanCafe.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCafe.DAO
{
    public class TableDAO
    {
        private static TableDAO instance;

        public static int tableWidth = 100;
        public static int tableHeight = 100;
        public static TableDAO Instance
        {
            get
            {
                if (instance == null) instance = new TableDAO();//một đối tượng TableDao được tạo mới do biến instance quản lý <-> đối tượng đó có tên là instance
                return instance;//Instance sẽ mang gt của instance (<-> tên hàm(ts,ts) sẽ mang gt của biến trả về) instance = Instance = đối tượng vừa được tạo mới
            }// TableDao.Instance ( vì Instance là static) <-> một đối tượng của lớp TableDao được tạo mới => chấm/gọi được các phương thức trong TableDao

            private set
            {
                instance = value;
            }
        }

        private TableDAO() { }

        public void SwitchTable(int id1, int id2)
        {
            DataProvider.Instance.ExecuteQuery("USP_SwitchTabel @idTable1 , @idTabel2", new object[] { id1, id2 });
        }

        public List<Table> LoadTableList()
        {
            List<Table> tableList = new List<Table>();
            DataTable data = DataProvider.Instance.ExecuteQuery("USP_GetTableList");
            foreach(DataRow item in data.Rows)//lấy ra các hàng trong bảng của sql(datatable), item có/quản lý các trường của 1 hàng/ 1 bản ghi
            {//truyền item có các gt tương ứng vào hàm Table(DataRow row) để tạo đối tượng table có các thuôc tính id name status có giá trị tương ứng với các gt của các thuộc tính item có/quản lý
                Table table = new Table(item); // biến table quản lý đối tượng/vungnho(table được tạo) / một đối tượng table được tạo có các ttinh id nam status
                // <=> các trường trong class Table đã có dl, mỗi lần new là các trường trong Table của dto lại có các gt mới được lấy ra thông qua item
                tableList.Add(table);// add đối tượng table này vào list, mỗi ptu tableList[i] quản lý một đối tượng table có 3 thuộc tính nam status id
            }
            return tableList;//mỗi phần tử của list là một table có các gt id name status
        }
        //lấy gt của các trường trong từng hàng của datarow thông qua item, truyền item vào Table(hàm tạo) để set gtri cho các
        // trường tương ứng của Table-item mang gt của các trường trong csdl(tạo mới đối tượng table)
    }
}
