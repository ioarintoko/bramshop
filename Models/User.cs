namespace Models;

public class User
{
    public int Id {get;set;}
    public string Name {get;set;}
    public required string Email {get;set;}
    public required string Password {get;set;}
    public string Address {get;set;}
    public string Phone {get;set;}
}
