using System.ComponentModel.DataAnnotations;

namespace PartFinderCore.Models;

public class PartStockLevelHistory
{
    [Key]
    public int HistoryPkey { get; init; }
    public int PartPkey { get; init; }
    public int StockLevel { get; init; }
    public DateTime DateChanged { get; set; } = DateTime.Now;
}