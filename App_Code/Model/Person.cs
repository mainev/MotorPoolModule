using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

/// <summary>
/// Summary description for Person
/// </summary>
/// 
[Serializable()]
public class Person
{
   
    public int Id { get; set; }
    public String FirstName { get; set; }
    public String MiddleName { get; set; }
    public String LastName { get; set; }
    public String PhoneNumber { get; set; }
    public String Gender { get; set; }
    public String Email { get; set; }    
    public DateTime Birthdate { get; set; }

}
