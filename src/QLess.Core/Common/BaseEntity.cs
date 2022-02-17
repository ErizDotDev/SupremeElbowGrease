using System.ComponentModel.DataAnnotations;

namespace QLess.Core.Common
{
	public class BaseEntity
	{
		[Key]
		public long Id { get; set; }
	}
}
