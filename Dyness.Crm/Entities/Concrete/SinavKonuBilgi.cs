using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class SinavKonuBilgi
    {
        public string DersAd { get; set; }

        public string Konu { get; set; }

        public int SoruAdet { get; set; }

        public int DogruAdet { get; set; }

        public int YanlisAdet { get; set; }

        public int BosAdet { get; set; }

        public double Basari => DogruAdet == 0 || SoruAdet == 0 ? 0 : 100 * DogruAdet / SoruAdet;
    }
}
