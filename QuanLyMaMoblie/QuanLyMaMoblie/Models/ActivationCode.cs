using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Graphics;

namespace QuanLyMaMoblie.Models
{
    public partial class ActivationCode : ObservableObject
    {
        public int dk { get; set; }

        [ObservableProperty]
        private string maCode;

        [ObservableProperty]
        private int dinhKy;

        [ObservableProperty]
        private double donGiaKy;

        [ObservableProperty]
        private string tinhTrangMa;

        [ObservableProperty]
        private DateTime ngayTao;

        [ObservableProperty]
        private DateTime ngayHetHan;

        [ObservableProperty]
        private Color frameColor5;

        [ObservableProperty]
        private Color frameColor6;
    }
}
