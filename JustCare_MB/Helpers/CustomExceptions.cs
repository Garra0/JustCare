using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JustCare_MB.Helpers;
//public class CustomExceptions
//{
//}

public class InvalidUserPasswordOrUserNotExistException : Exception
{
    public InvalidUserPasswordOrUserNotExistException() { }
    public InvalidUserPasswordOrUserNotExistException(string Password)
        : base(String.Format(Password)) { }
}

public class NotFoundException : Exception
{
    public NotFoundException() { }
    public NotFoundException(string notFound)
        : base(String.Format(notFound)) { }
}

public class EmptyFieldException : Exception
{
    public EmptyFieldException() { }
    public EmptyFieldException(string emptyField)
        : base(String.Format(emptyField)) { }
}

public class ExistsException : Exception
{
    public ExistsException() { }
    public ExistsException(string exists)
        : base(String.Format(exists)) { }
}

public class InvalidIdException : Exception
{
    public InvalidIdException() { }
    public InvalidIdException(string invalidIdException)
        : base(String.Format(invalidIdException)) { }
}

public class TimeNotValid : Exception
{
    public TimeNotValid() { }
    public TimeNotValid(string timeNotValid)
        : base(String.Format(timeNotValid)) { }
}

public class ImagesBadRequest : Exception
{
    public ImagesBadRequest() { }
    public ImagesBadRequest(string imagesBadRequest)
        : base(String.Format(imagesBadRequest)) { }
}

