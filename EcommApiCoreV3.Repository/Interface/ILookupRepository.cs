﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using EcommApiCoreV3.Entities;

namespace EcommApiCoreV3.Repository.Interface
{
    public interface ILookupRepository
    {
        Task<List<LookupColor>> GetActiveColor();
        Task<List<LookupSize>> GetActiveSize();
        Task<List<LookupOrderStatus>> GetOrderStatus();
    }
}
