using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AstarSearch
{
    class AstarPoint
    {
        public int x, y, G, H;
        public AstarPoint father;
        public AstarPoint() { }
        public AstarPoint(int x0, int y0, int G0, int H0, AstarPoint F)
        {
            x = x0;
            y = y0;
            G = G0;
            H = H0;
            father = F;
        }
    }
}
