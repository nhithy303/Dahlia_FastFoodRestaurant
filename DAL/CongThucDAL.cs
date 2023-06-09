﻿using DTO;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class CongThucDAL
    {
        DatabaseAccess da = new DatabaseAccess();
        string procedure = "spCONGTHUC";

        private SqlParameter[] GetParametersArray(CongThuc ct)
        {
            return new SqlParameter[]
            {
                new SqlParameter("@MaMon", ct.MaMon),
                new SqlParameter("@MaNL", ct.MaNL),
                new SqlParameter("@SoLuong", ct.SoLuong),
                new SqlParameter("@DonViTinh", ct.DonViTinh),
                new SqlParameter("@GiaNL", ct.GiaNL)
            };
        }

        private CongThuc GetCongThucFromDataRow(DataRow row)
        {
            CongThuc ct = new CongThuc();
            ct.MaMon = int.Parse(row["MAMON"].ToString());
            ct.MaNL = int.Parse(row["MANL"].ToString());
            ct.SoLuong = int.Parse(row["SOLUONG"].ToString());
            ct.DonViTinh = int.Parse(row["DONVITINH"].ToString());
            ct.GiaNL = int.Parse(row["GIANL"].ToString());
            return ct;
        }

        public CongThuc[] GetList(CongThuc ct)
        {
            CongThuc[] list = null;
            DataTable table = da.ExecuteQuery(procedure, "Select", GetParametersArray(ct));
            int len = table.Rows.Count;
            if (len == 0) { return null; }
            list = new CongThuc[len];
            for (int i = 0; i < len; i++)
            {
                list[i] = GetCongThucFromDataRow(table.Rows[i]);
            }
            return list;
        }

        public int Create(CongThuc ct)
        {
            return da.ExecuteNonQuery(procedure, "Create", GetParametersArray(ct));
        }

        public int Update(CongThuc ct)
        {
            return da.ExecuteNonQuery(procedure, "Update", GetParametersArray(ct));
        }

        public int Delete(CongThuc ct)
        {
            return da.ExecuteNonQuery(procedure, "Delete", GetParametersArray(ct));
        }
    }
}
