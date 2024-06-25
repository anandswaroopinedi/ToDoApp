using System;
using System.Collections.Generic;

namespace DataAccessLayer.Entities;

public partial class ErrorLog
{
    public int Id { get; set; }

    public string ErrorMessage { get; set; } = null!;

    public string FileName { get; set; } = null!;

    public string MethodName { get; set; } = null!;

    public string TimeStamp { get; set; } = null!;

    public int LineNumber { get; set; }

    public string StackTrace { get; set; } = null!;
}
