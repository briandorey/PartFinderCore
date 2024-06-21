using Microsoft.AspNetCore.Mvc.Rendering;
using PartFinderCore.Models;

namespace PartFinderCore.Classes;

public class FootprintCategoryHelpers
{
    public static List<SelectListItem> BuildNestedCategories(int selectedId, bool addRoot = false)
    {
        List<SelectListItem> menu = [];
        List<CategoryMenu> categories = [];

        using var dbContext = new SiteContext();
        var catItems = dbContext.FootprintCategory.OrderBy(p => p.FCName).ToList();
        categories.AddRange(catItems.Select(c => new CategoryMenu { Name = c.FCName, PrimaryKey = c.FCPkey, ParentId = c.ParentCategory }));

        var nestedCategories = BuildNestedCategories(categories);
        if (addRoot)
        {
            menu.Add(new SelectListItem
            {
                Text = "Root",
                Value = "0"
            });
        }
        GenNestedCategories(nestedCategories, menu, selectedId);
        return menu;

    }
    private static void GenNestedCategories(List<NestedCategory> nestedCategories, ICollection<SelectListItem> menu, int selectedId)
    {
        foreach (var category in nestedCategories)
        {
            menu.Add(new SelectListItem
            {
                Text = category.FullPath,
                Value = category.PrimaryKey.ToString(),
                Selected = category.PrimaryKey.ToString() == selectedId.ToString()
            });
            GenNestedCategories(category.Children, menu, selectedId);
        }
    }

    private static List<NestedCategory> BuildNestedCategories(List<CategoryMenu> categories)
    {
        var nestedCategories = new List<NestedCategory>();

        // Create a map to store parent-child relationships
        var parentChildMap = new Dictionary<int, List<NestedCategory>>();

        foreach (var category in categories)
        {
            var nestedCategory = new NestedCategory
            {
                Name = category.Name,
                PrimaryKey = category.PrimaryKey,
                FullPath = category.Name
            };

            if (category.ParentId == 0)
            {
                // Top-level category
                nestedCategories.Add(nestedCategory);
            }
            else
            {
                // Child category
                if (!parentChildMap.TryGetValue(category.ParentId, out var value))
                {
                    value = [];
                    parentChildMap[category.ParentId] = value;
                }

                value.Add(nestedCategory);
            }
        }

        // Link children to their parents and update FullPath
        foreach (var parentCategory in nestedCategories)
        {
            AddChildren(parentCategory, parentChildMap);
        }

        return nestedCategories;
    }

    private static void AddChildren(NestedCategory parentCategory, IReadOnlyDictionary<int, List<NestedCategory>> parentChildMap)
    {
        if (!parentChildMap.TryGetValue(parentCategory.PrimaryKey, out var children)) return;
        foreach (var child in children)
        {
            child.FullPath = $"{parentCategory.FullPath} - {child.Name}";
            parentCategory.Children.Add(child);
            AddChildren(child, parentChildMap);
        }
    }
}