﻿using System.Data;
using System.Data.SqlClient;
using System.Windows;
using Microsoft.Reporting.WinForms;

namespace QuanLyHocSinh_OOAD
{
    /// <summary>
    /// Interaction logic for frmHocSinhBaoCao.xaml
    /// </summary>
    public partial class frmHocSinhBaoCao : Window    {
        //public static string strConnectionString = "Data Source=DESKTOP-DLT0AO8;Initial Catalog=QLHS;Integrated Security=True";
        //SqlConnection conn = new SqlConnection(strConnectionString);
        SqlConnection conn = Connection.KetNoi();
        public static string strName, strHieuTruong, strNamHoc;
        private int iKhoi;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ReportViewerDemo.Reset();
            DataTable dt = GetData();

            ReportDataSource dsDataSource = new ReportDataSource("DataSet1", dt); //ReportDataSource("", dt);
            ReportViewerDemo.LocalReport.DataSources.Add(dsDataSource);

            //embbedded to RDLC report file
            ReportViewerDemo.LocalReport.ReportEmbeddedResource = "QuanLyHocSinh_OOAD.HocSinh.rdlc";
            //ReportViewerDemo.LocalReport.ReportPath = "\\RDLC_Report\\GiaoVien.rdlc";


            //Get information about School such as school's name and principal's name
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "Select * from THONGTINTRG";
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    strName = dr.GetString(1);
                    strHieuTruong = dr.GetString(3);
                }

            }
            catch (SqlException)
            {
                MessageBox.Show("Có lỗi trong quá trình liên kết cơ sở dữ liệu");
            }

            conn.Close();




            //Parameters
            ReportParameter[] rptParameter = new ReportParameter[]
            {
                new ReportParameter("TenTruong", strName.ToUpper()),
                new ReportParameter("TenHT", strHieuTruong),
                new ReportParameter("NamHoc", strNamHoc),
                new ReportParameter("Khoi", iKhoi.ToString())
            };
            ReportViewerDemo.LocalReport.SetParameters(rptParameter);


            ReportViewerDemo.RefreshReport();
        }

        public frmHocSinhBaoCao(int Khoi, string NamHoc)
        {
            InitializeComponent();
            iKhoi = Khoi;
            strNamHoc = NamHoc;
        }

        private DataTable GetData()
        {
            DataTable dtDataTable = new DataTable();

            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT MAHS, HOTEN, NGSINH, GIOITINH, MALOP FROM HOCSINH WHERE NAMHOC = @NAMHOC AND LEFT(MALOP,2) = @KHOI", conn);
                cmd.Parameters.AddWithValue("@NAMHOC", strNamHoc);
                cmd.Parameters.AddWithValue("@KHOI", iKhoi.ToString());
                //SqlDataAdapter daDataAdapter = cmd.ExecuteNonQuery();
                //daDataAdapter = cmd.ExecuteNonQuery();
                //daDataAdapter.Fill(dtDataTable);
                dtDataTable.Load(cmd.ExecuteReader());
            }
            catch (SqlException)
            {
                MessageBox.Show("Có lỗi trong quá trình tạo báo cáo, mời bạn thử lại", "Thông báo", MessageBoxButton.OK);
            }

            conn.Close();
            return dtDataTable;

        }

    }
}
