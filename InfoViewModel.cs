using IMNAT.School.Models;

namespace IMNAT.School.ViewModels
{
    public class InfoViewModel
    {
        public Courses Student { get; set; }
        public Address Address { get; set; }
        public string Title { get; set; }
        public string Header { get; set; }
    }
}
