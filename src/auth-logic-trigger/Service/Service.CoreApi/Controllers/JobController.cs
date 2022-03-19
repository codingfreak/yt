namespace codingfreaks.LogicAppSample.Service.CoreApi.Controllers
{
	using Microsoft.AspNetCore.Authorization;
	using Microsoft.AspNetCore.Mvc;
	using System;
	using System.Linq;

	[ApiController]
	[Authorize]
	[Route("[controller]")]
	public class JobController : ControllerBase
	{
		#region member vars

		private readonly ILogger<JobController> _logger;

		#endregion

		#region constructors and destructors

		public JobController(ILogger<JobController> logger)
		{
			_logger = logger;
		}

		#endregion

		#region methods

		[HttpPost(Name = "StartJob")]
		public IActionResult StartJob()
		{
			return Ok();
		}

		#endregion
	}
}
