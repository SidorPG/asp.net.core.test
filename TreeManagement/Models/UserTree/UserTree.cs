namespace Api.Models.UserTree;

public class UserTree
{
    public int id { get; set; }
    public string name { get; set; }
    public List<UserTree> children { get; set; }
}