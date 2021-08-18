using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //Section4: 
    // To avoid defining these attributes in all controller files, created a common controller file.
    // Moved the above two attributes [ApiController], [Route] from Controller files
    // and inheriting this controller from ControllerBase here.
    public class BaseApiController: ControllerBase
    {
        
    }
}