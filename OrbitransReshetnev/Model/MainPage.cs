using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrbitransReshetnev.Model
{
    public class MainPage
    {
        public DateTime DateNow { get; set; }

        public MainPage()
        {
            DateNow = DateTime.Now; // Устанавливаем текущую дату по умолчанию
        }
    }
}
