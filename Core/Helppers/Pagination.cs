﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Helppers
{
    public class Pagination<T> where T : class
    {
        public Pagination(int pageIndex, int pageSize, int count, IReadOnlyList<T> data)
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
            PagesCount = (int)Math.Ceiling(Convert.ToDouble(count) / Convert.ToDouble(pageSize));
            Count = count;
            Data = data;
        }

        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int PagesCount { get; set; }
        public int Count { get; set; }
        public IReadOnlyList<T> Data { get; set; }

    }
}
