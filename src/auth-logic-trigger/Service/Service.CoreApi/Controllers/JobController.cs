namespace codingfreaks.LogicAppSample.Service.CoreApi.Controllers
{
	using Microsoft.AspNetCore.Authorization;
	using Microsoft.AspNetCore.Mvc;
	using System;
	using System.Linq;

	/// <summary>
	/// Provides methods to control jobs.
	/// </summary>
	[ApiController]
	[Route("api/v{version:apiVersion}/[controller]")]
	public class JobController : ControllerBase
	{
		#region constructors and destructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		/// <param name="logger">The logger to use.</param>
		public JobController(ILogger<JobController> logger)
		{
			Logger = logger;
		}

		#endregion

		#region methods

		/// <summary>
		/// Queues a new job.
		/// </summary>
		[HttpPost(Name = "StartJob")]
		public IActionResult StartJob()
		{
			return Ok();
		}

		#endregion

		#region properties

		/// <summary>
		/// The injected logger.
		/// </summary>
		private ILogger<JobController> Logger { get; }

		#endregion
	}
}
