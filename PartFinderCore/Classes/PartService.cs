using Microsoft.EntityFrameworkCore;
using PartFinderCore.Models;
using System.Linq.Expressions;

namespace PartFinderCore.Classes;

public class PartService
{
    private static string GetSortColumn(string? columnName)
    {
        if (columnName == null)
        {
            return "PartName";
        }
        return columnName.Replace("_desc", "") switch
        {
            "name" => "PartName",
            "description" => "PartDescription",
            "stock" => "StockLevel",
            "location" => "StorageName",
            "manufacturer" => "ManufacturerName",
            "footprint" => "FootprintName",
            "category" => "PCName",
            _ => "PartName"
        };
    }
    public static async Task<List<PartViewModel>> GetPartsAsync(string sortColumn, int skip, int pageSize, SiteContext dbContext, int filterFootprint = 0, int filterManufacturer = 0, int filterStorage = 0, int filterCategory = 0)
    {
        var sortDescending = sortColumn.EndsWith("desc");
        var filterColumn = GetSortColumn(sortColumn);

        var query = dbContext.Parts.Select(p => new PartViewModel
        {
            PartPkey = p.PartPkey,
            PartCategoryID = p.PartCategoryID,
            PartFootprintID = p.PartFootprintID,
            PartManID = p.PartManID,
            PartName = p.PartName,
            PartDescription = p.PartDescription!,
            PartComment = p.PartComment,
            StockLevel = p.StockLevel,
            MinStockLevel = p.MinStockLevel,
            Price = p.Price,
            DateCreated = p.DateCreated,
            DateUpdated = p.DateUpdated,
            Condition = p.Condition,
            StorageLocationID = p.StorageLocationID,
            MPN = p.MPN!,
            BarCode = p.BarCode!,
            StorageName = p.StorageLocation!.StorageName,
            StorageSortOrder = p.StorageLocation.StorageSortOrder,
            ManufacturerName = p.Manufacturer!.ManufacturerName,
            FootprintName = p.Footprint!.FootprintName,
            PCName = p.PartCategory!.PCName
        });

        if (filterFootprint > 0)
        {
            query = query.Where(p => p.PartFootprintID == filterFootprint);
        }
        if (filterManufacturer > 0)
        {
            query = query.Where(p => p.PartManID == filterManufacturer);
        }
        if (filterStorage > 0)
        {
            query = query.Where(p => p.StorageLocationID == filterStorage);
        }
        if (filterCategory > 0)
        {
            query = query.Where(p => p.PartCategoryID == filterCategory);
        }




        // Apply sorting dynamically
        if (!string.IsNullOrEmpty(filterColumn))
        {
            var parameter = Expression.Parameter(typeof(PartViewModel), "p");
            var property = typeof(PartViewModel).GetProperty(filterColumn);
            if (property != null)
            {
                var propertyAccess = Expression.MakeMemberAccess(parameter, property);
                var orderByExp = Expression.Lambda(propertyAccess, parameter);
                var methodName = sortDescending ? "OrderByDescending" : "OrderBy";
                var resultExp = Expression.Call(typeof(Queryable), methodName, [query.ElementType, property.PropertyType], query.Expression, Expression.Quote(orderByExp));
                query = query.Provider.CreateQuery<PartViewModel>(resultExp);
            }
        }

        // Apply pagination
        return await query.Skip(skip).Take(pageSize).ToListAsync();
    }
}