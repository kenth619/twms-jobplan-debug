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
        private readonly TWMSServerContext _context;

        public WorkOrderController(TWMSServerContext context)
        {
            _context = context;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateNewWorkOrder([FromBody]string? ok)
        {
            //string ok = "";
            return new OkObjectResult(ok);

            //WorkOrder newOrder = new()
            //{
            //    RequestedBy = o.RequestedBy,
            //    Area = o.Area,
            //    Section = o.Section,
            //    AssociatedWONumber = o.AssociatedWONumber,
            //    JobType = o.JobType,
            //    JobTypeSubCategory = o.JobTypeSubCategory,
            //    Priority = o.Priority,
            //    SourceDocumentType = o.SourceDocumentType,
            //    SourceDocumentNumber = o.SourceDocumentNumber,
            //    Account = o.Account,
            //    CostCentre = o.CostCentre,
            //    JobNumber = o.JobNumber,
            //    Status = "New",
            //    DateCreated = DateTime.Now
            //};

            //_context.tblWorkOrderHeader.Add(newOrder);

            //return new OkObjectResult(newOrder);
        }



        [HttpPost("approve")]
        public async Task<IActionResult> Approve(List<int> workOrders)
        {
            List<int> list = workOrders;
            return new OkObjectResult(list);
        }
    }
}
