using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using ChupeLupe.Models;

namespace ChupeLupe.Services
{
    public interface IWebServicesApi
    {
        Task<List<Promotion>> GetPromotions();
    }
}
