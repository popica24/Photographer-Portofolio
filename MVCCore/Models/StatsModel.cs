

using System.ComponentModel.DataAnnotations;

public class StatsModel
{
    [Key]
    public string Id { get; set; }
    public int Events { get; set; }
    public int Reviews { get; set; }
    public int Trophies { get; set; }
}