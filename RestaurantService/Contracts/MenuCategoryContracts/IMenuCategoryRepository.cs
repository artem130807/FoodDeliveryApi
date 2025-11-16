using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RestaurantService.Models;

namespace RestaurantService.Contracts.MenuCategoryContracts
{
    public interface IMenuCategoryRepository
    {
        Task<List<Models.MenuCategory>> GetMenuCategory();
        Task<Models.MenuCategory> GetMenuCategoryById(Guid Id);
        Task CreateMenuCategory(Models.MenuCategory menuCategory);
        Task<Models.MenuCategory> UdpateMenuCategory(Guid Id ,Models.MenuCategory menuCategory);
        Task DeleteMenuCategory(Guid Id);
        
    }
}