using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TetrisWeb.DTOs;

public class ChatDto
{
	public int Id { get; set; }

	public int PlayerId { get; set; }
	public string? PlayerUsername {get; set;}

	public string? Message { get; set; }

	public DateTime? TimeSent { get; set; }
}

