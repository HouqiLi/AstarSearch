using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AstarSearch
{
    class AstarAlgo
    {
        List<AstarPoint> astar_open_list = new List<AstarPoint>();
        List<AstarPoint> astar_close_list = new List<AstarPoint>();

        byte[,] R;

        public AstarAlgo(byte[,] arr) 
        {
            this.R = arr;
        }

        //从开启列表查找F值最小的节点
        private AstarPoint get_min_from_open_list()
        {
            AstarPoint pmin = null;

            foreach (AstarPoint p in astar_open_list)
            {
                if (pmin == null || pmin.G + pmin.H > p.G + p.H) 
                {
                    pmin = p;
                }

            }
            return pmin;
        }

        //判断某个点是否为障碍物
        private bool is_bar(AstarPoint p, byte[,] map)
        {
            return (map[p.y, p.x] == 0);
        }

        //判断某个点是否在开启或关闭列表内
        private bool is_in_list(int x, int y, List<AstarPoint> list)
        {
            foreach (AstarPoint p in list)
            {
                if (p.x == x && p.y == y) {
                    return true;
                }
            }
            return false;
        }

        //从列表内找到对应坐标的点
        private AstarPoint get_point_from_list(int x, int y, List<AstarPoint> list)
        {
            foreach (AstarPoint p in list)
            {
                if (p.x == x && p.y == y)
                {
                    return p;
                }
            }
            return null;
        }

        //G表示从起点走的距离，横或竖一格10，斜着14
        private int get_G(AstarPoint p)
        {
            if (p.father == null) return 0;
            if (p.x == p.father.x || p.y == p.father.y)
            {
                return p.father.G + 10;
            }
            else
            {
                return p.father.G + 14;
            }
        }

        //计算p到dist的距离，一格10
        private int get_H(AstarPoint p, AstarPoint dist)
        {
            return Math.Abs(p.x - dist.x) * 10 + Math.Abs(p.y - dist.y) * 10;
        }

        private void check_p8(AstarPoint p0, byte[,] map, AstarPoint pa, ref AstarPoint pb)
        {
            for (int xt = p0.x - 1; xt <= p0.x + 1; xt++)
            {
                for (int yt = p0.y - 1; yt <= p0.y + 1; yt++)
                {
                    //排除超过边界和关闭自身的点
                    if ((xt >= 0 && xt < R.GetLength(0) && yt >= 0 && yt < R.GetLength(1)) &&
                        !(xt == p0.x && yt == p0.y))
                    {
                        //非障碍点和非关闭列表中的点
                        if (map[yt, xt] != 0 && !is_in_list(xt, yt, astar_close_list))
                        {
                            if (is_in_list(xt, yt, astar_open_list))
                            {
                                AstarPoint pt = get_point_from_list(xt, yt, astar_open_list);
                                int new_G = p0.G + ((p0.x == pt.x || p0.y == pt.y) ? 10 : 14);
                                if (new_G < pt.G)
                                {
                                    astar_open_list.Remove(pt);
                                    pt.father = p0;
                                    pt.G = new_G;
                                    astar_open_list.Add(pt);
                                }
                            }
                            else
                            {
                                AstarPoint pt = new AstarPoint();
                                pt.x = xt;
                                pt.y = yt;
                                pt.father = p0;
                                pt.G = get_G(pt);
                                pt.H = get_H(pt, pb);
                                astar_open_list.Add(pt);
                            }
                        }
                    }
                }
            }
        }

        //pa为起点，pb为终点
        public List<AstarPoint> astar_find_way(AstarPoint pa, AstarPoint pb)
        {
            List<AstarPoint> plist = new List<AstarPoint>();
            astar_open_list.Add(pa);
            while (!(is_in_list(pb.x, pb.y, astar_open_list)) || astar_open_list.Count == 0)
            {
                AstarPoint p0 = get_min_from_open_list();
                if (p0 == null) return null;
                astar_open_list.Remove(p0);
                astar_close_list.Add(p0);
                check_p8(p0, R, pa, ref pb);
            }

            AstarPoint p = get_point_from_list(pb.x, pb.y, astar_open_list);
            while (p.father != null)
            {
                plist.Add(p);
                p = p.father;
            }
            return plist;
        }
    }
}
