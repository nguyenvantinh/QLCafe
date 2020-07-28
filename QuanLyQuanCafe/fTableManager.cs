using QuanLyQuanCafe.DAO;
using QuanLyQuanCafe.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static QuanLyQuanCafe.fAccountProfile;

namespace QuanLyQuanCafe
{
    public partial class fTableManager : Form
    {
        private Account loginAccount;

        public Account LoginAccount
        {
            get
            {
                return loginAccount;
            }

            set
            {
                loginAccount = value;
                ChangeAccount(loginAccount.Type);
            }
        }

        public fTableManager(Account acc)
        {
            InitializeComponent();

            this.LoginAccount = acc;

            LoadTable();
            LoadCategory();
            //lsvBill.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            //lsvBill.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
            LoadComboBoxTable(cbSwitchTable);
        }
        #region Methods
        void ChangeAccount(int type)
        {
            adminToolStripMenuItem.Enabled = type == 1 ? true : false;
            thôngTinTàiKhoảnToolStripMenuItem.Text += " (" + LoginAccount.DisplayName + ")";
        }

        void LoadCategory()
        {
            List<Category> lstCategory = CategoryDAO.Instance.GetListCategory();
            cbCategory.DataSource = lstCategory;
            cbCategory.DisplayMember = "name";
        }

        void LoadFoodListByCategoryID(int id)
        {
            List<Food> lstFood = FoodDAO.Instance.GetListFoodByIDCategory(id);
            cbFood.DataSource = lstFood;
            cbFood.DisplayMember = "name";
        }

        void LoadTable()
        {
            flpTable.Controls.Clear();
            List<Table> tableList = TableDAO.Instance.LoadTableList();//mỗi ptu của tablelist là một table có các ttinh là các trường trong class table của dto
            foreach(Table table in tableList) //lấy từng đt table trong tablelist ra , Table item or Table table cũng ok Table table hly hơn vì trong tablelist là các table có các thuộc tính id name status
            {
                Button button = new Button() { Width = TableDAO.tableWidth, Height = TableDAO.tableHeight };
                button.Text = table.Name + Environment.NewLine + table.Status;
                button.Click += Button_Click1;
                button.Tag = table;
                switch (table.Status)
                {
                    case "Trống":
                        button.BackColor = Color.Aqua;
                        break;
                    default:
                        button.BackColor = Color.YellowGreen;
                        break;
                }
                flpTable.Controls.Add(button);
            }
        }
        void LoadComboBoxTable(ComboBox cb)
        {
            cb.DataSource = TableDAO.Instance.LoadTableList();
            cb.DisplayMember = "Name";
        }

        void ShowBill(int tableID)
        {
            lsvBill.Items.Clear();
            List<DTO.Menu> listBillInfo = MenuDAO.Instance.GetListMenuByTable(tableID);
            float totalPrice = 0;

            foreach(DTO.Menu item in listBillInfo)
            {
                ListViewItem lsvItem = new ListViewItem(item.FoodName.ToString());
                lsvItem.SubItems.Add(item.Count.ToString());
                lsvItem.SubItems.Add(item.Price.ToString());
                lsvItem.SubItems.Add(item.TotalPrice.ToString());
                totalPrice += item.TotalPrice;
                lsvBill.Items.Add(lsvItem);
            }
            CultureInfo culture = new CultureInfo("vi-VN");
            Thread.CurrentThread.CurrentCulture = culture;
            txbTotalPrice.Text = totalPrice.ToString("c");
            
        }


        #endregion

        #region Events
        private void Button_Click1(object sender, EventArgs e)
        {
            int tableID = (((sender as Button).Tag) as Table).Id;
            // Lấy dữ liệu bàn gán vào lsvFood
            lsvBill.Tag = (sender as Button).Tag;
            ShowBill(tableID);
        }

        private void đăngXuấtToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void thôngTinCáNhânToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fAccountProfile f = new fAccountProfile(LoginAccount);
            f.UpdateAccount += F_UpdateAccount;
            f.ShowDialog();
        }
        // truyền dữ liệu từ form AccountProfile sang TableManager
        private void F_UpdateAccount(object sender, AccountEvent e)
        {
            thôngTinTàiKhoảnToolStripMenuItem.Text = "Thông tin tài khoản (" + e.Acc.DisplayName + ")";
        }

