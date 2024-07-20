using System;
using fazz.Models.Entities;
using fazz.Models.Responses;

namespace fazz.Models.Responses
{
	public class ClientDataResponse
	{
		public List<ClientData> ClientDataList { get; set; } = new List<ClientData>();
	}
}

