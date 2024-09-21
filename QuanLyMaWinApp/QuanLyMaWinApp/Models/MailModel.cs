using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyMaWinApp.Models
{
    public class MailModel
    {
        public string ID { get; set; }
        public string Title { get; set; }
        public string Detail { get; set; }
        public string ProductName { get; set; }
        public DateTime RequestDay { get; set; }
        public string Ocode { get; set; }
        public string Ncode { get; set; }
        public int dinhKyDaChon { get; set; }
        public double toTal { get; set; }
        public int isPay { get; set; }
        public string Status { get; set; }
        public string iconSee { get; set; }
        public int isDelete { get; set; }

        public MailModel()
        {
            iconSee = "eye.png";
        }

        public static List<MailModel> SampleData = new List<MailModel>
        {
            new MailModel
            {
                ID = "M001",
                Title = "Request a new activation code for FT - NIR Spectrometer MPA II - PIK Instruments",
                Detail = "Request a new activation code.",
                ProductName = "FT - NIR Spectrometer MPA II - PIK Instruments",
                RequestDay = new DateTime(2024, 4, 12),
                Ocode = "GKTK02938245",
                dinhKyDaChon = 1,
                toTal = 2500 * 0.1,
                isPay = 1,
                Status = "Progressing",
                isDelete = 0,
                iconSee = "eye.png"
            },
            new MailModel
            {
                ID = "M002",
                Title = "Request a new activation code for FT - NIR Spectrometer MPA II - PIK Instruments",
                Detail = "Request a new activation code.",
                ProductName = "FT - NIR Spectrometer MPA II - PIK Instruments",
                RequestDay = new DateTime(2024, 5, 16),
                Ocode = "GKTK02938245",
                dinhKyDaChon = 1,
                toTal = 2500 * 0.1,
                isPay = 1,
                Status = "Progressing",
                isDelete = 0,
                iconSee = "eye.png"
            }
        };
    }
}
