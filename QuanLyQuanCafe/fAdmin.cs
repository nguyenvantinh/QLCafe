using QuanLyQuanCafe.DAO;
using QuanLyQuanCafe.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyQuanCafe
{
    public partial class fAdmin : Form
    {
        BindingSource foodlist = new BindingSource();
        public fAdmin()
        {
            InitializeComponent();
            LoadInitial();
        }
        #region methods
        void LoadInitial()
        {
            dtgvFood.DataSource = foodlist;
            
            LoadDateTimePickerBill();
            LoadBillListByDate(dtpkFromDate.Value, dtpkToDate.Value);
            LoadListFood();
            
            LoadCategoryIntoComboBox(cbCategory_food);
            AddFoodBinding();
        }

        void LoadDateTimePickerBill()
        {
            DateTime today = DateTime.Now;
            dtpkFromDate.Value = new DateTime(today.Year, today.Month, 1);
            dtpkToDate.Value = dtpkFromDate.Value.AddMonths(1).AddDays(-1);
        }

        void LoadBillListByDate(DateTime from,DateTime to)
        {
            dtgvBill.DataSource = BillDAO.Instance.GetBillListByDate(from,to);
        }

        void LoadListFood()
        {
            foodlist.DataSource = FoodDAO.Instance.GetListFood();
        }

        void AddFoodBinding()
        {
            txtbFoodName.DataBindings.Add(new Binding("Text",dtgvFood.DataSource,"name",true,DataSourceUpdateMode.Never));
            txbFoodID.DataBindings.Add(new Binding("Text", dtgvFood.DataSource, "id", true, DataSourceUpdateMode.Never));
            nmFoodPrice.DataBindings.Add(new Binding("Value", dtgvFood.DataSource, "price", true, DataSourceUpdateMode.Never));
        }

        void LoadCategoryIntoComboBox(ComboBox cb)
        {
            cb.DataSource = CategoryDAO.Instance.GetListCategory();
            cb.DisplayMember = "name";
        }

        List<Food> SearchFoodByName(string name)
        {
            List<Food> lstFood = new List<Food>();
            lstFood = FoodDAO.Instance.SearchFoodByName(name);
            return lstFood;
        }
        #endregion

        #region events
        private void btnViewBill_Click(object sender, EventArgs e)
        {
            LoadBillListByDate(dtpkFromDate.Value, dtpkToDate.Value);
        }

        private void btnShowFood_Click(object sender, EventArgs e)
        {
            LoadListFood();
        }


        private void txbFoodID_TextChanged(object sender, EventArgs e)
        {
            try
            {
                int id = (int)dtgvFood.SelectedCells[0].OwningRow.Cells["IDCategory"].Value;

                Category category = CategoryDAO.Instance.GetCategoryByID(id);

                cbCategory_food.SelectedIndex = category.ID - 1;
            }
            catch { }
        }

        private void btnAddFood_Click(object sender, EventArgs e)
        {
            string name = txtbFoodName.Text;
            int categoryID = (cbCategory_food.SelectedItem as Category).ID;
            float price = (float)nmFoodPrice.Value;

            if (FoodDAO.Instance.InsertFood(name, categoryID, price))
            {
                LoadListFood();
                MessageBox.Show("Thêm thành công");
                if (insertFood != null)
                {
                    insertFood(this, new EventArgs());
                }
            }
            else
            {
                MessageBox.Show("Có lỗi khi thêm thức ăn");
            }
        }

        private void btnUpdateFood_Click(object sender, EventArgs e)
        {
            string name = txtbFoodName.Text;
            int categoryID = (cbCategory_food.SelectedItem as Category).ID;
            float price = (float)nmFoodPrice.Value;
            int id = Convert.ToInt32(txbFoodID.Text);

            if (FoodDAO.Instance.UpdateFood(id, name, categoryID, price))
            {

                MessageBox.Show("Sửa món thành công");
                LoadListFood();
                if (updateFood != null)
                {
                    updateFood(this, new EventArgs());
                }
            }
            else
            {
                MessageBox.Show("Có lỗi khi sửa thức ăn");
            }
        }

        private void btnDeleteFood_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(txbFoodID.Text);

            if (FoodDAO.Instance.DeleteFood(id))
            {
                MessageBox.Show("Xóa món thành công");
                LoadListFood();
                if (deleteFood != null)
                {
                    deleteFood(this, new EventArgs());
                }
            }
            else
            {
                MessageBox.Show("Có lỗi khi xóa thức ăn");
            }
        }

        private event EventHandler insertFood;
        public event EventHandler InsertFood
        {
            add { insertFood += value; }
            remove { insertFood -= value; }
        }

        private event EventHandler updateFood;
        public event EventHandler UpdateFood
        {
            add { updateFood += value; }
            remove { updateFood -= value; }
        }

        private event EventHandler deleteFood;
        public event EventHandler DeleteFood
        {
            add { deleteFood += value; }
            remove { deleteFood -= value; }
        }


        #endregion

        private void btnSearchFood_Click(object sender, EventArgs e)
        {
            foodlist.DataSource= SearchFoodByName(txbSearchFoodName.Text);
        }
    }
}
