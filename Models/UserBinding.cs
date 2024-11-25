using Microsoft.AspNetCore.Components.Forms;

public class UserBindings
{
    public int UserNum { get; set; }  
    public string? UserText { get; set; }  
    public string? UserEmail { get; set; }  
    public string? UserNumber { get; set; }  
    public string? UserDropDown { get; set; }  
    public bool UserRequired { get; set; } = false;
    public string? UserColor { get; set; }
    public DateTime? UserDate { get; set; }   

    public DateTime? UserDateTime { get; set; }  
    public TimeSpan? UserTime { get; set; }  
    public int UserRange { get; set; }  
    public string? UserRadio { get; set; }  
    
}