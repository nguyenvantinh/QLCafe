using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCafe.DTO
{
    public class Category
    {
        private int iD;
        private string name;

        public Category(int _id,string _name)
        {
            this.ID = _id;
            this.Name = _name;
        }

        public Category(DataRow row)
        {
            this.ID = (int)row["id"];
            this.Name =row["name"].ToString();
        }

        public int ID
        {
            get
            {
                return iD;
            }

            set
            {
                iD = value;
            }
        }

        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                name = value;
            }
        }
    }
}
