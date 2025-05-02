using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using TWMSServer.Model;
using TWMSServer.Providers;

namespace TWMSServer.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class WorkOrderController
    {

        [HttpPost("approve")]
        public async Task<IActionResult> Approve(List<int> workOrders)
        {

            List<int> list = workOrders;


            

            return new OkObjectResult(list);
        }
    }
}
