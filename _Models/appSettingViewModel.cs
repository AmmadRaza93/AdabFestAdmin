using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdabFest_Admin._Models
{
    public class appSettingViewModel
    {
    }

    public class AppSetingBLL
    {       
        public string About { get; set; }
        public string AppName { get; set; }
        public string AppVersion { get; set; }
        public string Image { get; set; } = "";
        public string FacebookUrl { get; set; }
        public string InstagramUrl { get; set; }        
        public string PdfUrl { get; set; } = "";
        public int SettingID { get; set; }
        public string SplashScreen { get; set; } = "";
        public int? StatusID { get; set; }
        public string TwitterUrl { get; set; }
        public string YoutubeUrl { get; set; }
        public IFormFile File { get; set; }               
    }

}

 
 