        private void adminToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fAdmin f = new fAdmin();
            f.InsertFood += F_InsertFood;
            f.UpdateFood += F_UpdateFood;
            f.DeleteFood += F_DeleteFood;
            f.ShowDialog();
        }

        private void F_DeleteFood(object sender, EventArgs e)
        {
            LoadFoodListByCategoryID((cbCategory.SelectedItem as Category).ID);
            if(lsvBill.Tag!=null)
                ShowBill((lsvBill.Tag as Table).Id);
            LoadTable();
        }

        private void F_UpdateFood(object sender, EventArgs e)
        {
            LoadFoodListByCategoryID((cbCategory.SelectedItem as Category).ID);
            if (lsvBill.Tag != null)
                ShowBill((lsvBill.Tag as Table).Id);
        }

        private void F_InsertFood(object sender, EventArgs e)
        {
            LoadFoodListByCategoryID((cbCategory.SelectedItem as Category).ID);
            if (lsvBill.Tag != null)
                ShowBill((lsvBill.Tag as Table).Id);
        }

        private void cbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            int id = 0;
            ComboBox cb = sender as ComboBox;

            Category selected = cb.SelectedItem as Category;
            id = selected.ID;
            LoadFoodListByCategoryID(id);
        }

        private void btnAddFood_Click(object sender, EventArgs e)
        {
            Table table = lsvBill.Tag as Table;
            if(table == null)
            {
                MessageBox.Show("Chọn bàn trước khi thêm món.");
                return;
            }
            int idBill = BillDAO.Instance.GetUnCheckBillGetByID(table.Id);
            int idFood = (cbFood.SelectedItem as Food).ID;
            int count = (int)nmCountFood.Value;

            // nếu bàn không có bill thì tạo bill mới cho bàn đó
            if(idBill == -1)
            {
                BillDAO.Instance.InsertBill(table.Id);
                // Thêm BillInfo
                BillInfoDAO.Instance.InsertBillInfo(BillDAO.Instance.GetMaxIDBill(), idFood, count);

            }

            else
            {
                BillInfoDAO.Instance.InsertBillInfo(idBill, idFood, count);
            }
            ShowBill(table.Id);
            LoadTable();
        }

        private void btnCheckOut_Click(object sender, EventArgs e)
        {
            Table table = lsvBill.Tag as Table;

            int idBill = BillDAO.Instance.GetUnCheckBillGetByID(table.Id);
            int discount = (int)nmDiscount.Value;
            double totalPrice = double.Parse(txbTotalPrice.Text, NumberStyles.Currency);
            double finalTotalPrice = totalPrice - (totalPrice/100) * discount;
            if (idBill != -1)
            {
                if(MessageBox.Show(string.Format("Bạn có chắc muốn thanh toán hóa đơn cho {0} ?\n Tổng tiền - (Tổng tiền /100) * Giảm giá = {1} - ({1}/100)*{2} = {3}",table.Name,totalPrice,discount,finalTotalPrice),"Thông báo", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK)
                {
                    BillDAO.Instance.CheckOut(idBill,discount,(float)finalTotalPrice);
                    // Sau khi thanh toán thì Show lại bill --> đã chuyển status về 1 nên không hiện chi tiết bill
                    ShowBill(table.Id);
                    LoadTable();
                }
            }
        }

        private void btnSwitchTable_Click(object sender, EventArgs e)
        {
            int id1 = (lsvBill.Tag as Table).Id;
            int id2 = (cbSwitchTable.SelectedItem as Table).Id;

            if (MessageBox.Show(string.Format("Bạn có thật sự muốn chuyển {0} sang {1} không ?", (lsvBill.Tag as Table).Name, (cbSwitchTable.SelectedItem as Table).Name),"Thông báo",MessageBoxButtons.OKCancel)==System.Windows.Forms.DialogResult.OK)
            {
                TableDAO.Instance.SwitchTable(id1, id2);

                LoadTable();
            }

            
        }

        private void thôngTinTàiKhoảnToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        #endregion


    }
}
