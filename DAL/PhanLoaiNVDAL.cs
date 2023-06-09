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
    public class PhanLoaiNVDAL
    {
        DatabaseAccess da = new DatabaseAccess();
        string procedure = "spPHANLOAINV";
        private SqlParameter[] GetParametersArray(PhanLoaiNV plnv)
        {
            return new SqlParameter[]
            {
                new SqlParameter("@MaLoai", plnv.MaLoai),
                new SqlParameter("@TenLoai", plnv.TenLoai)
            };
        }

        private PhanLoaiNV GetPhanLoaiNVFromDataRow(DataRow row)
        {
            PhanLoaiNV plnv = new PhanLoaiNV();
            plnv.MaLoai = int.Parse(row["MALOAI"].ToString());
            plnv.TenLoai = row["TENLOAI"].ToString();
            return plnv;
        }

        public PhanLoaiNV[] GetList(PhanLoaiNV plnv)
        {
            PhanLoaiNV[] list = null;
            DataTable table = da.ExecuteQuery(procedure, "Select", GetParametersArray(plnv));
            int len = table.Rows.Count;
            if (len == 0) { return null; }
            list = new PhanLoaiNV[len];
            for (int i = 0; i < len; i++)
            {
                list[i] = GetPhanLoaiNVFromDataRow(table.Rows[i]);
            }
            return list;
        }

        public int Create(PhanLoaiNV plnv)
        {
            return da.ExecuteNonQuery(procedure, "Create", GetParametersArray(plnv));
        }

        public int Update(PhanLoaiNV plnv)
        {
            return da.ExecuteNonQuery(procedure, "Update", GetParametersArray(plnv));
        }

        public int Delete(PhanLoaiNV plnv)
        {
            return da.ExecuteNonQuery(procedure, "Delete", GetParametersArray(plnv));
        }

        public int Restore(PhanLoaiNV plnv)
        {
            return da.ExecuteNonQuery(procedure, "Restore", GetParametersArray(plnv));
        }
    }
}
