namespace TravelWebApi.Utils;

public class GenericFilter<T> where T : class
{
    public string CurrentSearch { get; set; }
    public string CurrentOrder { get; set; }
    public string CurrentOrderType { get; set; }
    public IEnumerable<T> Result { get; set; }
}