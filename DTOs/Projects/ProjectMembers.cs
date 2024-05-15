using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Volontyor_Hakaton.Models;

namespace Volontyor_Hakaton.DTOs.Projects
{
    public class ProjectMembers
    {
        [Key]
        public int up_Id { get; set; }
        public string ProjectName {  get; set; }
        public int ProjectId { get; set; }
        public string UserInfo { get; set; }
        public int UserId { get; set; }
        public int Score { get; set; } = 1; // 1 ~ 5

    }
}
