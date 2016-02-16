using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for VehicleLogDTO
/// </summary>
public class VehicleLogDTO
{
    public String vehicle_plate_no { get; set; }
    public double odometer { get; set; }
    public double last_fuel_price { get; set; }
    public double total_gas { get; set; }
    public double total_cost { get; set; }
    public String date { get; set; }
    public String full_tank { get; set; }
    public double result { get; set; }
    public String driver_username {get; set;}
    public String audit_user { get; set; }
    public String audit_date { get; set; }

}