
namespace Models;

public class Transaction
{
    public int Id {get;set;}
    public int IdUser {get;set;}
    public DateTime CreateDate {get;set;}
    public DateTime SendDate {get;set;}
    public string Status {get;set;}
    public string Receipt {get;set;}
    public int Total {get;set;}
    public IEnumerable<TransactionDetail>? TransactionDetails { get; set; }
}