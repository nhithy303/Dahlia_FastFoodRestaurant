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
    public class HoaDonBanHangDAL
    {
        DatabaseAccess da = new DatabaseAccess();
        string procedure = "spHOADONBANHANG";

        private SqlParameter[] GetParametersArray(HoaDonBanHang hdbh)
        {
            return new SqlParameter[]
            {
                new SqlParameter("@MaHD", hdbh.MaHD),
                new SqlParameter("@MaNV", hdbh.MaNV),
                new SqlParameter("@NgayHD", hdbh.NgayHD),
                new SqlParameter("@TongTien", hdbh.TongTien),
                new SqlParameter("@ThanhToan", hdbh.ThanhToan),
                new SqlParameter("@TrangThai", hdbh.TrangThai)
            };
        }

        private HoaDonBanHang GetHoaDonBanHangFromDataRow(DataRow row)
        {
            HoaDonBanHang hdbh = new HoaDonBanHang();
            hdbh.MaHD = int.Parse(row["MAHD"].ToString());
            hdbh.MaNV =  int.Parse(row["MANV"].ToString());
            hdbh.NgayHD = DateTime.Parse(row["NGAYHD"].ToString()).ToString("dd/MM/yyyy");
            hdbh.TongTien = int.Parse(row["TONGTIEN"].ToString());
            hdbh.ThanhToan = int.Parse(row["THANHTOAN"].ToString());
            hdbh.TrangThai = int.Parse(row["TRANGTHAI"].ToString());
            return hdbh;
        }

        public HoaDonBanHang[] GetList(HoaDonBanHang hdbh)
        {
            HoaDonBanHang[] list = null;
            DataTable table = da.ExecuteQuery(procedure, "Select", GetParametersArray(hdbh));
            int len = table.Rows.Count;
            if (len == 0) { return null; }
            list = new HoaDonBanHang[len];
            for (int i = 0; i < len; i++)
            {
                list[i] = GetHoaDonBanHangFromDataRow(table.Rows[i]);
            }
            return list;
        }

        public int GetLatest()
        {
            DataTable table = da.ExecuteQuery(procedure, "GetLatest", new SqlParameter[] { });
            if (table.Rows.Count > 0)
            {
                return int.Parse(table.Rows[0][0].ToString());
            }
            return 0;
        }

        public int Create(HoaDonBanHang hdbh)
        {
            return da.ExecuteNonQuery(procedure, "Create", GetParametersArray(hdbh));
        }

        public int Update(HoaDonBanHang hdbh)
        {
            return da.ExecuteNonQuery(procedure, "Update", GetParametersArray(hdbh));
        }

        public int Delete(HoaDonBanHang hdbh)
        {
            return da.ExecuteNonQuery(procedure, "Delete", GetParametersArray(hdbh));
        }
    }
}
