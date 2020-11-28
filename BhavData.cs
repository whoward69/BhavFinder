using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BhavFinder
{

    public class BhavData : DataTable
    {
        private DataColumn colInstance = new DataColumn("Instance", typeof(string));
        private DataColumn colName = new DataColumn("Name", typeof(string));
        private DataColumn colGroupInstance = new DataColumn("GroupInstance", typeof(string));
        private DataColumn colGroupName = new DataColumn("GroupName", typeof(string));

        public BhavData()
        {
            this.Columns.Add(colInstance);
            this.Columns.Add(colName);
            this.Columns.Add(colGroupInstance);
            this.Columns.Add(colGroupName);
        }

        public void Append(uint instance, String name, uint groupInstance, String groupName)
        {
            this.Rows.Add("0x" + instance.ToString("X4"), name, "0x" + groupInstance.ToString("X8"), groupName);
        }

        public void Append(DataRow row)
        {
            this.Rows.Add(row);
        }
    }
}
