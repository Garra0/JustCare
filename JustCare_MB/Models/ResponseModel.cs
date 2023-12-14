using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JustCare_MB.Models;
public class ResponseModel
{
    public bool Success { get; set; }
    public string Message { get; set; }
    public int StatusCode { get; set; }
}