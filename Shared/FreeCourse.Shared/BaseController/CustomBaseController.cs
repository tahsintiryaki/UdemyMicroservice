using FreeCourse.Shared.Dtos;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace FreeCourse.Shared.BaseController
{
    public class CustomBaseController:ControllerBase
    {
        //CustomBaseController'in ControllerBase'ten kalıtım alabilmesi için "FreeCourse.Shared" projesine sağ tık > Edit Project File deyip "<FrameworkReference Include="Microsoft.AspNetCore.App"/>" satırı ekledik.

        public IActionResult CreateActionResultInstance<T>(Response<T> response)
        {
            return new ObjectResult(response)
            {
                StatusCode = response.StatusCode
            };
        }
    }
}
