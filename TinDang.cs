using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoAnNhom
{
    public class TinDang
    {
        public int Id { get; set; }
        public string TieuDe { get; set; }
        public string Loai { get; set; }        // MuaBan / ChoThue
        public string LoaiBDS { get; set; }     // NhaO / CanHo / Dat / BietThu
        public decimal Gia { get; set; }
        public decimal DienTich { get; set; }
        public string DiaChi { get; set; }
        public string QuanHuyen { get; set; }
        public string ThanhPho { get; set; }
        public string MoTa { get; set; }
        public string HinhAnh { get; set; }
        public int NguoiDangId { get; set; }
        public DateTime NgayDang { get; set; }
    }
}
